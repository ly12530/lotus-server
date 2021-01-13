using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Domain;
using RestApi.Models;

namespace RestApi.Mappers
{
    public class RequestMapper
    {
        public static MapRequestDTO MapToRequestDTO(Request request)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Customer, MapRequestDTO.MapRequestCustomer>();
                mc.CreateMap<User, MapRequestDTO.MapRequestUser>();
                mc.CreateMap<Request, MapRequestDTO>()
                    .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                    .ForMember(dest => dest.Subscribers, opt => opt.MapFrom(src => src.Subscribers))
                    .ForMember(dest => dest.DesignatedUser, opt => opt.MapFrom(src => src.DesignatedUser));
            });
            var mapper = new Mapper(conf);
            return mapper.Map<Request, MapRequestDTO>(request);
        }

        public static IList<MapRequestDTO> MapToRequestDTOList(List<Request> requests)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Customer, MapRequestDTO.MapRequestCustomer>();
                mc.CreateMap<User, MapRequestDTO.MapRequestUser>();
                mc.CreateMap<Request, MapRequestDTO>()
                    .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                    .ForMember(dest => dest.Subscribers, opt => opt.MapFrom(src => src.Subscribers))
                    .ForMember(dest => dest.DesignatedUser, opt => opt.MapFrom(src => src.DesignatedUser));
            });
            var mapper = new Mapper(conf);
            return mapper.Map<List<Request>, List<MapRequestDTO>>(requests.ToList());
        }
        
        public static IList<MapSubscribersDTO> MapToSubscriberDTOList(List<User> subs)
        {
            var conf = new MapperConfiguration(mc => mc.CreateMap<User, MapSubscribersDTO>());
            var mapper = new Mapper(conf);
            
            return mapper.Map<List<User>, List<MapSubscribersDTO>>(subs);
        }
    }
}