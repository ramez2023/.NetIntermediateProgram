using WEBAPI.IntegrationTests.Extensions;
using WEBAPI.IntegrationTests.Services.Interfaces;

namespace WEBAPI.IntegrationTests.Services
{
    public class CategoryTestService : ICategoryTestService
    {
        public async Task<HttpResponseMessage> GetAll(object? query, HttpClient client)
        {
            var requestUri = $"/api/categories/{query.ConvertToQueryString()}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Get(int? id, HttpClient client)
        {
            var requestUri = $"/api/category/{id}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Create(object? category, HttpClient client)
        {
            var requestUri = "/api/category/add";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = category?.ReadAsJsonContent(),
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Update(object? category, HttpClient client)
        {
            var requestUri = "/api/category/update";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = category?.ReadAsJsonContent(),
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Delete(int? id, HttpClient client)
        {
            var requestUri = $"/api/category/delete/{id}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri, UriKind.Relative),
            };

            return await client.SendAsync(request);
        }
    }
}