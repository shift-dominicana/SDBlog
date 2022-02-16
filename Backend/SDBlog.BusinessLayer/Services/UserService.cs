using AutoMapper;
using FluentValidation;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Repositories.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities;

namespace SDBlog.BusinessLayer.Services
{
    public class UserService : BaseRepository<User>, IUserService
    {
        public UserService(
            MainDbContext context
            , IValidator<User> validator
            , IMapper mapper)
            : base(context, validator, mapper)
        {
        }
    }
}
