using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Services
{
    public interface IUserAccountService
    {
        Task RegisterUser(UserRegisterDto userRegisterDto);
        string GenerateJwt(LoginDto loginDto);
        IEnumerable<User> GetAll();
    }
}
