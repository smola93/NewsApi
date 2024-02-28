namespace NewsApi.Application.DTO.NewsApiOrg
{
    public class ArticlesResultDto
    {
        public required string Status { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
        public int TotalResults { get; set; }
        public List<ArticleDto>? Articles { get; set; }
    }
}
