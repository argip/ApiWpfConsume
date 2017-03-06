using System.Collections.Generic;
using System.Threading.Tasks;
using ArgipApiWpfConsume.Models;

namespace ArgipApiWpfConsume.Services
{
    public interface IArgipApiData
    {
        Task<ProductsAndPagination> GetProdutcsAsync(string url, string accessToken);
        Task<string> UpdateProductAsync(string url, string accessToken, MapProduct mapproduct);
        Task<DataCalcOrderModel> CalculateOrderAsync(string url, string accessToken, List<OrderItem> orderItems);
        Task<Product> GetProductAsync(string url, string accessToken);
    }
}