using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.ViewModels
{
    public class DialogViewModel : Conductor<object>
    {

        public DialogViewModel()
        {
            Buttons = new BindableCollection<int>(Enumerable.Range(1, 3));
        }

        public void SomeAction()
        {
            Debug.Print("SomeAction called");
        }

        public void SomeActionWithParameter(int value)
        {
            Debug.Print("SomeActionWithParameter called through bubbling with value={0}", value);
        }

        public BindableCollection<int> Buttons { get; private set; }
    }
}
