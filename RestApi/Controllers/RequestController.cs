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
        ///     Get a list of all requests
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Request>> GetAll()
        {
            var result = _requestRepository.GetAllRequests();

            return Ok(result);
        }

        /// <summary>
        ///     Get a specific request by its id
        /// </summary>
        /// <param name="id">Id of the request</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetOne(int id)
        {
            var result = await _requestRepository.GetRequestById(id);

            return (result == null) ? NotFound() : Ok(result);
        }

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
    }
}