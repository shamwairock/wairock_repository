﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Book : Item
    {
        public string Author { get; set; }
    }
}