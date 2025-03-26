using CompositionLearning.src.Interfaces;
using Newtonsoft.Json.Linq;


namespace CompositionLearning.src.Systems;

public class Item
{
    public string Id;
    public string Name;
    public string Description;
    public string Type;
    public int MaxStack;
    public bool IsStackable;

    public Item(string id, string name, string description, string type, int maxStack, bool isStackable)
    {
        Id = id; Name = name;
        Description = description;
        Type = type;
        MaxStack = maxStack;
        IsStackable = isStackable;
    }

    public static class ItemManager
    {
        private static readonly Dictionary<string, Item> _items = new();

        // Load item definitions from a JSON file.
        public static void LoadItems()
        {
            string jsonFilePath = "itemData.json";

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"Error: JSON file not found at {jsonFilePath}");
                return;
            }

            string json = File.ReadAllText(jsonFilePath);
            JObject jsonData = JObject.Parse(json);

            // Iterate through each category (MeleeWeapons, Tools, etc.)
            foreach (var category in jsonData)
            {
                if (category.Value is not JArray itemsArray) continue;

                foreach (JObject itemObj in itemsArray)
                {
                    string? id = itemObj["Id"]?.ToString();
                    string? name = itemObj["Name"]?.ToString();
                    string? description = itemObj["Description"]?.ToString();
                    string? type = itemObj["Type"]?.ToString();
                    int maxStack = itemObj["MaxStack"]?.ToObject<int>() ?? 1;
                    bool isStackable = itemObj["IsStackable"]?.ToObject<bool>() ?? false;

                    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type))
                    {
                        Console.WriteLine("Skipping item with missing essential data.");
                        continue;
                    }

                    Item item = new(id, name, description ?? "", type, maxStack, isStackable);

                    // Process and add each component if available.
                    if (itemObj["Components"] is JArray componentsArray)
                    {
                        foreach (JObject comp in componentsArray)
                        {
                            string? compType = comp["Type"]?.ToString();
                            if (string.IsNullOrEmpty(compType)) continue;

                            IItemComponent? component = null;

                            switch (compType)
                            {
                                case "WeaponComponent":
                                    int damage = comp["Damage"]?.ToObject<int>() ?? 0;
                                    string damageType = comp["DamageType"]?.ToString() ?? "Unknown";
                                    float attackSpeed = comp["AttackSpeed"]?.ToObject<float>() ?? 1.0f;
                                    component = new WeaponComponent(damage, damageType, attackSpeed);
                                    break;

                                case "ToolComponent":
                                    string toolType = comp["ToolType"]?.ToString() ?? "Unknown";
                                    int miningPower = comp["MiningPower"]?.ToObject<int>() ?? 0;
                                    component = new ToolComponent(toolType, miningPower);
                                    break;

                                case "ArmorComponent":
                                    int defense = comp["Defense"]?.ToObject<int>() ?? 0;
                                    string weight = comp["Weight"]?.ToString() ?? "Medium";
                                    component = new ArmorComponent(defense, weight);
                                    break;

                                case "ConsumableComponent":
                                    string effect = comp["Effect"]?.ToString() ?? "None";
                                    int value = comp["Value"]?.ToObject<int>() ?? 0;
                                    bool isInstant = comp["IsInstant"]?.ToObject<bool>() ?? true;
                                    float duration = comp["Duration"]?.ToObject<float>() ?? 0.0f;
                                    component = new ConsumableComponent(effect, value, isInstant, duration);
                                    break;
                                case "DurableComponent":
                                    int durability = comp["Durability"]?.ToObject<int>() ?? 0;
                                    component = new DurableComponent(durability);
                                    break;
                            }

                            if (component != null)
                            {
                                item.AddComponent(component);
                            }
                        }
                    }

                    // Add the item to the dictionary.
                    _items[item.Id] = item;
                }
            }
        }

        public static Item? GetItem(string id)
        {
            return _items.TryGetValue(id, out var item) ? item : null;
        }
    }

    public readonly List<IItemComponent> components = new();

    public void AddComponent(IItemComponent component)
    {
        components.Add(component);
    }

    public T? GetComponent<T>() where T : IItemComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }

    public override string ToString()
    {
        string componentStrings = string.Join(", ", components.Select(c => c.ToString()));
        return $"Item: {Name}, Components: [{componentStrings}]";
    }
}
