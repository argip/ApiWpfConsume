using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Models
{
    public class ProductsAndPagination
    {
        public List<Product> Products { get; set; }
        public Pagination Pagination { get; set; }

        public ProductsAndPagination()
        {
            Products = new List<Product>();
            Pagination = new Pagination();
        }
    }
}
