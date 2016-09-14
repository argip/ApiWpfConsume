using ArgipApiWpfConsume.Models;
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
        private readonly Product product;
        int byquantity;
        int bypackages;

        public DialogViewModel(Product product)
        {
            this.product = product;
            ByPackages = 1;
            ByQuantity = product.SinglePackQuantityInPieces;
        }


        public int ByQuantity
        {
            get { return byquantity; }
            set
            {
                byquantity = value;
                NotifyOfPropertyChange(() => ByQuantity);
            }
        }

        public int ByPackages
        {
            get { return bypackages; }
            set
            {
                bypackages = value;
                NotifyOfPropertyChange(() => ByPackages);
            }
        }

        //public BindableCollection<int> Buttons { get; private set; }
    }
}
