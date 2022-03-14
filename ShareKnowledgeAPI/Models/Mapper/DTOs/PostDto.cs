using ShareKnowledgeAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Mapper.DTOs
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<CommentDto> Comments { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}
