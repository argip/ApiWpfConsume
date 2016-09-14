using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int BaseProductId { get; set; }
        public string Index { get; set; }
        public string ProductFullName { get; set; }
        public string EanBarcode { get; set; }
        public string MultiPackEanBarcode { get; set; }
        public int SinglePackQuantityInPieces { get; set; }
        public int MultiPackQuantityInPieces { get; set; }
        public int QuantityLimitLevel1 { get; set; }
        public int QuantityLimitLevel2 { get; set; }
        public string PictureUrl { get; set; }
        public decimal YourMainPrice { get; set; }
        public decimal YourPriceLevel1 { get; set; }
        public decimal YourPriceLevel2 { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int PiecesInStock { get; set; }
        public int OtherPossibleQuantity { get; set; }
        public string NearestDilivery { get; set; }
        public string YourIndex { get; set; }
        public string YourProductFullName { get; set; }
        public string YourEanBarcode { get; set; }
        public bool IsActive { get; set; }
        public decimal BoxWeight { get; set; }
        public decimal TaxRate { get; set; }
        public List<TagModel> Tags { get; set; }
    }

    public class TagModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
