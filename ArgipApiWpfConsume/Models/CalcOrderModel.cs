using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Models
{
    public class CalcOrderModel
    {
        public decimal TotalWeight { get; set; }
        public decimal TotalNetValue { get; set; }
        public decimal TotalGrossValue { get; set; }
        public decimal TaxValue { get; set; }
        public string CurrencyName { get; set; }
        public List<OrderModelItem> Items { get; set; }
    }

    public class DataCalcOrderModel
    {
        public string StatusCode { get; set; }
        public int IntStatusCode { get; set; }
        public CalcOrderModel CalcOrderModel { get; set; }
    }
}
