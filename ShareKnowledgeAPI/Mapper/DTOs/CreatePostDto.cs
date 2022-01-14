using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Mapper.DTOs
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
