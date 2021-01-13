using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [Authorize(Policy = "BettingCoordinatorOnly")]
    [Authorize(Policy = "CustomerOnly")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRequestRepository _requestRepository;

        public CustomerController(ICustomerRepository customerRepository, IRequestRepository requestRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        }

        /// <summary>
        /// Get a list of all Customers
        /// </summary>
        /// <returns>List of all Customers</returns>
        /// <response code="200"/>
        /// <response code="403"/>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<Customer>> GetAll()
        {
            var result = _customerRepository.GetAllCustomers();
            
            return Ok(result);

        }

        /// <summary>
        /// Get Customer by Id
        /// </summary>
        /// <param name="id">Id of the Customer</param>
        /// <returns>Customer with the given Id</returns>
        /// <response code="200"/>
        /// <response code="404"/>
        /// <response code="403"/>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Customer>> GetOne(int id)
        {
            if (id == 0) return NotFound();
            var result = await _customerRepository.GetCustomerById(id);

            result.Requests = _requestRepository.GetAllRequests().Where(res => res.CustomerId == id).ToList();

            return (result == null) ? NotFound() : Ok(result);
        }
        
    }
}
