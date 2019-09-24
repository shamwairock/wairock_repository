using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BitSpread.Security.DataModel;
using BitSpread.Security.Exceptions;
using BitSpread.Security.Interfaces;

namespace BitSpread.Security.DataAccess
{
    public class OrderAccessMock : IOrderAccess
    {
        public string MockOrdersPath { get; set; }

        public string MockOrdersSummaryPath { get; set; }

        public double GetLastClosingPrice()
        {
            try
            {
                var lines = File.ReadAllLines(MockOrdersSummaryPath);

                IList<Tuple<DateTime, double, double>> tuples = new List<Tuple<DateTime, double, double>>();

                if (lines == null)
                {
                    throw new InvalidClosingPriceException("Failed to get last closing price.");
                }

                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    var chunks = line.Split('|');
                    DateTime.TryParse(chunks[0], out var dateTime);
                    double.TryParse(chunks[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double openingPrice);
                    double.TryParse(chunks[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double closingPrice);

                    tuples.Add(new Tuple<DateTime, double, double>(dateTime, openingPrice, closingPrice));
                }

                var lastSummary = tuples.OrderByDescending(x => x.Item1).FirstOrDefault();

                if (lastSummary == null)
                {
                    throw new InvalidClosingPriceException("Failed to get last closing price.");
                }
                else
                {
                    return lastSummary.Item3;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidClosingPriceException("Failed to get last closing price.", ex);
            }
        }

        public List<Order> GetOrders()
        {
            var lines = File.ReadAllLines(MockOrdersPath);
            var orders = new List<Order>();

            if (lines != null)
            {
                var index = 1;
                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    var chunks = line.Split('|');
                    double.TryParse(chunks[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double volume);
                    double.TryParse(chunks[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double price);
                    Enum.TryParse(chunks[2], true, out OrderType orderType);
                    var securityCode = chunks[3].ToString();

                    var order = new Order()
                    {
                        OrderId = index++.ToString(),
                        Timestamp = DateTime.Now,
                        Volume = volume,
                        Price = price,
                        OrderType = orderType,
                        SecurityCode = securityCode
                    };

                    orders.Add(order);
                }

            }

            return orders;
        }
    }
}
