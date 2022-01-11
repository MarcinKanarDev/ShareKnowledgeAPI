using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CommentText { get; set; }
        public int Brains { get; set; }

        //Relationship
        public virtual Post Post { get; set; }
    }
}
