using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Post> Posts  { get; set; }
    }
}
