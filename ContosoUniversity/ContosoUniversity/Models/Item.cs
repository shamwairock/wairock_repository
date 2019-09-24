using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public decimal Price { get; set; }
        public Guid ItemCategoryId { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
    }
}