using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgipApiWpfConsume.Services
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int QuantityInPieces { get; set; }
    }

    public class CartHolder
    {
        private List<CartItem> CartItems { get; set; }

        public CartHolder()
        {
            CartItems = new List<CartItem>();
        }

        public int CountCart()
        {
            return CartItems.Count();
        }

        public void AddItem(CartItem newitem)
        {
            var itemToCheck = CartItems.FirstOrDefault(r => r.ProductId == newitem.ProductId); 
            if (itemToCheck != null) // autopack
            {
                itemToCheck.QuantityInPieces += newitem.QuantityInPieces;
            }
            else
            {
                CartItems.Add(newitem);
            }
        }

        public void RemoveItem(int productId)
        {
            var itemToRemove = CartItems.FirstOrDefault(r => r.ProductId == productId);
            if(itemToRemove != null) CartItems.Remove(itemToRemove);
        }

        public List<CartItem> ListCartItems()
        {
            return CartItems;
        }

        public void CleanCart()
        {
            CartItems.RemoveAll(x => x.ProductId > 0);
        }
    }
}
