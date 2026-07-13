namespace CarServiceApi.Wrappers
{
    public class PagedResponse<T>
    {
        private List<object> users;
        private int count;

        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }


        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        public PagedResponse(List<object> users, int count, int pageNumber, int pageSize)
        {
            this.users = users;
            this.count = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
