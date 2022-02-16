using AutoMapper;
using SDBlog.Api.Controllers.Base;
using SDBlog.BusinessLayer.Dtos.Categories;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.DataModel.Entities;

namespace SDBlog.Api.Controllers
{
    public class CategoryController : BaseController<Category, CategoryDto>
    {
        public CategoryController(ICategoryService service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
