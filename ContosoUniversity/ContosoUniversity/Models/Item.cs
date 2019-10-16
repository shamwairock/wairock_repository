using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public abstract class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ItemID { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public decimal Price { get; set; }

        public Guid ItemCategoryID { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }
    }
}