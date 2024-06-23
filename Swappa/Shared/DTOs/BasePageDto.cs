using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Shared.DTOs
{
    public abstract class BasePageDto : IPageDto
    {
        const int minPageSize = 1;
        const int maxPageSize = 10000;

        private int _pageNumber = 1;
        private int _pageSize = 1000;

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
