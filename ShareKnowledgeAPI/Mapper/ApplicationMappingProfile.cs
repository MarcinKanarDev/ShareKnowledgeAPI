using AutoMapper;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Mapper
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            //Post profiles
            CreateMap<Post, PostDto>();
            CreateMap<CreatePostDto, Post>();
               

            //Comment profiles
            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, Comment>();
            
            //Catrgory profiles
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();


        }
    }
}
