using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstanceFactory = BitSpread.Security.DataAccessFactory.DataAccessFactory;
using BitSpread.Security.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderAccessService orderAccessService;

        public OrdersController()
        {
            orderAccessService = InstanceFactory.CreateInstance();
        }

        // GET: api/Orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var orders = orderAccessService.GetOrders();
            return orders;
        }

        // GET: api/Orders/XYZ
        [HttpGet("{id}", Name = "Get")]
        public Order Get(string id)
        {
            var order = orderAccessService.GetOrders().Where(x => x.OrderId.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
