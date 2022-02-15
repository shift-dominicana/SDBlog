using SDBlog.Core.Base;
using System;

namespace SDBlog.BusinessLayer.Dtos.Posts
{
    public class PostDto : DtoBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset PublicDate { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Link { get; set; }

    }
}
