namespace CompositionLearning.src.Interfaces;


public class IItemComponent { }

public class WeaponComponent : IItemComponent
{
    public int Damage { get; set; }
    public string DamageType { get; set; }
    public float AttackSpeed { get; set; }

    public WeaponComponent(int damage, string damageType, float attackSpeed)
    {
        Damage = damage;
        DamageType = damageType;
        AttackSpeed = attackSpeed;
        
    }

    public override string ToString()
    {
        return $"Damage: {Damage}, Damage Type: {DamageType}, Attack Speed: {AttackSpeed}";
    }
}

// Component for tool-related behavior
public class ToolComponent : IItemComponent
{
    public string ToolType { get; set; }
    public int MiningPower { get; set; }

    public ToolComponent(string toolType, int miningPower)
    {
        ToolType = toolType;
        MiningPower = miningPower;
        
    }

    public override string ToString()
    {
        return $"Tool Type: {ToolType}, Mining Power: {MiningPower}";
    }
}

// Component for armor properties
public class ArmorComponent : IItemComponent
{
    public int Defense { get; set; }
    public string Weight { get; set; }

    public ArmorComponent(int defense, string weight)
    {
        Defense = defense;
        Weight = weight;
    }

    public override string ToString()
    {
        return $"Defense: {Defense}, Weight: {Weight}";
    }
}

// Component for consumable properties
public class ConsumableComponent : IItemComponent
{
    public string Effect { get; set; }
    public int Value { get; set; }
    public bool IsInstant { get; set; }
    public float Duration { get; set; }
    public ConsumableComponent(string effect, int value, bool isInstant, float duration)
    {
        Effect = effect;
        Value = value;
        IsInstant = isInstant;
        Duration = duration;
    }

    public override string ToString()
    {
        return $"Effect: {Effect}, Value: {Value}, Is Instant: {IsInstant}, Duration: {Duration}";
    }
}

public class DurableComponent: IItemComponent
{
    public int Durability { get; set; }

    public DurableComponent(int durability)
    {
        Durability = durability;
    }

    public override string ToString()
    {
        return $"Durability: {Durability}";
    }
}
