using System.Text;
using Newtonsoft.Json;

namespace WEBAPI.IntegrationTests.Extensions;

public static class HttpResponseExtensions
{
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpResponseMessage response)
    {
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
        }
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent httpContent)
    {
        var responseContent = await httpContent.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public static StringContent ReadAsJsonContent(this Object? obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj ?? new()), Encoding.UTF8, "application/json");
    }

}