using ArgipApiWpfConsume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Services
{
    public class SettingsData: ISettingsData
    {
        public void SaveSettings(SettingsModel settingsModel)
        {
            MySettingsProvider provider = new MySettingsProvider();

            //save data to store
            provider.Audience = settingsModel.Audience;
            provider.BaseApiAddress = settingsModel.BaseApiAddress;
            provider.ClientId = settingsModel.ClientId;
            provider.ClientSecret = settingsModel.ClientSecret;
            provider.TokenEndpoint = settingsModel.TokenEndpoint;

            provider.Save();
        }

        public SettingsModel ReadSettings()
        {
            MySettingsProvider provider = new MySettingsProvider();
            return new SettingsModel {
                Audience = provider.Audience,
                BaseApiAddress = provider.BaseApiAddress,
                ClientId = provider.ClientId,
                ClientSecret = provider.ClientSecret,
                TokenEndpoint = provider.TokenEndpoint
            };
        }
    }
}
