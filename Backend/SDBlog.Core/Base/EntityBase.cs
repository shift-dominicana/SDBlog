using System;

namespace SDBlog.Core.Base
{
    public class EntityBase
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
}
