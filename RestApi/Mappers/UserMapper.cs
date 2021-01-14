using System.Collections.Generic;
using AutoMapper;
using Core.Domain;
using RestApi.Models;

namespace RestApi.Mappers
{
    public class UserMapper
    {
        public static MapUserAuthDTO ToUserAuthDTO(User user)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<User, MapUserAuthDTO>();
            });

            var mapper = new Mapper(conf);
            return mapper.Map<User, MapUserAuthDTO>(user);
        }

        public static IList<MapUserDTO> ToUserDTOList(List<User> users)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Request, MapUserDTO.MapUserJobsDTO>();
                mc.CreateMap<User, MapUserDTO>()
                    .ForMember(user => user.Jobs, opt => opt.MapFrom(user => user.Jobs));
            });

            var mapper = new Mapper(conf);
            return mapper.Map<IList<User>, IList<MapUserDTO>>(users);
        }
    }
}