using SDBlog.Core.Base;
using SDBlog.DataModel.Entities.Categories;
using SDBlog.DataModel.Entities.PostTags;
using SDBlog.DataModel.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.DataModel.Entities.Posts
{
    public class Post : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset PublicDate { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Link { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }

        public ICollection<PostTag> PostTags { set; get; }
    }
}
