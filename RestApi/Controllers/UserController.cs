using System;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using static BCrypt.Net.BCrypt;

namespace RestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register(NewUserDTO userDto)
        {
            if (ModelState.IsValid)
            {
                var hashPassword = HashPassword(userDto.Password, 12);

                if (Verify(userDto.Password, hashPassword))
                {
                    var userToRegister = new User
                    {
                        UserName = userDto.UserName,
                        EmailAddress = userDto.EmailAddress,
                        Role = userDto.Role,
                        Password = hashPassword
                    };

                    await _userRepository.RegisterUser(userToRegister);

                    return CreatedAtAction(nameof(GetById), new {id = userToRegister.Id}, userToRegister);
                }

            }

            return BadRequest(userDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var result = await _userRepository.GetUserById(id);

            return (result == null) ? NotFound() : Ok(result);
        }
    }
}