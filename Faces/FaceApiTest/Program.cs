using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FaceApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = @"office.png";
            var urlAddress = "http://localhost:6001/api/faces/{0}";
            var uriWithParm = new Uri(string.Format(urlAddress, Guid.NewGuid()));
            ImageUtility imageUtility = new ImageUtility();
            var bytes = imageUtility.ConvertToBytes(imagePath);
            Tuple<List<byte[]>, Guid> faceTuple = null;
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.PostAsync(uriWithParm, byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faceTuple = JsonConvert.DeserializeObject<Tuple<List<byte[]>, Guid>>(apiResponse);

                }
            }
            if (faceTuple.Item1.Count > 0)
            {
                for (int i = 0; i < faceTuple.Item1.Count; i++)
                {
                    imageUtility.FromBytesToImage(faceTuple.Item1[i], "face" + i);
                }
            }
        }
    }
}
