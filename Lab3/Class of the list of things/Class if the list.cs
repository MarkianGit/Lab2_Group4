namespace Lab3.Models
{
    class PackingItem
    {
        public string ItemName { get; set; }
        public bool IsPacked { get; set; }

        public PackingItem(string itemName)
        {
            ItemName = itemName;
            IsPacked = false;
        }

        public void PackItem()
        {
            IsPacked = true;
        }
    }
}