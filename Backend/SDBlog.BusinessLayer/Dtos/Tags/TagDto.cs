using SDBlog.Core.Base;

namespace SDBlog.BusinessLayer.Dtos.Tags
{
    public class TagDto : DtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
