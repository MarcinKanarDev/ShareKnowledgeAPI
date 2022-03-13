namespace ShareKnowledgeAPI.Models
{
    public class Query
    {
        public string? SearchPhrase { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
