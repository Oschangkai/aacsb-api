using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AACSB.WebApi.Application.Common.Requests;

public abstract class HttpRequest : IHttpRequest
{
    private readonly HttpClient _client;

    private string _baseAddress = string.Empty;
    private string _mediaType = "application/json";
    private string _token = string.Empty;

    private ILogger<HttpRequest> Logger { get; }

    protected HttpRequest(string? baseAddress, string? mediaType, string? token, ILogger<HttpRequest> logger)
    {
        if (baseAddress is not null)
            _baseAddress = baseAddress;
        if (mediaType is not null)
            _mediaType = mediaType;
        if (token is not null)
            this._token = token;
        Logger = logger;

        _client = new HttpClient()
        {
            BaseAddress = new Uri($"{_baseAddress}")
        };
    }

    public Task<HttpResponseMessage> GetAsync(string path, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
    {
        string uri = GenerateUri(path, queryParams);
        return _client.GetAsync(uri, cancellationToken);
    }

    public Task<HttpResponseMessage> PostAsync(string path, string? postData, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
    {
        string uri = GenerateUri(path, queryParams);
        var contentPost = postData is null ? null : new StringContent(postData, Encoding.UTF8, "application/json");

        return _client.PostAsync(uri, contentPost, cancellationToken);
    }

    public Task<HttpResponseMessage> PostAsync<T>(string path, T? postData, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
    {
        string uri = GenerateUri(path, queryParams);
        string json = JsonSerializer.Serialize(postData);
        HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");

        return _client.PostAsync(uri, contentPost, cancellationToken);
    }

    private static string GenerateUri(string path, Dictionary<string, string>? queryParams)
    {
        string uri = $"{path}?";

        var query = new Dictionary<string, string>();
        if (queryParams is not null)
        {
            query = query.Concat(queryParams)
                .GroupBy(p => p.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }

        foreach ((string key, string value) in query)
        {
            uri += $"{key}={value}&";
        }

        return uri;
    }
}