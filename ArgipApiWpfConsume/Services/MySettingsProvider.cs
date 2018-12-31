using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Services
{

    public class MySettingsProvider : ApplicationSettingsBase
    {

        [UserScopedSetting()]
        [DefaultSettingValue(@"https://identityserver.argip.com.pl/connect/token")]
        public string TokenEndpoint
        {
            get
            {
                return ((string)this["TokenEndpoint"]);
            }
            set
            {
                this["TokenEndpoint"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue(@"https://argipapi.argip.com.pl/")]
        public string BaseApiAddress
        {
            get
            {
                return ((string)this["BaseApiAddress"]);
            }
            set
            {
                this["BaseApiAddress"] = value;
            }
        }


        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ClientId
        {
            get
            {
                return ((string)this["ClientId"]);
            }
            set
            {
                this["ClientId"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ClientSecret
        {
            get
            {
                return ((string)this["ClientSecret"]);
            }
            set
            {
                this["ClientSecret"] = value;
            }
        }
    }
}
