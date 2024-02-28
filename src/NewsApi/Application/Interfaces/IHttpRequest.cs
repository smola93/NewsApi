namespace NewsApi.Application.Interfaces
{
    public interface IHttpRequest<T> where T : class
    {
        Task<T> MakeRequest(string baseUrl, string path, string? querystring, HttpMethod httpMethod, CancellationToken cancellationToken = default);
    }
}
