using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.Application.CQRS.Queries;
using NewsApi.Application.DTO.NewsApiOrg;
using NewsApi.Application.Enums.NewsApiOrg;
using NewsApi.Infrastructure.Authentication;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ArticlesResultDto), 200)]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [HttpGet("[action]")]
        public async Task<ActionResult<ArticlesResultDto>> GetRecentNewsByTitleKeywordAsync(string keyword, Language language, int articlesCount = 5)
        {
            var result = await _mediator.Send(new GetRecentNewsByTitleKeywordQuery.Request
            {
                TitleKeyword = keyword,
                Language = language,
                ArticlesCount = articlesCount,
            });

            return Ok(result);
        }
    }
}
