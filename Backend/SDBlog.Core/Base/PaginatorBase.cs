namespace SDBlog.Core.Base
{
    public class PaginatorBase
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
    }
}
