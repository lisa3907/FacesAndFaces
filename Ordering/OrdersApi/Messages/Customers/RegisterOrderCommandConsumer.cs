using MassTransit;
using Messaging.InterfacesConstants.Commands;
using Newtonsoft.Json;
using OrdersApi.Models;
using OrdersApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrdersApi.Messages.Customers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        public RegisterOrderCommandConsumer(IOrderRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
        }
        public async Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var result = context.Message;

            if(result.OrderId != null && result.PictureUrl != null &&
                result.UserEmail != null && result.ImageData != null)
            {
                SaveOrder(result);

                var client = _httpClientFactory.CreateClient();
                Tuple<List<byte[]>, Guid> orderDetailData = await GetFacesFromFaceApiAsync(client, result.ImageData, result.OrderId);
                var faces = orderDetailData.Item1;
                var orderId = orderDetailData.Item2;
                SaveOrderDetails(orderId, faces);
            }
        }

        private void SaveOrderDetails(Guid orderId, List<byte[]> faces)
        {
            var order = _repository.GetOrderAsync(orderId).Result;
            if(order == null)
            {
                order.Status = Status.Processed;
                foreach (var face in faces)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        FaceData = face

                    };
                    order.OrderDetails.Add(orderDetail);
                }
                _repository.UpdateOrder(order);
            }
        }

        private async Task<Tuple<List<byte[]>, Guid>> GetFacesFromFaceApiAsync(HttpClient client, byte[] imageData, Guid orderId)
        {
            // Provides HTTP content based on a byte array. so diffrent than normal array
            var byteContent = new ByteArrayContent(imageData);
            Tuple<List<byte[]>, Guid> orderDetailData = null;
            using (var response = await client.PostAsync("http://localhost:6001/api/faces/" + orderId, byteContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                orderDetailData = JsonConvert.DeserializeObject<Tuple<List<byte[]>, Guid>>(apiResponse);
            }

            return orderDetailData;
        }

        private void SaveOrder(IRegisterOrderCommand result)
        {
            Order order = new Order
            {
                OrderId = result.OrderId,
                UserEmail = result.UserEmail,
                Status = Status.Registered,
                PictureUrl = result.PictureUrl,
                ImageData = result.ImageData

            };
            _repository.RegisterOrder(order);
        }
    }
}
