using CompositionLearning.src.Interfaces;
using CompositionLearning.src.Systems;
using static CompositionLearning.src.Systems.Item.ItemManager;

LoadItems();
Item axe = GetItem("steel_axe");
Console.WriteLine(
    $"Axe name: {axe.Name}, " +
    $"Axe durability: {axe.GetComponent<DurableComponent>()?.Durability}, " +
    $"Axe damage: {axe.GetComponent<WeaponComponent>()?.Damage}"
);