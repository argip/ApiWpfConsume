using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Models
{
    public class OrderModelItem
    {
        public Product Product { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public int PiecesOrdered { get; set; }
        public int PiecesReceived { get; set; }
        public decimal ItemNetValue { get; set; }
        public decimal ItemWeight { get; set; }
    }
}
