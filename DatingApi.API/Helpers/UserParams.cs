namespace DatingApi.API.Helpers
{
    public class UserParams
    {
        private int maxPageSize = 50;
        public int PageNumber { get; set; }
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > maxPageSize) ? maxPageSize : value;}
        }
        public string Gender { get; set; }
        public int UserId { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }
        public int MyProperty { get; set; }
        public bool Likees { get; set; } = false;
        public bool Likers { get; set; } = false;
    }
}