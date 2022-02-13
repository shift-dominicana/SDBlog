using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SDBlog.Api.Controllers.Base;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Dtos.Users;
using SDBlog.BusinessLayer.Services.Users;
using SDBlog.DataModel.Entities.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using SDBlog.BusinessLayer.Interfaces.Users;

namespace Ecommerce.Api.Controllers.Users
{
    public class UserController : BaseController<User, UserDto>
    {

        public UserController(IUserService service, IMapper mapper) : base(service, mapper)
        {

        }


    }
}
