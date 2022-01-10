using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
        public int Brains { get; set; }

        //Relationship
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
