namespace TestTaskPerson.Api.Controllers
{
    public class PagedRequest
    {
        public string OrderByField { get; set; }
        public bool Asc { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public static readonly PagedRequest Default = new PagedRequest { OrderByField = "Name", Asc = true, Page = 1, PageSize = 5 };
    }
}