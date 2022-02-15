using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Users;
using SDBlog.DataModel.Entities.Users;

namespace SDBlog.BusinessLayer.Mappers
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
