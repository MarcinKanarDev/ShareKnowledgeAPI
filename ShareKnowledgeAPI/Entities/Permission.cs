using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
    }
}