namespace DatingApi.API.Helpers
{
    public class MessageParams
    {
        private int maxPageSize = 50;
        public int PageNumber { get; set; }
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > maxPageSize) ? maxPageSize : value;}
        }
        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}