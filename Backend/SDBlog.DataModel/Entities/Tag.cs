using SDBlog.Core.Base;

namespace SDBlog.DataModel.Entities
{
    public class Tag : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
