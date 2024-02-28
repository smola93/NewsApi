using NewsApi.Application.Enums.NewsApiOrg;

namespace NewsApi.Application.Contracts.NewsApiOrg
{
    public class EverythingRequest
    {
        public required string Q { get; set; }
        public SearchIn? SearchIn { get; set; }
        public List<string>? Sources { get; set; }
        public List<string>? Domains { get; set; }
        public List<string>? ExcludeDomains { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Language? Language { get; set; }
        public SortBy? SortBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
