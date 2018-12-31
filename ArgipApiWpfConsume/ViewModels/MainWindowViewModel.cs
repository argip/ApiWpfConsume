using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArgipApiWpfConsume.Abstractions;
using System.Windows;
using ArgipApiWpfConsume.Models;
using ArgipApiWpfConsume.Services;
using System.Web;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Dynamic;

namespace ArgipApiWpfConsume.ViewModels
{
    class MainWindowViewModel : PropertyChangedBase, IShell
    {
        readonly IWindowManager windowManager;
        readonly IArgipApiData argipApiData;
        readonly ISettingsData settingsData;
        readonly AccessTokenService accessTokenService;
        readonly CartHolder cartHolder;

        //settings
        string tokenendpoint { get; set; }
        string baseapiaddress { get; set; }
        string clientid { get; set; }
        string clientsecret { get; set; }

        // othrer data
        string filtertext;
        string customindex;
        string customname;
        string customean;
        string progressinfo;
        int productidforupdate;
        bool isbusy = false;
        string nextpageurl = "";
        string windowTitle = "Api demo app...";
        string cartItemsInfo = "Cart (0 items)";

        //cart data
        decimal cartTotalWeight { get; set; }
        decimal cartTotalNetValue { get; set; }
        decimal cartTotalGrossValue { get; set; }
        decimal cartTaxValue { get; set; }
        string cartCurrencyName { get; set; }
        public BindableCollection<OrderModelItem> CartItems { get; private set; }


        public BindableCollection<Product> ProductList
        {
            get; private set;
        }


        public BindableCollection<Product> ProductCustomIndex
        {
            get; private set;
        }

        public string TotalRecords
        {
            get { return string.Format("{0} records", ProductList.Count); }
        }

        //public string FindInfo
        private string findInfo;

        public string FindInfo
        {
            get { return findInfo; }
            set
            {
                findInfo = value;
                NotifyOfPropertyChange(() => FindInfo);
            }
        }


        public string WindowTitle
        {
            get { return windowTitle; }
        }

        public MainWindowViewModel(
            IWindowManager windowManager, 
            IArgipApiData argipApiData, 
            ISettingsData settingsData, 
            AccessTokenService accessTokenService,
            CartHolder cartHolder)
        {
            this.windowManager = windowManager;
            this.argipApiData = argipApiData;
            this.settingsData = settingsData;
            this.accessTokenService = accessTokenService;
            this.cartHolder = cartHolder;

            ProductList = new BindableCollection<Product>();
            ProductCustomIndex = new BindableCollection<Product>();

            // read settings from store
            var settings = settingsData.ReadSettings();
            BaseApiAddress = settings.BaseApiAddress;
            TokenEndpoint = settings.TokenEndpoint;
            ClientId = settings.ClientId;
            ClientSecret = settings.ClientSecret;
        }


        public string TokenEndpoint
        {
            get { return tokenendpoint; }
            set
            {
                tokenendpoint = value;
                NotifyOfPropertyChange(() => TokenEndpoint);
            }
        }
        public string BaseApiAddress
        {
            get { return baseapiaddress; }
            set
            {
                baseapiaddress = value;
                NotifyOfPropertyChange(() => BaseApiAddress);
            }
        }
        public string ClientId
        {
            get { return clientid; }
            set
            {
                clientid = value;
                NotifyOfPropertyChange(() => ClientId);
            }
        }
        public string ClientSecret
        {
            get { return clientsecret; }
            set
            {
                clientsecret = value;
                NotifyOfPropertyChange(() => ClientSecret);
            }
        }


        public string FilterText
        {
            get { return filtertext; }
            set
            {
                filtertext = value;
                NotifyOfPropertyChange(() => FilterText);
                //NotifyOfPropertyChange(() => CanFilterData);
                //NotifyOfPropertyChange(() => TotalRecords);
            }
        }

        public string ProgressInfo
        {
            get { return progressinfo; }
            set
            {
                progressinfo = value;
                NotifyOfPropertyChange(() => ProgressInfo);
            }
        }

        public string CustomIndex
        {
            get { return customindex; }
            set
            {
                customindex = value;
                NotifyOfPropertyChange(() => CustomIndex);
            }
        }

