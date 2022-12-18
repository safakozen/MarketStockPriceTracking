using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wigros.Web.Models
{
    public class ProductPrice
    {
        public List<Product> Product { get; set; }
        public List<Price> Price { get; set; }
    }
}