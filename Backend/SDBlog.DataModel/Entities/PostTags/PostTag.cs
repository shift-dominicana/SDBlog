using SDBlog.Core.Base;
using SDBlog.DataModel.Entities.Posts;
using SDBlog.DataModel.Entities.Tags;

namespace SDBlog.DataModel.Entities.PostTags
{
    public class PostTag : EntityBase
    {
        public int PostId { get; set; }
        public int TagId { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
