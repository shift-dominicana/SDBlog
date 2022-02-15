using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Categories;
using SDBlog.DataModel.Entities.Categories;

namespace SDBlog.BusinessLayer.Mappers
{
    public class CategoryMap : Profile
    {
        public CategoryMap()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap();
        }
    }
}
