using System;
using CompositionLearning.src;
using CompositionLearning.src.Interfaces;
using static CompositionLearning.src.Item.ItemManager;


LoadItems();
Item axe = GetItem("steel_axe");
Console.WriteLine(
    $"Axe name: {axe.Name}, " +
    $"Axe durability: {axe.GetComponent<DurableComponent>().Durability},  " +
    $"Axe damage: {axe.GetComponent<WeaponComponent>().Damage}"
);
