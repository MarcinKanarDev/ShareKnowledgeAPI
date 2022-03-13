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
            CreateMap<UpdatePostDto, PostDto>();
               

            //Comment profiles
            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, Comment>();
            
            //Category profiles
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();


        }
    }
}
