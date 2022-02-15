using SDBlog.Core.Base;

namespace SDBlog.BusinessLayer.Dtos.PostTags
{
    public class PostTagDto : DtoBase
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
    }
}
