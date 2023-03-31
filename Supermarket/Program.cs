namespace Supermarket
{
    class Programm
    {
        static void Main()
        {
            Menu menu = new Menu();
            menu.ShowMenu();
        }
    }

    class Menu
    {
        public void ShowMenu()
        {
            const string MenuServeClient = "1";
            const string MenuCreateClientsQueue = "2";
            const string MenuExit = "0";

            bool isExit = false;
            string userInput;
            Supermarket supermarket = new Supermarket();

            supermarket.FillStorage();

            while (isExit == false)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine(MenuServeClient + " - Обслужить клиента");
                Console.WriteLine(MenuCreateClientsQueue + " - Создать очередь клиентов");
                Console.WriteLine(MenuExit + " - Выход");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case MenuServeClient:
                        supermarket.ServeClient();
                        break;

                    case MenuCreateClientsQueue:
                        supermarket.CreateClient();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }
    }

    class Supermarket
    {
        private int _account;
        private List<Product> _storage = new List<Product>();
        private Queue<Client> _clients = new Queue<Client>();
        private Random _random = new Random();
        private Product[] _allProductTypes = new Product[]
        {
            new Product("Хлеб", 50),
            new Product("Молоко", 70),
            new Product("Яйца", 40),
            new Product("картошка", 45),
            new Product("Сметана", 70),
            new Product("Булка", 40),
            new Product("Жир", 20),
            new Product("Пиво", 140),
            new Product("Куриный пупок", 40),
            new Product("Кислый пупс", 100),
            new Product("Пюрешка", 70),
            new Product("Салат ВсемРад", 110),
            new Product("Салат ТухлыйСмрад", 20),
            new Product("Салат ВесёлыйВлад", 120),
            new Product("Мясо Птицы", 80),
            new Product("Мясо Рыбы", 70),
            new Product("Ни рыба - ни мясо", 60),
            new Product("Колбаса копчёная", 130),
            new Product("Колбаса варёная", 135),
            new Product("Цыплёнок жареный", 110),
            new Product("Цыплёнок пареный", 115),
            new Product("Чай", 80),
            new Product("Кофе", 70),
            new Product("Потанцуем", 145)
        };

        public void FillStorage()
        {
            for (int i = 0; i < _allProductTypes.Length; i++)
            {
                for (int j = 0; j < _random.Next(10, 100); j++)
                {
                    _storage.Add(_allProductTypes[i]);
                }
            }

            Console.WriteLine($"На складе {_storage.Count()} едениц продукции");
        }

        public void CreateClient()
        {
            int clientsCount = 10;
            int clientsEntered = 0;

            for (int i = 0; i < clientsCount; i++)
            {
                List<Product> shoppingCart = new List<Product>();

                if (_storage.Count > 0)
                {
                    for (int j = 0; j < _random.Next(5, 10); j++)
                    {
                        Product product = TakeProductFromSrorage(_random.Next(0, _storage.Count));

                        if (product != null)
                        {
                            shoppingCart.Add(product);
                        }
                    }

                    _clients.Enqueue(new Client(_random.Next(200, 500), shoppingCart));
                    clientsEntered++;
                }
                else
                {
                    Console.WriteLine("На складе кончились товары");
                    i = clientsCount;
                }
            }

            Console.WriteLine($"В очередь встало {clientsEntered} клиентов");
            Console.WriteLine($"Всего клиентов в очереди - {_clients.Count()}");
        }

        public void ServeClient()
        {
            if (_clients.Count > 0)
            {
                bool isPayed = false;

                Client client = _clients.Peek();
                List<Product> shoppingCart = client.GetShoppingCart();

                while (isPayed == false)
                {
                    int totalPrice = 0;

                    for (int i = 0; i < shoppingCart.Count(); i++)
                    {
                        totalPrice += shoppingCart[i].Price;
                    }
                    if (client.Money >= totalPrice)
                    {
                        _account += client.Pay(totalPrice);
                        _clients.Dequeue();
                        Console.WriteLine($"Товары на сумму {totalPrice} оплачены, клиент обслужен.");
                        Console.WriteLine($"Балланс магазина - {_account}");
                        Console.WriteLine($"Клиентов в очереди - {_clients.Count()}");
                        isPayed = true;
                    }
                    else
                    {
                        int itemIndex = _random.Next(0, shoppingCart.Count());
                        Console.WriteLine($"У клиента не хватило денег, товар - \"{shoppingCart[itemIndex].Name}\" выложен из корзины");
                        shoppingCart.RemoveAt(itemIndex);
                    }
                }
            }
            else
            {
                Console.WriteLine("Очередь клиентов пуста");
            }
        }

        private Product TakeProductFromSrorage(int index)
        {
            if (_storage.Count() > 0)
            {
                Product product = _storage[index];
                _storage.RemoveAt(index);
                return product;
            }
            else
            {
                return null;
            }
        }
    }

    class Client
    {
        public int Money { get; private set; }
        private List<Product> _shoppingCart = new List<Product>();

        public Client(int money, List<Product> shoppingCart)
        {
            Money = money;
            _shoppingCart = shoppingCart;
        }

        public int Pay(int price)
        {
            if (Money >= price)
            {
                Money -= price;
                return price;
            }
            else
            {
                Console.WriteLine("Не хватает денег");
                return 0;
            }
        }

        public List<Product> GetShoppingCart()
        {
            return _shoppingCart;
        }
    }

    class Product
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}