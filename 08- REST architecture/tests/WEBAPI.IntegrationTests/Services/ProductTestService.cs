using WEBAPI.IntegrationTests.Extensions;
using WEBAPI.IntegrationTests.Services.Interfaces;

namespace WEBAPI.IntegrationTests.Services
{
    public class ProductTestService : IProductTestService
    {
        public async Task<HttpResponseMessage> GetAll(object? query, HttpClient client)
        {
            var requestUri = $"/api/products/{query.ConvertToQueryString()}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Get(int? id, HttpClient client)
        {
            var requestUri = $"/api/product/{id}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Create(object? product, HttpClient client)
        {
            var requestUri = "/api/product/add";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = product?.ReadAsJsonContent(),
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Update(object? product, HttpClient client)
        {
            var requestUri = "/api/product/update";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = product?.ReadAsJsonContent(),
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Delete(int? id, HttpClient client)
        {
            var requestUri = $"/api/product/delete/{id}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }
    }
}