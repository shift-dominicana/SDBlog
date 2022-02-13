using AutoMapper;
using FluentValidation;
using SDBlog.BusinessLayer.Interfaces.Users;
using SDBlog.BusinessLayer.Services.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities.Users;

namespace SDBlog.BusinessLayer.Services.Users
{
    public class UserService : BaseRepository<User>, IUserService
    {
        public UserService(MainDbContext context, IValidator<User> validator, IMapper mapper)
            : base(context, validator, mapper)
        {

        }
    }
}
