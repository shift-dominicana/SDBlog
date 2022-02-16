using AutoMapper;
using SDBlog.Api.Controllers.Base;
using SDBlog.BusinessLayer.Dtos.Users;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.DataModel.Entities;

namespace SDBlog.Api.Controllers
{
    public class UserController : BaseController<User, UserDto>
    {
        public UserController(IUserService service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
