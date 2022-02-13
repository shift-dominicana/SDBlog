using System.Collections.Generic;
using System.Linq;

namespace SDBlog.BusinessLayer.Dtos.Response
{
    public class DataCollectionDto<TDto>
    {
        public bool HasItems
        {
            get
            {
                return Items != null && Items.Any();
            }
        }

        public IEnumerable<TDto> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }

    }
}
