namespace projectValery42B.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int NumberOfOrders { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name}, {Address}, Поръчки: {NumberOfOrders}";
        }
    }
}
