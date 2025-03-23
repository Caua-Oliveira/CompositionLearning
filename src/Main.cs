using CompositionLearning.src;
using CompositionLearning.src.Interfaces;
using static CompositionLearning.src.Item.ItemManager;

LoadItems();
Item axe = GetItem("steel_axe");
Tool axe_tool_component = axe.GetComponent<Tool>();
Weapon axe_weapon_component = axe.GetComponent<Weapon>();
Console.WriteLine(
    $"Axe name: {axe.Name}, Axe durability: {axe_tool_component.Durability}, Axe damage: {axe_weapon_component.Damage}");

