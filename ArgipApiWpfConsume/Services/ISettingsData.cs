using ArgipApiWpfConsume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Services
{
    public interface ISettingsData
    {
        void SaveSettings(SettingsModel settingsModel);
        SettingsModel ReadSettings();
    }
}
