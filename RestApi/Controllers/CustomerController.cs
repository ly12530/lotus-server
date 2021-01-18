using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using RestApi.Mappers;
using RestApi.Models;
using static BCrypt.Net.BCrypt;

namespace RestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IConfiguration _config;


        public CustomerController(ICustomerRepository customerRepository, IRequestRepository requestRepository, IConfiguration config)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _config = config;
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
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            if (id == 0) return NotFound();
            var result = await _customerRepository.GetCustomerById(id);

            result.Requests = _requestRepository.GetAllRequests().Where(res => res.CustomerId == id).ToList();

            return (result == null) ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Register a new Customer
        /// </summary>
        /// <param name="newCustomer">Body with attributes of Customer</param>
        /// <returns>Customer which was created</returns>
        /// <response code="201"/>
        /// <response code="400"/>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<MapCustomerAuthDTO>> Register(NewCustomerDTO newCustomer)
        {
            if (ModelState.IsValid)
            {
                var hashPassword = HashPassword(newCustomer.Password, 12);

                if (Verify(newCustomer.Password, hashPassword))
                {
                    var customerToRegister = new Customer
                    {
                        Name = newCustomer.Name,
                        EmailAddress = newCustomer.EmailAddress,
                        Role = Role.Customer,
                        Password = hashPassword
                    };

                    await _customerRepository.RegisterCustomer(customerToRegister);

                    var mappedCustomer = CustomerMapper.ToCustomerAuthDTO(customerToRegister);

                    mappedCustomer.Token =
                        GenerateJSONWebToken(mappedCustomer, customerToRegister.Role.GetDisplayName());

                    return CreatedAtAction(nameof(GetById), new {id = customerToRegister.Id}, mappedCustomer);
                }
            }

            return BadRequest(newCustomer);
        }

        /// <summary>
        /// Login to an existing Customer
        /// </summary>
        /// <param name="loginDto">Body with attributes to identity Customer</param>
        /// <returns>Customer with matching Email and Password + JWT-token</returns>
        /// <response code="200"/>
        /// <response code="401"/>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<ActionResult<MapCustomerAuthDTO>> Login(LoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                var foundCustomer = await _customerRepository.GetCustomerByEmail(loginDto.EmailAddress);

                if (foundCustomer == null) return Unauthorized();
                
                if (Verify(loginDto.Password, foundCustomer.Password))
                {
                    var mappedFoundCustomer = CustomerMapper.ToCustomerAuthDTO(foundCustomer);

                    mappedFoundCustomer.Token =
                        GenerateJSONWebToken(mappedFoundCustomer, foundCustomer.Role.GetDisplayName());

                    return Ok(mappedFoundCustomer);
                }
            }

            return Unauthorized();
        }

        /// <summary>
        /// Update the Customers Password
        /// </summary>
        /// <param name="updateCustomerPassword">Body with attributes to update the Customers Password</param>
        /// <returns>Message if Password was successfully updated</returns>
        /// <response code="200"/>
        /// <response code="400"/>
        /// <response code="401"/>
        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> UpdatePassword(UpdateCustomerPasswordDTO updateCustomerPassword)
        {
            if (ModelState.IsValid)
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                var jwtToken = authHeader.Split(" ")[1];

                var customerId = GetCustomerIdFromJSONWebToken(jwtToken);

                var customer = await _customerRepository.GetCustomerById(customerId);

                var newHashedPassword = ValidateAndReplacePassword(updateCustomerPassword.OldPassword, customer.Password,
                    updateCustomerPassword.NewPassword, 12);

                customer.Password = newHashedPassword;

                await _customerRepository.UpdatePassword(customer);

                return Ok("Password successfully updated");
            }

            return BadRequest("Password failed to update");
        }
        
        /// <summary>
        /// Generate new JSON webtoken
        /// </summary>
        /// <param name="userAuth">Mapped Users which is authenticated</param>
        /// <param name="role">Display name of the role</param>
        /// <returns>Generated JWT Token</returns>
        private string GenerateJSONWebToken(MapCustomerAuthDTO userAuth, string role)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("CustomerId", userAuth.Id.ToString()),
                new Claim("Role", role)
            };
    
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
                _config["Jwt:Issuer"],    
                claims,
                expires: DateTime.Now.AddMinutes(120),    
                signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }

        /// <summary>
        /// Get the CustomerId from the JWT-Token
        /// </summary>
        /// <param name="jwtToken">JWT-Token</param>
        /// <returns>CustomerId of the current authenticated Customer</returns>
        private int GetCustomerIdFromJSONWebToken(string jwtToken)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
            var customerClaim = token.Claims.First(c => c.Type == "CustomerId");

            return Int32.Parse(customerClaim.Value);
        }
    }
}
