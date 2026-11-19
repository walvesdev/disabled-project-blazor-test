using AutoMapper;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
//using AutoMapper.Data.Configuration.Conventions;


namespace Sindvamb.Domain.Profiles.SindvambProfiles
{
    public class SindvambMapperProfiles : Profile
    {
        public SindvambMapperProfiles()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
