using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Tags;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Mappers
{
    public class TagMap : Profile
    {
        public TagMap()
        {
            CreateMap<Tag, TagDto>().ReverseMap();
        }
    }

}
