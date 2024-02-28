using NewsApi.Application.Contracts.NewsApiOrg;
using NewsApi.Application.DTO.NewsApiOrg;
using NewsApi.Application.Interfaces;
using System.Text.Json;

namespace NewsApi.Infrastructure.HttpClients
{
    public class NewsApiOrgHttpClient : INewsApiOrgHttpClient, IHttpRequest<ArticlesResultDto>
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public NewsApiOrgHttpClient(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("NewsApiOrg").GetValue<string>("ApiKey") ??
                throw new ArgumentNullException("No newsapi.org ApiKey Found");
            _baseUrl = configuration.GetSection("NewsApiOrg").GetValue<string>("BaseUrl") ??
                throw new ArgumentNullException("No newsapi.org BaseUrl Found");

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("user-agent", "default-user-agent");
        }

        public async Task<ArticlesResultDto> GetEverythingAsync(EverythingRequest request, CancellationToken cancellationToken = default)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                queryParams.Add("q=" + request.Q);
            }
            if (request.SearchIn.HasValue)
            {
                queryParams.Add("searchIn=" + request.SearchIn.Value.ToString());
            }
            if (request.Sources != null && request.Sources.Count > 0)
            {
                queryParams.Add("sources=" + string.Join(",", request.Sources));
            }
            if (request.Domains != null && request.Domains.Count > 0)
            {
                queryParams.Add("domains=" + string.Join(",", request.Domains));
            }
            if (request.ExcludeDomains != null && request.ExcludeDomains.Count > 0)
            {
                queryParams.Add("excludeDomains=" + string.Join(",", request.ExcludeDomains));
            }
            if (request.From.HasValue)
            {
                queryParams.Add("from=" + string.Format("{0:s}", request.From.Value));
            }
            if (request.To.HasValue)
            {
                queryParams.Add("to=" + string.Format("{0:s}", request.To.Value));
            }
            if (request.Language.HasValue)
            {
                queryParams.Add("language=" + request.Language.Value.ToString().ToLowerInvariant());
            }
            if (request.SortBy.HasValue)
            {
                queryParams.Add("sortBy=" + request.SortBy.Value.ToString());
            }
            if (request.Page > 1)
            {
                queryParams.Add("page=" + request.Page);
            }
            if (request.PageSize > 0)
            {
                queryParams.Add("pageSize=" + request.PageSize);
            }

            var querystring = string.Join("&", queryParams.ToArray());

            return await MakeRequest(_baseUrl, "everything", querystring, HttpMethod.Get, cancellationToken);
        }

        public async Task<ArticlesResultDto> MakeRequest
            (string baseUrl, string path, string? querystring, HttpMethod httpMethod, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(httpMethod, baseUrl + path + "?" + querystring);
            var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);

            string? json = await httpResponse?.Content?.ReadAsStringAsync()!;

            if (!string.IsNullOrEmpty(json))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<ArticlesResultDto>(json, options);

                if (apiResponse != null)
                {
                    return apiResponse;
                }
                else
                {
                    return new ArticlesResultDto
                    {
                        Status = "error",
                        Code = "unexpectedError",
                        Message = "Could not deserialize API response"
                    };
                }
            }
            else
            {
                return new ArticlesResultDto
                {
                    Status = "error",
                    Code = "unexpectedError",
                    Message = "API did not respond to the provided request"
                };
            }
        }
    }
}
