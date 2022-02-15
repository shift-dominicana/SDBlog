using AutoMapper;
using FluentValidation;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Repositories.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities.Tags;

namespace SDBlog.BusinessLayer.Services
{
    public class TagService : BaseRepository<Tag>, ITagService
    {
        public TagService(MainDbContext context
            , IValidator<Tag> validator
            , IMapper mapper)
            : base(context, validator, mapper)
        {
        }
    }
}
