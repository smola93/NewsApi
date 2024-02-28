using FluentValidation;
using MediatR;
using NewsApi.Application.Contracts.NewsApiOrg;
using NewsApi.Application.DTO.NewsApiOrg;
using NewsApi.Application.Enums.NewsApiOrg;
using NewsApi.Application.Interfaces;

namespace NewsApi.Application.CQRS.Queries
{
    public static class GetRecentNewsByTitleKeywordQuery
    {
        public class Request : IRequest<ArticlesResultDto>
        {
            public required string TitleKeyword { get; set; }
            public Language Language { get; set; }
            public int ArticlesCount { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.TitleKeyword).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Request, ArticlesResultDto>
        {
            private readonly INewsApiOrgHttpClient _newsApiOrgHttpClient;

            public Handler(INewsApiOrgHttpClient newsApiOrgHttpClient)
            {
                _newsApiOrgHttpClient = newsApiOrgHttpClient;
            }

            public async Task<ArticlesResultDto> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var everythingRequest = new EverythingRequest
                {
                    Q = request.TitleKeyword,
                    Language = request.Language,
                    PageSize = request.ArticlesCount,
                    SearchIn = SearchIn.Title, //Business requirement, ref: ticket-1
                };

                return await _newsApiOrgHttpClient.GetEverythingAsync(everythingRequest, cancellationToken);
            }
        }
    }
}