        public string CustomName
        {
            get { return customname; }
            set
            {
                customname = value;
                NotifyOfPropertyChange(() => CustomName);
            }
        }

        public string CustomEan
        {
            get { return customean; }
            set
            {
                customean = value;
                NotifyOfPropertyChange(() => CustomEan);
            }
        }

        public int ProductIdForUpdate
        {
            get { return productidforupdate; }
            set
            {
                productidforupdate = value;
                NotifyOfPropertyChange(() => ProductIdForUpdate);
            }
        }


        public string FilterQueryString
        {
            get { return string.IsNullOrEmpty(FilterText) ? "" : string.Format("?quickfilter={0}", HttpUtility.UrlEncode(FilterText)); }
        }

        public string NextPageUrl
        {
            get { return nextpageurl; }
            set
            {
                nextpageurl = value;
                NotifyOfPropertyChange(() => NextPageUrl);
                NotifyOfPropertyChange(() => CanNextPage);
                NotifyOfPropertyChange(() => TotalRecords);
            }
        }

        public string CartItemsInfo
        {
            get { return cartItemsInfo; }
            set
            {
                cartItemsInfo= value;
                NotifyOfPropertyChange(() => CartItemsInfo);
            }
        }

        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                isbusy = value;
                NotifyOfPropertyChange(() => IsBusy);
                NotifyOfPropertyChange(() => CanFilterData);
                NotifyOfPropertyChange(() => CanNextPage);
                NotifyOfPropertyChange(() => CanSaveCustomData);
            }
        }


        public bool CanFilterData
        {
            get { return !IsBusy; }
        }

        public bool CanSaveCustomData
        {
            get { return !IsBusy; }
        }

        public async void SaveCustomData()
        {
            IsBusy = true;
            ProgressInfo = "Getting access token...";
            string accessToken = await accessTokenService.GetAccessTokenAsync(ClientId, ClientSecret, TokenEndpoint);

            ProgressInfo = "Saving data...";
            MapProduct map = new MapProduct
            {
                ProductId = ProductIdForUpdate,
                EanBarcode = CustomEan,
                Index = CustomIndex,
                ProductFullName = CustomName
            };

            string result = await argipApiData.UpdateProductAsync(BaseApiAddress + @"v1/Products",accessToken, map);
            IsBusy = false;
            ProgressInfo = result;
            ClearFields();
        }

        private void ClearFields()
        {
            ProductIdForUpdate = 0;
            CustomEan = "";
            CustomIndex = "";
            CustomName = "";
        }

        public void SaveSettingsData()
        {
            IsBusy = true;
            settingsData.SaveSettings(new SettingsModel
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                BaseApiAddress = BaseApiAddress,
                TokenEndpoint = TokenEndpoint
            });
            IsBusy = false;
        }

        public async void DoFilterData(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;
            if (keyArgs != null && keyArgs.Key == Key.Enter)
            {
                await FilterData();
            }

        }

        private string findIndexText;

        public string FindIndexText
        {
            get { return findIndexText; }
            set
            {
                findIndexText = value;
                NotifyOfPropertyChange(() => FindIndexText);
            }
        }



        public async Task FindData()
        {
            IsBusy = true;
            ProductCustomIndex.Clear();
            FindInfo = "";
            ProgressInfo = "Getting access token...";
            string accessToken = await accessTokenService.GetAccessTokenAsync(ClientId, ClientSecret, TokenEndpoint);
            ProgressInfo = "Getting data...";
            var dane = await argipApiData.GetProductAsync(BaseApiAddress + @"/v1/Products/YourIndex/" + FindIndexText, accessToken);
            if (dane != null) ProductCustomIndex.Add(dane);
            else FindInfo = "No product found.";
            IsBusy = false;
            ProgressInfo = "OK";
            NotifyOfPropertyChange(() => ProductCustomIndex);
        }

        public async Task FilterData()
        {
            IsBusy = true;
            
            NextPageUrl = "";
            ProductList.Clear();
            ProgressInfo = "Getting access token...";
            string accessToken = await accessTokenService.GetAccessTokenAsync(ClientId, ClientSecret, TokenEndpoint);
            ProgressInfo = "Getting data...";
            var dane = await argipApiData.GetProdutcsAsync(BaseApiAddress + @"v1/Products/0/200/true" + FilterQueryString, accessToken);
            ProductList.AddRange(dane.Products);
            IsBusy = false;
            ProgressInfo = "OK";
            // jeśli jest następna strona, to poinformuj
            if (!string.IsNullOrEmpty(dane.Pagination.NextPageLink))
            {
                NextPageUrl = dane.Pagination.NextPageLink;
            }
            else
            {
                NextPageUrl = "";
            }

            NotifyOfPropertyChange(() => ProductList);
        }

        public bool CanNextPage
        {
            get { return !string.IsNullOrEmpty(NextPageUrl) && !IsBusy; }
        }

        public async void NextPage()
        {
            IsBusy = true;
            ProgressInfo = "Getting access token...";
            string accessToken = await accessTokenService.GetAccessTokenAsync(ClientId, ClientSecret, TokenEndpoint);
            ProgressInfo = "Getting data...";
            var dane = await argipApiData.GetProdutcsAsync(NextPageUrl, accessToken);

            ProductList.AddRange(dane.Products);
            IsBusy = false;
            ProgressInfo = "OK";

            if (!string.IsNullOrEmpty(dane.Pagination.NextPageLink))
            {
                NextPageUrl = dane.Pagination.NextPageLink;
            }
            else
            {
                NextPageUrl = "";
            }

            NotifyOfPropertyChange(() => ProductList);
        }

        public async void AddToCart(Product product)
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.MinWidth = 300;
            settings.Title = "Quantity dialog";

            var result = windowManager.ShowDialog(new DialogViewModel(product, cartHolder), null, settings);
            CartItemsInfo = string.Format("Cart ({0} items)", cartHolder.CountCart());

            //calculating order

            IsBusy = true;
            ProgressInfo = "Getting access token...";
            string accessToken = await accessTokenService.GetAccessTokenAsync(ClientId, ClientSecret, TokenEndpoint);
            ProgressInfo = "Calculating cart...";
            List<OrderItem> orderItems = cartHolder.ListCartItems().Select(x => new OrderItem { ProductId = x.ProductId, QuantityInPieces = x.QuantityInPieces }).ToList();
            var dane = await argipApiData.CalculateOrderAsync(BaseApiAddress + @"v1/Order/Calculate", accessToken, orderItems);
            ProgressInfo = dane.StatusCode;

            CartTotalWeight = dane.CalcOrderModel.TotalWeight;
            CartTotalNetValue = dane.CalcOrderModel.TotalNetValue;
            CartTotalGrossValue = dane.CalcOrderModel.TotalGrossValue;
            CartTaxValue = dane.CalcOrderModel.TaxValue;
            CartCurrencyName = dane.CalcOrderModel.CurrencyName;
            
        }

        public void CleanCart()
        {
            cartHolder.CleanCart();

            CartTotalWeight = 0;
            CartTotalNetValue = 0;
            CartTotalGrossValue = 0;
            CartTaxValue = 0;
            CartItemsInfo = string.Format("Cart ({0} items)", 0);
        }

        public decimal CartTotalWeight
        {
            get { return cartTotalWeight; }
            set
            {
                cartTotalWeight = value;
                NotifyOfPropertyChange(() => CartTotalWeight);
            }
        }

        public decimal CartTotalNetValue
        {
            get { return cartTotalNetValue; }
            set
            {
                cartTotalNetValue = value;
                NotifyOfPropertyChange(() => CartTotalNetValue);
            }
        }

        public decimal CartTotalGrossValue
        {
            get { return cartTotalGrossValue; }
            set
            {
                cartTotalGrossValue = value;
                NotifyOfPropertyChange(() => CartTotalGrossValue);
            }
        }

        public decimal CartTaxValue
        {
            get { return cartTaxValue; }
            set
            {
                cartTaxValue = value;
                NotifyOfPropertyChange(() => CartTaxValue);
            }
        }

        public string CartCurrencyName
        {
            get { return cartCurrencyName; }
            set
            {
                cartCurrencyName = value;
                NotifyOfPropertyChange(() => CartCurrencyName);
            }
        }
    }
}
