using AutoMapper;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Mapper
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Post, PostDto>();
            
            CreateMap<Comment, CommentDto>();
            
            CreateMap<Category, CategoryDto>();

            CreateMap<CreatePostDto, Post>();
        }
    }
}
