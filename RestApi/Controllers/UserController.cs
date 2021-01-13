using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using Microsoft.VisualBasic;
using RestApi.Mappers;
using RestApi.Models;
using static BCrypt.Net.BCrypt;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace RestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _config = config;
        }
        
        /// <summary>
        /// Get all Users either with or without a role.
        /// </summary>
        /// <param name="role">Role of the User</param>
        /// <returns>Users matching the query</returns>
        /// <response code="200"/>
        /// <response code="403"/>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = "AdminAndBettingMasterOnly")]
        public ActionResult<List<User>> GetAllUsers([FromQuery] Role? role)
        {
            var users = _userRepository.GetAllUsers();
            
            if (role != null)
            {
                switch (role)
                {
                    case Role.Customer:
                        users = users.Where(res => res.Role == Role.Customer);
                        break;
                    case Role.Member:
                        users = users.Where(res => res.Role == Role.Member);
                        break;
                    case Role.PenningMaster:
                        users = users.Where(res => res.Role == Role.PenningMaster);
                        break;
                    case Role.BettingCoordinator:
                        users = users.Where(res => res.Role == Role.BettingCoordinator);
                        break;
                    case Role.Instructor:
                        users = users.Where(res => res.Role == Role.Instructor);
                        break;
                    case Role.Administrator:
                        users = users.Where(res => res.Role == Role.Administrator);
                        break;
                }
            }

            var mappedUsers = UserMapper.ToUserDTOList(users.ToList());
            
            return Ok(mappedUsers);
        }

        /// <summary>
        /// Register a new User
        /// </summary>
        /// <param name="registerDto">Body with attributes of the User</param>
        /// <returns>User which had been registered + JWT-token</returns>
        /// <response code="201"/>
        /// <response code="400"/>
        /// <response code="403"/>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<MapUserAuthDTO>> Register(RegisterDTO registerDto)
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
                    
                    var mappedUser = UserMapper.ToUserAuthDTO(userToRegister);

                    mappedUser.Token = GenerateJSONWebToken(mappedUser, userToRegister.Role.GetDisplayName());

                    return CreatedAtAction(nameof(GetById), new {id = mappedUser.Id}, mappedUser);
                }
            }

            return BadRequest(registerDto);
        }

        /// <summary>
        /// Login to an existing User
        /// </summary>
        /// <param name="loginDto">Body with attributes to identity User</param>
        /// <returns>User with matching Email and Password + JWT-token</returns>
        /// <response code="200"/>
        /// <response code="401"/>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<ActionResult<MapUserAuthDTO>> Login(LoginDTO loginDto)
        {
            if (ModelState.IsValid) {
                var foundUser = await _userRepository.GetUserByEmail(loginDto.EmailAddress);

                if (foundUser == null) return Unauthorized();

                if (Verify(loginDto.Password, foundUser.Password))
                {
                    var mappedFoundUser = UserMapper.ToUserAuthDTO(foundUser);

                    mappedFoundUser.Token = GenerateJSONWebToken(mappedFoundUser, foundUser.Role.GetDisplayName());
                    
                    return Ok(mappedFoundUser);
                }
            }

            return Unauthorized();
        }

        /// <summary>
        /// Get a specific user by it's ID
        /// </summary>
        /// <param name="id">Id of the specific user</param>
        /// <returns>Specific user with the given Id</returns>
        /// <response code="200"/>
        /// <response code="404"/>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var result = await _userRepository.GetUserById(id);

            return result == null ? NotFound() : Ok(result);
        }
        
        /// <summary>
        /// Generate new JSON webtoken
        /// </summary>
        /// <param name="userAuth">Mapped Users which is authenticated</param>
        /// <param name="role">Display name of the role</param>
        /// <returns></returns>
        private string GenerateJSONWebToken(MapUserAuthDTO userAuth, string role)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", userAuth.Id.ToString()),
                new Claim("Role", role)
            };
    
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
                _config["Jwt:Issuer"],    
                claims,
                expires: DateTime.Now.AddMinutes(120),    
                signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }
    }
}
