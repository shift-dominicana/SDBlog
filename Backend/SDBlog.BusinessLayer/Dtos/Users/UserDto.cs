using SDBlog.Core.Base;
using System;

namespace SDBlog.BusinessLayer.Dtos.Users
{
    public class UserDto : DtoBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsAdmin { get; set; }
    }
}
