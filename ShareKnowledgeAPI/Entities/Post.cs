using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
       
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }

    }
}
