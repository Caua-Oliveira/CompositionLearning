namespace CompositionLearning;

public class Item
{
    // Required
    public string id = "itemxd";
    public string name = "itemxd";
    public string description;
    public string type;
    public int default_price;
    public int maxStack = 16;
    public bool isStackable = true;

    // Possible
    public int damage;
    public int mining_power;
    public int defense;
    public int healing;


}

