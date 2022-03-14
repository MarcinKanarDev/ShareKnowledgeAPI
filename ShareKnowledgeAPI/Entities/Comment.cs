using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
