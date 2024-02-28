using NewsApi.Application.Contracts.NewsApiOrg;
using NewsApi.Application.DTO.NewsApiOrg;

namespace NewsApi.Application.Interfaces
{
    public interface INewsApiOrgHttpClient
    {
        Task<ArticlesResultDto> GetEverythingAsync(EverythingRequest request, CancellationToken cancellationToken = default);
    }
}