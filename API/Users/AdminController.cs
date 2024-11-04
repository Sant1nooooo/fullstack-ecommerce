using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Command_Operations;
using server.Application.Models;
using server.Application.Query_Operations.Users;
using static server.Core.ResponseModels;

namespace server.API.Users
{
    [Route("api[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        public readonly ISender _sender;
        public AdminController(ISender sender, IValidator<CreateCustomer_Command> validator)
        {
            _sender = sender;
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("get-user-list")]
        public async Task<IActionResult> GetUserLists([FromQuery] GetUserLists_Query request, CancellationToken ct = default)
        {
            IEnumerable<User>? userLists = await _sender.Send(request,ct);
            if (userLists == null) return BadRequest(new { message = "WARNING: User table is empty!"});
            return Ok(userLists);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("get-customer")]
        public async Task<IActionResult> GetCustomer([FromQuery] GetCustomer_Query request, CancellationToken ct = default)
        {
            Customer? userLists = await _sender.Send(request, ct);
            if (userLists == null) return BadRequest(new { message = "WARNING: Invalid userID!" });
            return Ok(userLists);
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("get-admin")]
        public async Task<IActionResult> GetAdmin([FromQuery] GetAdmin_Query request, CancellationToken ct = default)
        {
            User? admin = await _sender.Send(request, ct);
            if (admin == null) return BadRequest(new {message = "WARNING: Invalid adminID"});
            return Ok(admin);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add-admin")]
        public async Task<IActionResult> CreateAdmin([FromQuery] CreateAdmin_Command request, CancellationToken ct = default)
        {
            await _sender.Send(request, ct);
            //Add try catch block in case the handler and repository throw error
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add-customer")]
        public async Task<IActionResult> CreateCustomer([FromQuery] CreateCustomer_Command request, CancellationToken ct = default)
        {
            CreateCustomer_Result result = await _sender.Send(request, ct);
            if (result.IsExisting)
            {
                Console.WriteLine("endpoint...");
                return BadRequest(new { message = result.Message }); //Error message
            }
            return Ok(result.Message); //Successful message
        }

        [HttpGet("login")]
        public async Task<IActionResult> LoginAccount([FromQuery] Login_Query request, CancellationToken ct = default)
        {
            LoginUser_Result result = await _sender.Send(request, ct);
            if (result.IsInvalid)
            {
                return BadRequest( new { message = result.ErrorMessage });
            }
            //return Ok(result.User);
            return Ok( new { token = result.Token });
        }

    }
}
