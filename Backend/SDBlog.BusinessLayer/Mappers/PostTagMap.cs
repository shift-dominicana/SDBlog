using AutoMapper;
using SDBlog.BusinessLayer.Dtos.PostTags;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Mappers
{
    public class PostTagMap : Profile
    {
        public PostTagMap()
        {
            CreateMap<PostTag, PostTagDto>().ReverseMap();
        }
    }
}
