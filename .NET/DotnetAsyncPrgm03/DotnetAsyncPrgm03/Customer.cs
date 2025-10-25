namespace DotnetAsyncPrgm03
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfOrders { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name}, {Address}, Поръчки: {NumberOfOrders}";
        }
    }
}
