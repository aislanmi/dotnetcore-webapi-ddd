using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
     [Route("api/v1/[controller]")]
     [ApiController]
     public class UsersController : ControllerBase
     {
          private readonly IUserService _userService;

          public UsersController(IUserService service)
          {
               _userService = service;
          }

          [Authorize("Bearer")]
          [HttpGet]
          public async Task<ActionResult> GetAllUsers()
          {
               if (!ModelState.IsValid) 
               {
                    return BadRequest(ModelState);
               }

               try
               {
                    return Ok(await _userService.GetAllUsers());
               }
               catch (ArgumentException argumentException)
               {
                    return StatusCode((int)HttpStatusCode.InternalServerError, argumentException.Message);
               }
          }

          [Authorize("Bearer")]
          [HttpGet]
          [Route("{id}", Name = "GetUserById")]
          public async Task<ActionResult> GetUserById(long id)
          {
               if (!ModelState.IsValid)
               {
                    return BadRequest(ModelState);
               }

               try
               {
                    return Ok(await _userService.GetUserById(id));
               }
               catch (ArgumentException argumentException)
               {
                    return StatusCode((int)HttpStatusCode.InternalServerError, argumentException.Message);
               }
          }

          [Authorize("Bearer")]
          [HttpPost("CreateUser")]
          public async Task<ActionResult> CreateUser([FromBody] UserDtoCreate user)
          {
               if (!ModelState.IsValid)
               {
                    return BadRequest(ModelState);
               }

               try
               {
                    var result = await _userService.CreateUser(user);

                    if (result != null)
                    {
                         return Created(new Uri(Url.Link("GetUserById", new { id = result.Id })), result);
                    }

                    return BadRequest();

               }
               catch (ArgumentException argumentException)
               {
                    return StatusCode((int)HttpStatusCode.InternalServerError, argumentException.Message);
               }
          }

          [Authorize("Bearer")]
          [HttpPut("UpdateUser")]
          public async Task<ActionResult> UpdateUser([FromBody] UserDtoUpdate user)
          {
               if (!ModelState.IsValid)
               {
                    return BadRequest(ModelState);
               }

               try
               {
                    var result = await _userService.UpdateUser(user);

                    if (result != null)
                    {
                         return Ok(result);
                    }

                    return BadRequest();

               }
               catch (ArgumentException argumentException)
               {
                    return StatusCode((int)HttpStatusCode.InternalServerError, argumentException.Message);
               }
          }

          [HttpDelete("DeleteUser/{id}")]
          public async Task<ActionResult> DeleteUser(long id)
          {
               if (!ModelState.IsValid)
               {
                    return BadRequest(ModelState);
               }

               try
               {
                    return Ok(await _userService.DeleteUser(id));

               }
               catch (ArgumentException argumentException)
               {
                    return StatusCode((int)HttpStatusCode.InternalServerError, argumentException.Message);
               }
          }

     }
}
