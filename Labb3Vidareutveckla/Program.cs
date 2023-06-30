namespace Labb3Vidareutveckla
{
    // Interface representing a warm drink
    public interface IWarmDrink
    {
        void Consume();
    }
    // Implementing interface IWarmDrink
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served.");
        }
    }
    // Implementing interface IWarmDrink
    internal class HotChocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot Chocholate is served.");
        }
    }
    // Implementing interface IWarmDrink
    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Coffee is served.");
        }
    }
    // Implementing interface IWarmDrink
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served.");
        }
    }
    // Interface for factories preparing hot beverages
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }
    // A factory to prepare hot water and its implementing the IWarmDrinkFactory interface
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup");
            return new Water();
        }
    }
    // A factory to prepare hot chocholate and its implementing the IWarmDrinkFactory interface
    internal class HotChocolateFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot Chocolate in your cup");
            return new HotChocolate();
        }
    }
    // A factory to prepare coffee and its implementing the IWarmDrinkFactory interface
    internal class CoffeeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml Coffee in your cup");
            return new Coffee();
        }
    }
    // A factory to prepare cappuccino and its implementing the IWarmDrinkFactory interface
    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml Cappuccino in your cup");
            return new Cappuccino();
        }
    }
    // Class WarmDrinkMachine that handles hot beverages
    public class WarmDrinkMachine
    {
        // Enum containing the beverages available
        public enum AvailableDrink // violates open-closed
        {
            Water,
            Coffee,
            Cappuccino,
            HotChocolate,
        }
        private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
          new Dictionary<AvailableDrink, IWarmDrinkFactory>(); // Dictionary that stores the factories

        private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
          new List<Tuple<string, IWarmDrinkFactory>>(); // List to store the names and factories for the beverages

        public WarmDrinkMachine()
        {
            foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    namedFactories.Add(Tuple.Create(
                      t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }
        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }
            Console.WriteLine("Select a number to continue:");
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i) // c# 7
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Console.Write("How much: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return namedFactories[i].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine();
            IWarmDrink drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}