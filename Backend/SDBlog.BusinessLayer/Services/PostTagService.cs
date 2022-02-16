using AutoMapper;
using FluentValidation;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Repositories.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Services
{
    public class PostTagService : BaseRepository<PostTag>, IPostTagService
    {
        public PostTagService(MainDbContext context
            , IValidator<PostTag> validator
            , IMapper mapper)
            : base(context, validator, mapper)
        {
        }
    }
}
