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
        int quantityYouNeed;
        int quantityYouReceive;
        int boxesYouReceive;
        decimal weightSum;

        public DialogViewModel(Product product)
        {
            this.product = product;
            if (product.PiecesInStock > 0)
            {
                BoxesYouReceive = 1;
                //QuantityYouReceive = product.SinglePackQuantityInPieces;
                QuantityYouNeed = product.SinglePackQuantityInPieces;
            }
            else
            {
                BoxesYouReceive = 0;
                QuantityYouNeed = 0;
            }
        }


        public int QuantityYouNeed
        {
            get { return quantityYouNeed; }
            set
            {
                quantityYouNeed = value;
                if(quantityYouNeed > 0)
                {
                    if (quantityYouNeed <= product.SinglePackQuantityInPieces)
                    {
                        QuantityYouReceive = product.SinglePackQuantityInPieces;
                        BoxesYouReceive = 1;
                    }
                    else
                    {
                        if (quantityYouNeed % product.SinglePackQuantityInPieces > 0)
                        {
                            int myDiv = (int)(quantityYouNeed / product.SinglePackQuantityInPieces);
                            QuantityYouReceive = (myDiv * product.SinglePackQuantityInPieces) + product.SinglePackQuantityInPieces;
                            BoxesYouReceive = myDiv + 1;
                        }

                        if (quantityYouNeed % product.SinglePackQuantityInPieces == 0)
                        {
                            int myDiv = (int)(quantityYouNeed / product.SinglePackQuantityInPieces);
                            QuantityYouReceive = (myDiv * product.SinglePackQuantityInPieces);
                            BoxesYouReceive = myDiv;
                        }
                    }

                    WeightSum = Math.Round(product.BoxWeight * BoxesYouReceive, 2);
                }
                else
                {
                    BoxesYouReceive = 0;
                    QuantityYouReceive = 0;
                    WeightSum = 0;
                }

                NotifyOfPropertyChange(() => QuantityYouNeed);
            }
        }

        public int QuantityYouReceive
        {
            get { return quantityYouReceive; }
            set
            {
                quantityYouReceive = value;
                NotifyOfPropertyChange(() => QuantityYouReceive);
            }
        }

        public int BoxesYouReceive
        {
            get { return boxesYouReceive; }
            set
            {
                boxesYouReceive = value;
                if (boxesYouReceive > 0)
                {

                    QuantityYouReceive = boxesYouReceive * product.SinglePackQuantityInPieces;
                    WeightSum = Math.Round(product.BoxWeight * boxesYouReceive, 2);
                }
                else
                {
                    QuantityYouReceive = 0;
                }


                NotifyOfPropertyChange(() => BoxesYouReceive);
            }
        }

        public decimal WeightSum
        {
            get { return weightSum; }
            set
            {
                weightSum = value;
                NotifyOfPropertyChange(() => WeightSum);
            }
        }

        public void BoxesUp()
        {
            BoxesYouReceive++;
        }

        public void BoxesDown()
        {
            BoxesYouReceive--;
        }
        //public BindableCollection<int> Buttons { get; private set; }
    }
}
