namespace ShareKnowledgeAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string? UserImageUri { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }

    }
}
