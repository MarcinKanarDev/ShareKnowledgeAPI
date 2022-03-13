using Microsoft.AspNetCore.Authorization;
using ShareKnowledgeAPI.Entities;
using System.Security.Claims;

namespace ShareKnowledgeAPI.Authorization
{
    public class ResourceOperationRequirementHandler
            : AuthorizationHandler<ResourceOperationRequirement, Post>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ResourceOperationRequirement requirement, Post post)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create
                ) 
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (post.CreatedById == int.Parse(userId)) 
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
