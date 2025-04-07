using AutoMapper;
using DATA.DTO;
using DATA.Models;

namespace DATA.Utility
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, UserRsponseDto>().ReverseMap();
            CreateMap<Report, ReportResponseDto>().ReverseMap();

        }
    }
}
