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

        /// <summary>
        ///     Register a new User
        /// </summary>
        /// <param name="registerDto">Body with attributes of the User</param>
        /// <returns>User which had been registered + JWT-token</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (ModelState.IsValid) {
                var hashPassword = HashPassword(registerDto.Password, 12);

                if (Verify(registerDto.Password, hashPassword)) {
                    var userToRegister = new User
                    {
                        UserName = registerDto.UserName,
                        EmailAddress = registerDto.EmailAddress,
                        Role = registerDto.Role,
                        Password = hashPassword
                    };

                    await _userRepository.RegisterUser(userToRegister);

                    return CreatedAtAction(nameof(GetById), new {id = userToRegister.Id}, userToRegister);
                }
            }

            return BadRequest(registerDto);
        }

        /// <summary>
        ///     Login to an existing User
        /// </summary>
        /// <param name="loginDto">Body with attributes to identity User</param>
        /// <returns>User with matching email & password + JWT-token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDTO loginDto)
        {
            if (ModelState.IsValid) {
                var foundUser = await _userRepository.GetUserByEmail(loginDto.EmailAddress);

                if (Verify(loginDto.Password, foundUser.Password)) {
                    return Ok(foundUser);
                }
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Get a specific user by it's ID
        /// </summary>
        /// <param name="id">Id of the specific user</param>
        /// <returns>Specific user with the given Id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var result = await _userRepository.GetUserById(id);

            return result == null ? NotFound() : Ok(result);
        }
    }
}