using SDBlog.Core.Base;
using SDBlog.DataModel.Entities.Posts;
using System;
using System.Collections.Generic;

namespace SDBlog.DataModel.Entities.Users
{
    public class User : EntityBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
