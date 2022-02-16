using AutoMapper;
using FluentValidation;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Repositories.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Services
{
    public class PostService : BaseRepository<Post>, IPostService
    {
        public PostService(MainDbContext context
            , IValidator<Post> validator
            , IMapper mapper)
            : base(context, validator, mapper)
        {
        }
    }
}
