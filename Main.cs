namespace CompositionLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Inventory inv = new Inventory();
            Item i = new Item();
            inv.AddItem(i, 200);
            inv.RemoveItemByID("itemx", 17);
            inv.AddItem(i, 3);
            inv.ToString();


        }
    }
}