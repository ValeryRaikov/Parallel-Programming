using projectValery42B.Models;

namespace projectValery42B.Data
{
    public class CustomerRepository
    {
        private readonly List<Customer> _customers;
        private readonly Random _rand = new Random();

        public CustomerRepository()
        {
            _customers = new List<Customer>
            {
                new() { Id = 1, Name = "Иван Иванов", Address = "ул. Първа 1", NumberOfOrders = 5 },
                new() { Id = 2, Name = "Мария Петрова", Address = "ул. Втора 2", NumberOfOrders = 3 },
                new() { Id = 3, Name = "Георги Георгиев", Address = "ул. Трета 3", NumberOfOrders = 8 },
                new() { Id = 4, Name = "Елена Димитрова", Address = "ул. Четвърта 4", NumberOfOrders = 2 },
                new() { Id = 5, Name = "Николай Николов", Address = "ул. Пета 5", NumberOfOrders = 7 },
                new() { Id = 6, Name = "Стамат Пешов", Address = "ул. Шеста 6", NumberOfOrders = 4 },
                new() { Id = 7, Name = "Марин Митев", Address = "ул. Седма 7", NumberOfOrders = 2 },
                new() { Id = 8, Name = "Виктория Георгиева", Address = "ул. Осма 8", NumberOfOrders = 1 },
                new() { Id = 9, Name = "Лили Филипова", Address = "ул. Девета 9", NumberOfOrders = 6 },
                new() { Id = 10, Name = "Никола Михов", Address = "ул. Десета 10", NumberOfOrders = 5 },
            };
        }

        public IEnumerable<Customer> GetAllCustomers() => _customers;

        // "Бавно" търсене на клиент по ID — със Thread.Sleep вместо await Task.Delay
        public Customer? FindCustomerById(int id)
        {
            int delay = _rand.Next(500, 5000);
            Thread.Sleep(delay);
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        // Паралелно търсене с Thread вместо Task.WhenAll
        public Customer[] FindCustomers(IEnumerable<int> ids)
        {
            var foundCustomers = new List<Customer>();
            var threads = new List<Thread>();
            var lockObj = new object();

            foreach (var id in ids)
            {
                var thread = new Thread(() =>
                {
                    var result = FindCustomerById(id);
                    if (result != null)
                    {
                        lock (lockObj)
                        {
                            foundCustomers.Add(result);
                        }
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            // Изчакваме всички нишки
            foreach (var t in threads)
                t.Join();

            return foundCustomers.ToArray();
        }
    }
}
