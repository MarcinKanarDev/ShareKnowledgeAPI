using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Mapper.DTOs
{
    public class CreatePostDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public List<CategoryDto>? CategoryDtos { get; set; }

    }
}
