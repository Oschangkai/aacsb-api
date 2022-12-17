namespace AACSB.WebApi.Application.Common.Requests;

public interface IHttpRequest : IScopedService
{
    public Task<HttpResponseMessage> GetAsync(string path, Dictionary<string, string>? queryParams, CancellationToken cancellationToken);
    public Task<HttpResponseMessage> PostAsync(string path, string? postData, Dictionary<string, string>? queryParams, CancellationToken cancellationToken);
    public Task<HttpResponseMessage> PostAsync<T>(string path, T? postData, Dictionary<string, string>? queryParams, CancellationToken cancellationToken);
}