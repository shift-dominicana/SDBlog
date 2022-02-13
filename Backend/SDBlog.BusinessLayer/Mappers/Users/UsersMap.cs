using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Users;
using SDBlog.DataModel.Entities.Users;

namespace SDBlog.BusinessLayer.Mappers.Users
{
    public class UsersMap : Profile
    {
        public UsersMap()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
