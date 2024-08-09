namespace Swappa.Shared.DTOs
{
    public class PageDto
    {
        const int minPageSize = 1;
        const int maxPageSize = 200;

        private int _pageNumber = 1;
        private int _pageSize = 20;

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = value < minPageSize ? minPageSize : value;
            }
        }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
    }
}
