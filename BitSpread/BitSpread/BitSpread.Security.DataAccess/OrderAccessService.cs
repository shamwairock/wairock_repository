using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BitSpread.Security.DataModel;
using BitSpread.Security.Interfaces;

namespace BitSpread.Security.DataAccess
{
    public class OrderAccessService : IOrderAccessService
    {
        public List<Order> GetOrders()
        {
            var lines = File.ReadAllLines(@"MockOrders.txt");
            var orders = new List<Order>();

            if (lines != null)
            {
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

                    var order = new Order()
                    {
                        Timestamp = DateTime.Now,
                        Volume = volume,
                        Price = price,
                        OrderType = orderType
                    };

                    orders.Add(order);
                }

            }

            return orders;
        }
    }
}
