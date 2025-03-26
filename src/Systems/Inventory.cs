namespace CompositionLearning.src.Systems;

public class Inventory
{
    private readonly List<InventorySlot?> _items;
    public const int MaxSlots = 10;

    public Inventory()
    {
        _items = Enumerable.Repeat<InventorySlot?>(null, MaxSlots).ToList();
    }

    private class InventorySlot
    {
        public Item Item { get; }
        public int Quantity { get; private set; }

        public InventorySlot(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public int Add(int quantity)
        {
            int spaceLeft = Item.MaxStack - Quantity;
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
            return $"Item: {Item.Name}, Quantity: {Quantity}";
        }
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Inventory Contents:");
        for (int i = 0; i < _items.Count; i++)
        {
            sb.AppendLine(_items[i] != null
                ? $"Slot {i}: {_items[i]}"
                : $"Slot {i}: Empty");
        }
        return sb.ToString();
    }

    public void AddItem(Item item, int quantity)
    {
        while (quantity > 0)
        {
            if (item.IsStackable)
            {
                InventorySlot? existingSlot = _items.FirstOrDefault(slot =>
                    slot?.Item.Id == item.Id && slot.Quantity < item.MaxStack);

                if (existingSlot != null)
                {
                    int added = existingSlot.Add(quantity);
                    quantity -= added;
                    continue;
                }
            }

            int emptyIndex = _items.FindIndex(s => s == null);
            if (emptyIndex == -1)
            {
                Console.WriteLine("Inventory is full!");
                return;
            }

            if (item.IsStackable)
            {
                _items[emptyIndex] = new InventorySlot(item, 0);
                int added = _items[emptyIndex]!.Add(quantity);
                quantity -= added;
                continue;
            }

            var emptyIndices = _items
                .Select((s, i) => s == null ? i : -1)
                .Where(i => i != -1)
                .Take(quantity)
                .ToList();

            if (emptyIndices.Count < quantity)
            {
                Console.WriteLine("Not enough slots for all non-stackable items");
                quantity = emptyIndices.Count;
            }

            foreach (int index in emptyIndices)
            {
                _items[index] = new InventorySlot(item, 1);
            }
            quantity = 0;
        }
    }

    public void RemoveItemById(string id, int quantity)
    {
        if (ContainsId(id, quantity))
        {
            while (quantity > 0)
            {
                InventorySlot? existingSlot = _items.FirstOrDefault(slot => slot?.Item.Id == id);
                if (existingSlot == null) break;

                int removed = existingSlot.Remove(quantity);
                quantity -= removed;

                if (existingSlot.IsEmpty)
                {
                    int index = _items.IndexOf(existingSlot);
                    _items[index] = null;
                }
            }
        }
        else
        {
            Console.WriteLine("Not enough items to remove");
        }
    }

    public Item? RemoveItemByIndex(int index, int quantity)
    {
        if (index < 0 || index >= _items.Count)
        {
            Console.WriteLine("Invalid inventory slot index!");
            return null;
        }

        if (_items[index] == null)
        {
            Console.WriteLine("Slot is empty!");
            return null;
        }

        Item itemRemoved = _items[index]!.Item;
        int quantityRemoved = _items[index]!.Remove(quantity);

        if (_items[index]!.IsEmpty)
        {
            _items[index] = null;
        }

        return itemRemoved;
    }

    public int CountId(string id)
    {
        return _items
            .Where(slot => slot?.Item.Id == id)
            .Sum(slot => slot?.Quantity ?? 0);
    }

    public bool ContainsId(string id, int min = 1)
    {
        return CountId(id) >= min;
    }
}
