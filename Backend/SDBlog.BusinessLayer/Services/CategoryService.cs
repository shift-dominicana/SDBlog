using AutoMapper;
using FluentValidation;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Repositories.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Services
{
    public class CategoryService : BaseRepository<Category>, ICategoryService
    {
        public CategoryService(MainDbContext context
            , IValidator<Category> validator
            , IMapper mapper)
            : base(context, validator, mapper)
        {
        }
    }
}
