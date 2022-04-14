using ShareKnowledgeAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShareKnowledgeAPI.Mapper.DTOs
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile UserImage { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int PermissionId { get; set; } = 2;
    }
}
