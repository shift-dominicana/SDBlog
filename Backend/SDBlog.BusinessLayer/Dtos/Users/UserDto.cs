using SDBlog.Core.Base;
using System;

namespace SDBlog.BusinessLayer.Dtos.Users
{
    public class UserDto : DtoBase
    {
        public String Email { get; set; }

        public String Password { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String NickName { get; set; }

        public DateTime Dob { get; set; } //Date of Birth

        public String Telephone { get; set; }

        public String CellPhone { get; set; }

        public String PersonalId { get; set; }

        public bool Confirmed { get; set; }
    }
}
