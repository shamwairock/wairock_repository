using System;

namespace ObserverDesignPattern
{
    public class ExchangeBroker : Observer
    {
        public string BrokerCode { get; set; }

        public void AddToWatchList(Stock stock)
        {
            stock.Attach(this);
        }

        public void DeleteFromWatchList(Stock stock)
        {
            stock.Detach(this);
        }

        public override void Update(Stock stock)
        {
            Console.WriteLine($"[BrokerCode:{BrokerCode}][StockCode:{stock.Code}][Price:{stock.Price}]");
        }
    }
}
