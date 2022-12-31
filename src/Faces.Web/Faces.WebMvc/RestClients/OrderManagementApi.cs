using Faces.WebMvc.ViewModels;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faces.WebMvc.RestClients
{
    public class OrderManagementApi : IOrderManagementApi
    {
        // we can make request to OrdersApi via this variable thanks to Refit
        // no need for header type and de/serialize 
        // this needs to be set in startup class
        private readonly IOrderManagementApi _restClient;
        public OrderManagementApi(IConfiguration configuration, HttpClient httpClient)
        {
            // OrdersApi location server
            string apiHostAndPort = configuration.GetSection("ApiServiceLocations").
                GetValue<string>("OrdersApiLocation");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
            _restClient = RestService.For<IOrderManagementApi>(httpClient);
        }

        public async Task<OrderViewModel> GetOrderById(Guid orderId)
        {
            try
            {
                return await _restClient.GetOrderById(orderId);
            }
            catch (ApiException ex)
            {

                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // wuuuuut ??
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            return await _restClient.GetOrders();
        }
    }
}
