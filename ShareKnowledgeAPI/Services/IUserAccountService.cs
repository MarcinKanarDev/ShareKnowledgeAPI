using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Services
{
    public interface IUserAccountService
    {
        void RegisterUser(UserRegisterDto userRegisterDto);
        string GenerateJwt(LoginDto loginDto);
        IEnumerable<User> GetAll();
    }
}
