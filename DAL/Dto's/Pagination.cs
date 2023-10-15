namespace DAL.Dto_s
{
    public class Pagination<T>
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
        public IEnumerable<T> data { get; set; }
    }
}
