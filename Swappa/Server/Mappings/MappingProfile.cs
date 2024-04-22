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
        }
    }
}