using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRequestRepository _requestRepository;

        public CustomerController(ICustomerRepository customerRepository, IRequestRepository requestRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetOne(int id)
        {
            var result = await _customerRepository.GetCustomerById(id);

            result.Requests = _requestRepository.GetAllRequests().Where(res => res.CustomerId == id).ToList();

            return (result == null) ? NotFound() : Ok(result);
        }


    }
}
