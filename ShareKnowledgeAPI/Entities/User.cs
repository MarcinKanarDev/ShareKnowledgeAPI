namespace ShareKnowledgeAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

    }
}
