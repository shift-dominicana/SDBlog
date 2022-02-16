using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Posts;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Mappers
{
    public class PostMap : Profile
    {
        public PostMap()
        {
            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}
