namespace WEBAPI.IntegrationTests.Extensions
{
    public static class RequestHelper
    {
        public static string ConvertToQueryString(this object? queryParameters)
        {
            if (queryParameters == null)
                return string.Empty;

            var properties = queryParameters.GetType().GetProperties();
            var queryString = string.Join("&", properties.Select(p => $"{p.Name}={p.GetValue(queryParameters)}"));
            return queryString.Length > 0 ? $"?{queryString}" : "";
        }
    }
}