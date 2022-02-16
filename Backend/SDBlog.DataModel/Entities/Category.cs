using SDBlog.Core.Base;
using System.Collections.Generic;

namespace SDBlog.DataModel.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
