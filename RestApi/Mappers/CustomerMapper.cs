using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Core.Domain;
using RestApi.Models;

namespace RestApi.Mappers
{
    public class CustomerMapper
    {
        public static MapCustomerAuthDTO MapToCustomerAuthDTO(Customer customer)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Customer, MapCustomerAuthDTO>();
            });

            var mapper = new Mapper(conf);
            return mapper.Map<Customer, MapCustomerAuthDTO>(customer);
        }

        public static IList<MapCustomerDTO> MapToCustomerDTOList(IList<Customer> customers)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Customer, MapCustomerDTO>()
                    .ForMember(c => c.Requests, opt => opt.Ignore());
            });

            var mapper = new Mapper(conf);
            return mapper.Map<IList<Customer>, IList<MapCustomerDTO>>(customers);
        }

        public static MapCustomerDTO MapToCustomerDTO(Customer customer)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Request, MapCustomerDTO.CustomerRequestDTO>();
                mc.CreateMap<Address, MapCustomerDTO.CustomerRequestAddressDTO>();
                mc.CreateMap<Customer, MapCustomerDTO>()
                    .ForMember(c => c.Requests, opt => opt.MapFrom(c => c.Requests));
            });

            var mapper = new Mapper(conf);
            return mapper.Map<Customer, MapCustomerDTO>(customer);
        }
    }
}