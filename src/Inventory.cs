using static System.Reflection.Metadata.BlobBuilder;

namespace CompositionLearning;
public class Inventory
{
    private class InventorySlot(Item item, int quantity)
    {
        public Item Item { get; } = item;
        public int Quantity { get; private set; } = quantity;

        public int Add(int quantity)
        {
            int spaceLeft = Item.maxStack - Quantity;
            int quantityToAdd = Math.Min(spaceLeft, quantity);
            Quantity += quantityToAdd;
            return quantityToAdd;
        }

        public int Remove(int quantity)
        {
            int quantityToRemove = Math.Min(Quantity, quantity);
            Quantity -= quantityToRemove;
            return quantityToRemove;
        }

        public bool IsEmpty => Quantity <= 0;

        public override string ToString()
        {
            return $"Item: {Item.name}, Quantity: {Quantity}";
        }

    }

    private List<InventorySlot?> items;

    private readonly int MAX_SLOTS = 10;

    public Inventory()
    {
        items = [.. new InventorySlot[MAX_SLOTS]];
    }

    public override string ToString()
    {
        Console.WriteLine("Inventory Contents:");
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                Console.WriteLine($"Slot {i}: {items[i]}");
            }
            else
            {
                Console.WriteLine($"Slot {i}: Empty");
            }
        }
        return "";
    }

    public void AddItem(Item item, int quantity)
    {
        while (quantity > 0) 
        {
            if (item.isStackable) 
            {
                InventorySlot? existing_slot = items.FirstOrDefault(slot => slot?.Item.id == item.id && slot.Quantity < item.maxStack);


                if (existing_slot != null) 
                {
                    int added = existing_slot.Add(quantity);
                    quantity -= added;
                    continue;
                }
            }

            int empty_index = items.FindIndex(s => s == null);
            if (empty_index == -1)
            {
                Console.WriteLine("Inventory is full!");
                return;
                
            }

            if (item.isStackable)
            {
                items[empty_index] = new InventorySlot(item, 0);
                int added = items[empty_index].Add(quantity);
                quantity -= added;
                continue;
            }

            var empty_indices = items.Select((s, i) => s == null ? i : -1).Where(i => i != -1).Take(quantity).ToList();
            if (empty_indices.Count < quantity)
            {
                Console.WriteLine("Not enough slots for all non-stackable items");
                quantity = empty_indices.Count;
            }
            foreach (int index in empty_indices)
            {
                items[index] = new InventorySlot(item, 1);
            }
            quantity = 0;
        }
    }

    public void RemoveItemByID(string id, int quantity)
    {
        if (ContainsID(id, quantity))
        {
            while (quantity > 0)
            {
                InventorySlot? existing_slot = items.FirstOrDefault(slot => slot?.Item.id == id);
                int removed = existing_slot.Remove(quantity);
                quantity -= removed;
                if (existing_slot.IsEmpty)
                {
                    items[items.IndexOf(existing_slot)] = null;
                }
            }
        }
        else
        {
            Console.WriteLine("Not enough items to remove");
        }

    }

    public int RemoveItemByIndex(int index, int quantity)
    {
        if (index < 0 || index >= items.Count)
        {
            Console.WriteLine("Invalid inventory slot index!");
            return 0;
        }

        if (items[index] == null)
        {
            Console.WriteLine("Slot is empty!");
            return 0;
        }

        int removed = items[index].Remove(quantity);
        
        if (items[index].IsEmpty)
        {
            items[index] = null;
        }

        return removed;
    }

    public int CountID(string id)
    {
        return items.Where(slot => slot?.Item.id == id).Sum(slot => slot.Quantity);
    }

    public bool ContainsID(string id, int min = 1) 
    {
        return CountID(id) >= min;
    }


}