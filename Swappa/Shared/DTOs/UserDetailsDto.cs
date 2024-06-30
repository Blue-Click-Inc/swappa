namespace Swappa.Shared.DTOs
{
    public class UserDetailsDto : UserBaseDto
    {
        public string Roles { get; set; } = string.Empty;
        public LocationToReturnDto? Location { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
