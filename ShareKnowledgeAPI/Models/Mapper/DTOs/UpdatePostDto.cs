using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Mapper.DTOs
{
    public class UpdatePostDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
