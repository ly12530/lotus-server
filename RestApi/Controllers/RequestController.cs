using System;
using System.Collections.Generic;
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

        public RequestController(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        }

        /// <summary>
        ///     Get a list of all Requests
        /// </summary>
        /// <returns>List of all Requests (open & closed)</returns>
        [HttpGet]
        public ActionResult<List<Request>> GetAll()
        {
            var result = _requestRepository.GetAllRequests();

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

            return (result == null) ? NotFound() : Ok(result);
        }

        /// <summary>
        ///     Get a list of all Requests where IsOpen = true
        /// </summary>
        /// <returns>List of all Requests (open)</returns>
        [HttpGet("isopen")]
        public ActionResult<List<Request>> GetAllOpenRequests()
        {
            var result = _requestRepository.GetOpenRequests();

            return Ok(result);
        }

        /// <summary>
        ///     Create a new request
        /// </summary>
        /// <param name="requestDto">Body with attributes of the Request</param>
        /// <returns>Request which was created</returns>
        [HttpPost]
        public async Task<ActionResult<RequestDTO>> AddNewRequest(NewRequestDTO requestDto)
        {
            if (ModelState.IsValid)
            {
                var requestToCreate = new Request
                {
                    CustomerId = requestDto.CustomerId,
                    Location = requestDto.Location,
                    StartDate = requestDto.StartDate,
                    EndDate = requestDto.EndDate,
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
        public async Task<ActionResult<RequestDTO>> UpdateIsOpen(int id, [FromBody]PutIsOpenRequestDTO requestToChange)
        {
            var request = await _requestRepository.GetRequestById(id);

            // Sanity Check
            if ((request == null) || (id != requestToChange.Id))
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

            await _requestRepository.UpdateIsOpen(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
        }
        
        /// <summary>
        ///     Update the Start- and EndDate attributes of the Request
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="requestToChange">Body with the attributes to change of the Request</param>
        /// <returns>Request with updated values</returns>
        [HttpPut("{id}/dates")]
        public async Task<ActionResult<RequestDTO>> UpdateDate(int id, [FromBody]PutDateRequestDTO requestToChange)
        {
            var request = await _requestRepository.GetRequestById(id);

            // Sanity Check
            if ((request == null) || (id != requestToChange.Id))
            {
                return BadRequest();
            }
            
            request.StartDate = requestToChange.StartDate;
            request.EndDate = requestToChange.EndDate;

            await _requestRepository.UpdateDate(request);

            var resultToReturn = request;

            return Ok(resultToReturn);
        }
    }
}