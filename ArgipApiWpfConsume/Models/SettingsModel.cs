using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Models
{
    public class SettingsModel
    {
        public string Audience { get; set; }
        public string TokenEndpoint { get; set; }
        public string BaseApiAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
