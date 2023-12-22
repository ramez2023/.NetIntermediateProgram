
namespace WEBAPI.IntegrationTests.Services.Interfaces;

public interface ICategoryTestService
{
    Task<HttpResponseMessage> GetAll(object? query, HttpClient client);
    Task<HttpResponseMessage> Get(int? query, HttpClient client);
    Task<HttpResponseMessage> Create(object? category, HttpClient client);
    Task<HttpResponseMessage> Update(object? category, HttpClient client);
    Task<HttpResponseMessage> Delete(int? id, HttpClient client);
}