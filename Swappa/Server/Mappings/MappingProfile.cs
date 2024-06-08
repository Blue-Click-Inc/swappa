using AutoMapper;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;

namespace SpringBoardApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppRole, RoleDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<RoleForUpdateDto, AppRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName));
            CreateMap<AppUser, UserDetailsDto>();
            CreateMap<UserDetailsForUpdateDto, AppUser>();
            CreateMap<FeedbackForAddDto, UserFeedback>();
        }
    }
}