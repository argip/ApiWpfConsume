using ArgipApiWpfConsume.Models;
using ArgipApiWpfConsume.Services;
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
        private readonly CartHolder cartHolder;
        int quantityYouNeed;
        int quantityYouReceive;
        int boxesYouReceive;
        decimal totalWeight;
        decimal actualPrice;
        string currencyName;
        decimal totalValue;

        public DialogViewModel(Product product, CartHolder cartHolder)
        {
            this.product = product;
            this.cartHolder = cartHolder;
            if (product.PiecesInStock > 0)
            {
                BoxesYouReceive = 1;
                QuantityYouNeed = product.SinglePackQuantityInPieces;
                ActualPrice = product.YourMainPrice;
                CurrencyName = product.CurrencyName;
            }
            else
            {
                BoxesYouReceive = 0;
                QuantityYouNeed = 0;
            }
        }


        public decimal TotalValue
        {
            get { return totalValue; }
            set
            {
                totalValue = value;
                NotifyOfPropertyChange(() => TotalValue);
            }
        }

        public decimal ActualPrice
        {
            get { return actualPrice; }
            set
            {
                actualPrice = value;
                NotifyOfPropertyChange(() => ActualPrice);
                TotalValue = Math.Round(actualPrice * (quantityYouReceive / 100m), 2, MidpointRounding.AwayFromZero);
            }
        }

        public string CurrencyName
        {
            get { return currencyName; }
            set
            {
                currencyName = value;
                NotifyOfPropertyChange(() => CurrencyName);
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

                    TotalWeight = Math.Round(product.BoxWeight * BoxesYouReceive, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    BoxesYouReceive = 0;
                    QuantityYouReceive = 0;
                    TotalWeight = 0;
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

                ActualPrice = product.YourMainPrice;

                if (product.QuantityLimitLevel1 != 0 && product.YourPriceLevel1 > 0 && quantityYouReceive >= product.QuantityLimitLevel1)
                {
                    ActualPrice = product.YourPriceLevel1;
                }

                if (product.QuantityLimitLevel2 != 0 && product.YourPriceLevel2 > 0 && quantityYouReceive >= product.QuantityLimitLevel2)
                {
                    ActualPrice = product.YourPriceLevel2;
                }

                NotifyOfPropertyChange(() => CanAddToCart);
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
                    TotalWeight = Math.Round(product.BoxWeight * boxesYouReceive, 2);
                }
                else
                {
                    QuantityYouReceive = 0;
                }


                NotifyOfPropertyChange(() => BoxesYouReceive);
            }
        }

        public decimal TotalWeight
        {
            get { return totalWeight; }
            set
            {
                totalWeight = value;
                NotifyOfPropertyChange(() => TotalWeight);
            }
        }

        public void BoxesUp()
        {
            BoxesYouReceive++;
        }

        public void BoxesDown()
        {
            if(boxesYouReceive > 0) BoxesYouReceive--;
        }

        public void AddToCart()
        {
            if(QuantityYouReceive > 0) cartHolder.AddItem(new CartItem { ProductId = product.ProductId, QuantityInPieces = QuantityYouReceive });
            TryClose();
        }

        public bool CanAddToCart
        {
            get { return QuantityYouReceive <= product.PiecesInStock; }
        }
    }
}
