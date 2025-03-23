using CompositionLearning.src.Interfaces;
using Newtonsoft.Json.Linq;

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
            string jsonFilePath = "C:\\Users\\Player 1\\Documents\\C# Projects\\CompositionLearning\\itemData.json";
            string json = File.ReadAllText(jsonFilePath);

            JObject jsonData = JObject.Parse(json);

            // Iterate through each category (MeleeWeapons, Tools, etc.)
            foreach (var category in jsonData)
            {
                JArray itemsArray = (JArray)category.Value;

                foreach (JObject itemObj in itemsArray)
                {
                    string id = itemObj["Id"].ToString();
                    string name = itemObj["Name"].ToString();
                    string description = itemObj["Description"].ToString();
                    string type = itemObj["Type"].ToString();
                    int maxStack = itemObj["MaxStack"].ToObject<int>();
                    bool isStackable = itemObj["IsStackable"].ToObject<bool>();

                    Item item = new(id, name, description, type, maxStack, isStackable);

                    // Process and add each component if available.
                    if (itemObj["Components"] != null)
                    {
                        foreach (JObject comp in itemObj["Components"]!)
                        {
                            string compType = comp["Type"]!.ToString();
                            IItemComponent? component = null;

                            switch (compType)
                            {
                                case "Weapon":
                                    int damage = comp["Damage"].ToObject<int>();
                                    string damageType = comp["DamageType"].ToString();
                                    float attackSpeed = comp["AttackSpeed"].ToObject<float>();
                                    component = new Weapon(damage, damageType, attackSpeed);
                                    break;

                                case "Tool":
                                    string toolType = comp["ToolType"].ToString();
                                    int miningPower = comp["MiningPower"].ToObject<int>();
                                    int durability = comp["Durability"].ToObject<int>();
                                    component = new Tool(toolType, miningPower, durability);
                                    break;

                                case "Armor":
                                    int defense = comp["Defense"].ToObject<int>();
                                    string weight = comp["Weight"].ToString();
                                    component = new Armor(defense, weight);
                                    break;

                                case "Consumable":
                                    string effect = comp["Effect"].ToString();
                                    int value = comp["Value"].ToObject<int>();
                                    bool isInstant = comp["IsInstant"].ToObject<bool>();
                                    float duration = comp["Duration"].ToObject<float>();
                                    component = new Consumable(effect, value, isInstant, duration);
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