using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public RequestController(IRequestRepository requestRepository, IUserRepository userRepository,
            IHttpClientFactory httpClientFactory, ICustomerRepository customerRepository)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpClientFactory = httpClientFactory;
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        /// <summary>
        /// Get a list of all Requests
        /// </summary>
        /// <param name="isOpen">Show open/closed reqyests</param>
        /// <param name="date">Show requests matching a specific date</param>
        /// <param name="hasDesignatedUser">Show requests which have a designated user</param>
        /// <returns>List of all Requests (open and closed)</returns>
        [HttpGet]
        public ActionResult<List<Request>> GetAll([FromQuery] bool? isOpen, [FromQuery] DateTime? date, [FromQuery] bool? hasDesignatedUser)
        {
            var requests = _requestRepository.GetAllRequests();

            if (isOpen != null)
            {
                switch (isOpen)
                {
                    case true:
                        requests = requests.Where(res => res.IsOpen);
                        break;
                    case false:
                        requests = requests.Where(res => !res.IsOpen);
                        break;
                }
            }

            if (date != null)
            {
                requests = requests.Where(res => res.Date.Date == date.Value.Date);
            }

            if (hasDesignatedUser != null && hasDesignatedUser == true)
            {
                requests = requests.Where(res => res.DesignatedUser != null);
            }
            
            // Configure the AutoMapper
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
            var requestSource = mapper.Map<List<Request>, List<MapRequestDTO>>(requests.ToList());

            return Ok(requestSource);
        }

        /// <summary>
        /// Get a specific Request by its id
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <returns>Request with the given id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetOne(int id)
        {
            var result = await _requestRepository.GetRequestById(id);
            
            if (result == null) return NotFound();
            
            result.Customer = await _customerRepository.GetCustomerById(result.CustomerId);
            
            // Configure the AutoMapper
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
            var reqResource = mapper.Map<List<Request>, List<MapRequestDTO>>(new List<Request>(){result}).First();

            return Ok(reqResource);

        }

        /// <summary>
        /// Get all subscribers of a specific Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <returns>List of subscribers of the Request with the given Id</returns>
        [HttpGet("{id}/subscribers")]
        public async Task<ActionResult<Request>> GetRequestSubscribers(int id)
        {
            var request = await _requestRepository.GetRequestById(id);

            if (request == null) return NotFound();
            
            var subs = _requestRepository.GetAllRequests().First(r => r.Id == id).Subscribers.ToList();

            // Configure the AutoMapper
            var conf = new MapperConfiguration(mc => mc.CreateMap<User, MapSubscribersDTO>());
            var mapper = new Mapper(conf);
            var subResource = mapper.Map<List<User>, List<MapSubscribersDTO>>(subs);
            
            return Ok(subResource);
        }


        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="requestDto">Body with attributes of the Request</param>
        /// <returns>Request which was created</returns>
        [HttpPost]
        public async Task<ActionResult<RequestDTO>> AddNewRequest(NewRequestDTO requestDto)
        {
            if (ModelState.IsValid)
            {
                var addressToCreate = new Address
                {
                    City = requestDto.Address.City,
                    Number = requestDto.Address.Number,
                    Postcode = requestDto.Address.Postcode,
                    Street = requestDto.Address.Street
                };

                addressToCreate.Geometry = await GetGeometry(addressToCreate);

                var customer = await _customerRepository.GetCustomerById(requestDto.CustomerId);

                if (customer != null)
                {
                    var requestToCreate = new Request
                    {
                        Title = requestDto.Title,
                        Customer = customer,
                        Address = addressToCreate,
                        Date = requestDto.Date,
                        StartTime = requestDto.StartTime,
                        EndTime = requestDto.EndTime,
                        IsExam = requestDto.IsExam,
                        LessonType = requestDto.LessonType
                    };

                    await _requestRepository.AddRequest(requestToCreate);
                    return CreatedAtAction(nameof(GetOne), new {id = requestToCreate.Id}, requestToCreate);
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Update the IsOpen attribute of the Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="requestToChange">Body with the attributes to change of the Request</param>
        /// <returns>Request with updated values</returns>
        [HttpPut("{id}/isopen")]
        public async Task<ActionResult<RequestDTO>> UpdateIsOpen(int id, [FromBody] PutIsOpenRequestDTO requestToChange)
        {
            var request = await _requestRepository.GetRequestById(id);

            // Sanity Check
            if (request == null)
            {
                return BadRequest();
            }

            if (requestToChange.IsOpen)
            {
                request.IsOpen = true;
            }

            if (!requestToChange.IsOpen)
            {
                request.IsOpen = false;
            }

            await _requestRepository.UpdateRequest(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
        }

        /// <summary>
        /// Update the Start- and EndTime attributes of the Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="requestToChange">Body with the attributes to change of the Request</param>
        /// <returns>Request with updated values</returns>
        [HttpPut("{id}/dates")]
        public async Task<ActionResult<RequestDTO>> UpdateTime(int id, [FromBody] PutTimeRequestDTO requestToChange)
        {
            var request = await _requestRepository.GetRequestById(id);

            // Sanity Check
            if (request == null || id != requestToChange.Id)
            {
                return BadRequest();
            }

            request.StartTime = requestToChange.StartTime;
            request.EndTime = requestToChange.EndTime;

            await _requestRepository.UpdateRequest(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
        }

        /// <summary>
        /// Subscribe function to let Users subscribe to a request
        /// </summary>
        /// <param name="id">Id of the specific Request</param>
        /// <param name="subscribeDTO">Body with the attributes of the User which wants to subscribe on the Request</param>
        /// <returns>Message if subscription was successful</returns>
        [HttpPut("{id}/subscribe")]
        public async Task<ActionResult<SubscribeDTO>> Subscribe(int id, [FromBody] SubscribeDTO subscribeDTO)
        { 

            var request = await _requestRepository.GetRequestById(id);
            var user = await _userRepository.GetUserById(subscribeDTO.UserId);

            // Sanity Check
            if (request == null)
            {
                return BadRequest();
            }

            request.Subscribe(user);
            user.Subscribe(request);

            if (!request.Subscribe(user))
            {
                return Unauthorized("Request is not open!");
            }
            await _requestRepository.UpdateRequest(request);
            await _userRepository.UpdateUser(user);

            return Ok("Succesfully subscribed!");
        }
        
        /// <summary>
        /// Update the real time and distance of a Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="putRealTimeDistanceRequestDTO">Body with attributes which contains real starttime and distance</param>
        /// <returns>Request which had been updated</returns>
        [HttpPut("{id}/timeanddistance")]
        public async Task<ActionResult<PutRealTimeDistanceRequestDTO>> UpdateTimeAndDistance(int id, [FromBody]PutRealTimeDistanceRequestDTO putRealTimeDistanceRequestDTO)
        {
            var request = await _requestRepository.GetRequestById(id);
            request.RealStartTime = putRealTimeDistanceRequestDTO.RealStartTime;
            request.RealEndTime = putRealTimeDistanceRequestDTO.RealEndTime;
            request.DistanceTraveled = putRealTimeDistanceRequestDTO.DistanceTraveled;

            await _requestRepository.UpdateRequest(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
          
        }
        /// <summary>
        /// Assign a specific member (User) to a Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="subscribeDTO">Body which contains with SubscriberId</param>
        /// <returns>Request which had been updated</returns>
        [HttpPut("{id}/assign")]
        public async Task<ActionResult<SubscribeDTO>> AssignUser(int id, [FromBody]SubscribeDTO subscribeDTO)
        {
            var request = await _requestRepository.GetRequestById(id);
            var user = await _userRepository.GetUserById(subscribeDTO.UserId);

            if (request != null && user != null)
            {
                request.DesignatedUser = user;
                user.Jobs.Add(request);

                await _requestRepository.UpdateRequest(request);
                await _userRepository.UpdateUser(user);

                var resultToReturn = request;

                return Ok(resultToReturn);
            }

            if (request == null && user == null) return Problem("User and Request do not exist", statusCode: (int)HttpStatusCode.BadRequest);
            else if (user == null) return Problem("User does not exist", statusCode: (int)HttpStatusCode.BadRequest);
            else if (request == null) return Problem("Request does not exist", statusCode: (int)HttpStatusCode.BadRequest);
            
            return BadRequest();
        }
        
        /// <summary>
        /// Determine the Lat and Lon of a given Address
        /// </summary>
        /// <param name="address">Body which contains the Address details</param>
        /// <returns>Array with Lat and Lon values</returns>
        private async Task<double[]> GetGeometry(Address address)
        {
            IList<double> result = new List<double>();

            var addressQuery = $"{address.Street} {address.Number} {address.City}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://photon.komoot.io/api/?q={addressQuery}");

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<JObject>(jsonString);
                var coords = json["features"][0]["geometry"]["coordinates"].ToArray();

                foreach (var coord in coords)
                {
                    result.Add(coord.ToObject<double>());
                }
            }

            return result.ToArray();
        }
    }
}