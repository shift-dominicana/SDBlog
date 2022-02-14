using System.Collections.Generic;
using System.Linq;

namespace SDBlog.DataModel.Classes
{
    public class PageCollection<T>
    {
        public bool HasItems
        {
            get
            {
                return Items != null && Items.Any();
            }
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
    }
}
