using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Pen : Item
    {
        public Color Color { get; set; }
    }
}