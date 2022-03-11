using System.Security.Claims;

namespace ShareKnowledgeAPI.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}