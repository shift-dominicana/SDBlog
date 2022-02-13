using SDBlog.Core.Base;
using SDBlog.DataModel.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.DataModel.Entities.Categories
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
