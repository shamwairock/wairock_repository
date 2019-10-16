using System;

namespace ObserverDesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Stock microsoft = new Stock(){Code = "MSFT", Price = 99, Quantity = 100};
            Stock apple = new Stock() { Code = "APPL", Price = 156, Quantity = 100 };

            ExchangeBroker gic = new ExchangeBroker(){BrokerCode = "GIC"};
            ExchangeBroker interBroker = new ExchangeBroker(){BrokerCode = "IB"};

            gic.AddToWatchList(microsoft);
            gic.AddToWatchList(apple);

            interBroker.AddToWatchList(apple);

            microsoft.Price = 99.2;
            apple.Price = 157.5;
            microsoft.Price = 99.6;
            microsoft.Price = 99.9;
            apple.Price = 156.5;

            Console.Read();
        }
    }
}
