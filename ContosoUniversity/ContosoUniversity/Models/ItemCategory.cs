using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class ItemCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ItemCategoryID { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}