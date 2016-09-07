using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Models
{
    public class Pagination
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string PrevPageLink { get; set; }
        public string NextPageLink { get; set; }
    }
}
