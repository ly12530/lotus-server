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
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;

        public RequestController(IRequestRepository requestRepository, IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }




        /// <summary>
        ///     Get a list of all Requests
        /// </summary>
        /// <returns>List of all Requests (open & closed)</returns>
        /// https://exmaple.com/request
        [HttpGet]
        public ActionResult<List<Request>> GetAll([FromQuery] bool? isOpen)
        {
            var result = _requestRepository.GetAllRequests();

            if (isOpen != null) {
                switch (isOpen) {
                    case true:
                        result = result.Where(res => res.IsOpen);
                        break;
                    case false:
                        result = result.Where(res => !res.IsOpen);
                        break;
                }
            }

            return Ok(result);
        }

        /// <summary>
        ///     Get a specific Request by its id
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <returns>Request with the given id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetOne(int id)
        {
            var result = await _requestRepository.GetRequestById(id);

            return result == null ? (ActionResult<Request>) NotFound() : Ok(result);
        }


        /// <summary>
        ///     Create a new request
        /// </summary>
        /// <param name="requestDto">Body with attributes of the Request</param>
        /// <returns>Request which was created</returns>
        [HttpPost]
        public async Task<ActionResult<RequestDTO>> AddNewRequest(NewRequestDTO requestDto)
        {
            if (ModelState.IsValid) {
                var addressToCreate = new Address
                {
                    City = requestDto.Address.City,
                    Number = requestDto.Address.Number,
                    Postcode = requestDto.Address.Postcode,
                    Street = requestDto.Address.Street
                };

                var requestToCreate = new Request
                {
                    CustomerId = requestDto.CustomerId,
                    Location = requestDto.Location,
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

            return BadRequest();
        }

        /// <summary>
        ///     Update the IsOpen attribute of the Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="requestToChange">Body with the attributes to change of the Request</param>
        /// <returns>Request with updated values</returns>
        [HttpPut("{id}/isopen")]
        public async Task<ActionResult<RequestDTO>> UpdateIsOpen(int id, [FromBody] PutIsOpenRequestDTO requestToChange)
        {
            var request = await _requestRepository.GetRequestById(id);

            // Sanity Check
            if (request == null || id != requestToChange.Id) {
                return BadRequest();
            }

            if (requestToChange.IsOpen) {
                request.IsOpen = true;
            }

            if (!requestToChange.IsOpen) {
                request.IsOpen = false;
            }

            await _requestRepository.UpdateRequest(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
        }

        /// <summary>
        ///     Update the Start- and EndTime attributes of the Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="requestToChange">Body with the attributes to change of the Request</param>
        /// <returns>Request with updated values</returns>
        [HttpPut("{id}/dates")]
        public async Task<ActionResult<RequestDTO>> UpdateTime(int id, [FromBody] PutTimeRequestDTO requestToChange)
        {
            var request = await _requestRepository.GetRequestById(id);

            // Sanity Check
            if (request == null || id != requestToChange.Id) {
                return BadRequest();
            }

            request.StartTime = requestToChange.StartTime;
            request.EndTime = requestToChange.EndTime;

            await _requestRepository.UpdateRequest(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
        }


        /// <summary>
        ///  Subscribe function to let Users subscribe to a request
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subscribeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}/subscribe")]
        public async Task<ActionResult<SubscribeDTO>> Subscribe(int id, [FromBody] SubscribeDTO subscribeDTO)
        { 

            var request = await _requestRepository.GetRequestById(id);
            var user = await _userRepository.GetUserById(subscribeDTO.UserId);

            request.Subscribers.Add(user);
            user.Requests.Add(request);  

            await _requestRepository.UpdateRequest(request);
            await _userRepository.UpdateUser(user);
            
            return Ok();
        }
    }
}