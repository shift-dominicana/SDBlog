namespace SDBlog.Core.Base
{
    public class PaginatorBaseDto
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
    }
}
