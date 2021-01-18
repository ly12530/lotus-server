using AutoMapper;
using Core.Domain;
using RestApi.Models;

namespace RestApi.Mappers
{
    public class CustomerMapper
    {
        public static MapCustomerAuthDTO ToCustomerAuthDTO(Customer customer)
        {
            var conf = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Customer, MapCustomerAuthDTO>();
            });

            var mapper = new Mapper(conf);
            return mapper.Map<Customer, MapCustomerAuthDTO>(customer);
        }
    }
}