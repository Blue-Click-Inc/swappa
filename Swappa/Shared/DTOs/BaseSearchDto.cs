namespace Swappa.Shared.DTOs
{
    public abstract class BaseSearchDto : BasePageDto
    {
        public string SearchBy { get; set; } = string.Empty;
    }
}