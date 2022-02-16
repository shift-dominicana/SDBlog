using SDBlog.Core.Base;
using System;
using System.Collections.Generic;

namespace SDBlog.DataModel.Entities
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
