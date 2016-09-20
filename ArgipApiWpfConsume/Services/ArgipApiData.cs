using ArgipApiWpfConsume.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace ArgipApiWpfConsume.Services
{
    public class ArgipApiData : IArgipApiData
    {
        private readonly HttpClient client;


        public ArgipApiData()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DataCalcOrderModel> CalculateOrderAsync(string url, string accessToken, List<OrderItem> orderItems)
        {
            string serializedobject = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(orderItems));
            StringContent content = new StringContent(serializedobject, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            DataCalcOrderModel calculatedOrder = new DataCalcOrderModel();


            using (HttpResponseMessage response = await client.PostAsync(new Uri(url), content))
            {
                calculatedOrder.StatusCode = response.StatusCode.ToString();
                calculatedOrder.IntStatusCode = (int)response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    calculatedOrder.CalcOrderModel = new CalcOrderModel();
                    string respcontent = await response.Content.ReadAsStringAsync();
                    calculatedOrder.CalcOrderModel = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CalcOrderModel>(respcontent));
                }
            }

            return calculatedOrder;
        }

        public async Task<ProductsAndPagination> GetProdutcsAsync(string url, string accessToken)
        {
            ProductsAndPagination data = new ProductsAndPagination();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using (HttpResponseMessage response = await client.GetAsync(new Uri(url)))
            {
                
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    data.Products = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Product>>(content));                   
                    //jeszcze informacja o następnej stronie z Headera
                    IEnumerable <string> paginationjson;
                    if (response.Headers.TryGetValues("X-Pagination", out paginationjson))
                    {
                        data.Pagination = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Pagination>(paginationjson.FirstOrDefault()));
                    }
                    
                }
            }

            return data;
        }

        public async Task<string> UpdateProductAsync(string url, string accessToken, MapProduct mapproduct)
        {
            string statuscode = "";
            string serializedobject = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(mapproduct));
            StringContent content = new StringContent(serializedobject, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using (HttpResponseMessage response = await client.PutAsync(new Uri(url), content))
            {
                statuscode = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    Product product = new Product();
                    string respcontent = await response.Content.ReadAsStringAsync();
                    product = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Product>(respcontent)); // use this if you need
                }
            }

            return statuscode;
        }
    }
}
