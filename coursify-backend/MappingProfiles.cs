using AutoMapper;
using coursify_backend.DTO.GET;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Models;

namespace coursify_backend
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Source -> Target

            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.Password, dest => dest.Ignore());

            CreateMap<User, UserInfo>();

            CreateMap<Category, CategoryDTO>();

            CreateMap<Course, CourseAdminTableDTO>();

        }
    }
}
