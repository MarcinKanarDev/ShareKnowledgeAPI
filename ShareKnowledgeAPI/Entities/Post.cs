using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int Brains { get; set; }

        //Relationships
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}
