using ArgipApiWpfConsume.Abstractions;
using ArgipApiWpfConsume.Services;
using ArgipApiWpfConsume.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume
{
    class Bootstrapper : BootstrapperBase
    {
        SimpleContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<IShell, MainWindowViewModel>();
            container.PerRequest<IArgipApiData, ArgipApiData>();
            container.Singleton<ISettingsData, SettingsData>();
            container.Singleton<AccessTokenService>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
