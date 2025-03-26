using CompositionLearning.src.Interfaces;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace CompositionLearning.src;

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
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.Type = type;
        this.MaxStack = maxStack;
        this.IsStackable = isStackable;
    }

    public static class ItemManager
    {
        private static readonly Dictionary<string, Item> _items = [];

        // Load item definitions from a JSON file.
        public static void LoadItems()
        {
            string json = File.ReadAllText("itemData.json");

            // Deserialize JSON into a dictionary where each category holds a list of items as JsonElement.
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Dictionary<string, List<JsonElement>>? jsonData = JsonSerializer.Deserialize<Dictionary<string, List<JsonElement>>>(json, options);

            if (jsonData == null)
            {
                Console.WriteLine("Failed to deserialize JSON.");
                return;
            }

            // Iterate through each category (MeleeWeapons, Tools, etc.)
            foreach (var category in jsonData)
            {
                foreach (JsonElement itemElement in category.Value)
                {
                    // Extract item properties.
                    string id = itemElement.GetProperty("Id").GetString()!;
                    string name = itemElement.GetProperty("Name").GetString()!;
                    string description = itemElement.GetProperty("Description").GetString()!;
                    string type = itemElement.GetProperty("Type").GetString()!;
                    int maxStack = itemElement.GetProperty("MaxStack").GetInt32();
                    bool isStackable = itemElement.GetProperty("IsStackable").GetBoolean();

                    // Create a new item instance.
                    Item item = new(id, name, description, type, maxStack, isStackable);

                    // Process and add each component if available.
                    if (itemElement.TryGetProperty("Components", out JsonElement componentsElement) &&
                        componentsElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement comp in componentsElement.EnumerateArray())
                        {
                            string compType = comp.GetProperty("Type").GetString()!;
                            IItemComponent? component = null;

                            switch (compType)
                            {
                                case "WeaponComponent":
                                    int damage = comp.GetProperty("Damage").GetInt32();
                                    string damageType = comp.GetProperty("DamageType").GetString()!;
                                    float attackSpeed = comp.GetProperty("AttackSpeed").GetSingle();
                                    component = new WeaponComponent(damage, damageType, attackSpeed);
                                    break;

                                case "ToolComponent":
                                    string toolType = comp.GetProperty("ToolType").GetString()!;
                                    int miningPower = comp.GetProperty("MiningPower").GetInt32();
                                    component = new ToolComponent(toolType, miningPower);
                                    break;

                                case "ArmorComponent":
                                    int defense = comp.GetProperty("Defense").GetInt32();
                                    string weight = comp.GetProperty("Weight").GetString()!;
                                    component = new ArmorComponent(defense, weight);
                                    break;

                                case "ConsumableComponent":
                                    string effect = comp.GetProperty("Effect").GetString()!;
                                    int value = comp.GetProperty("Value").GetInt32();
                                    bool isInstant = comp.GetProperty("IsInstant").GetBoolean();
                                    float duration = comp.GetProperty("Duration").GetSingle();
                                    component = new ConsumableComponent(effect, value, isInstant, duration);
                                    break;
                                case "DurabilityComponent":
                                    int durability = comp.GetProperty("Durability").GetInt32();
                                    component = new DurableComponent(durability);
                                    break;
                                default:
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

    public readonly List<IItemComponent> components = [];

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