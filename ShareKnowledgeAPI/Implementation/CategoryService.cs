using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Authorization;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Exceptions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Models;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext applicationDbContext, IUserContextService contextService,
            IAuthorizationService authorizationService, IMapper mapper)
        {
            _authorizationService = authorizationService;
            _userContextService = contextService;
            _context = applicationDbContext;
            _mapper = mapper;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.
                FirstOrDefaultAsync(c => c.Id == categoryId);
                
            if (category is null)
                throw new NotFoundException($"Category not found");

            var authorizeResult = _authorizationService.AuthorizeAsync(_userContextService.User, category,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizeResult.Succeeded)
            {
                throw new ForbidException("You don't have an access to this resorce.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(Query query)
        {
            var categories = await _context.Categories
                .Where(p => query.SearchPhrase == null ||
                    p.CategoryName.ToLower().Contains(query.SearchPhrase.ToLower()))
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return categoryDtos;
        }
    }
}
