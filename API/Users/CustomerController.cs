﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using server.Application.Command_Operations.CartProduct;
using static server.Core.ResponseModels;
namespace server.API.Customers
{
    [Route("api[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly ISender _sender;
        public CustomersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("add-product-cart")]
        public async Task<IActionResult> AddCartProduct([FromQuery] AddCartProduct_Command request, CancellationToken ct = default)
        {
            //Extract the `UserID` claims from the frontend.
            AddCartProduct_Result result = await _sender.Send(request, ct);
            if (result.IsFailure) return BadRequest(new { message = result.Message });
            return Ok(result.Message);
        }

        [HttpDelete("remove-cart-product")]
        public async Task<IActionResult> RemoveCartProduct([FromQuery] RemoveCartProduct_Command request, CancellationToken ct = default)
        {
            RemoveCartProduct_Result result = await _sender.Send(request, ct);
            if (!result.IsDeleted) return BadRequest(new { message = result.Message });
            return Ok(result.Message);

        }

        [HttpPatch("increase-cart-product")]
        public async Task<IActionResult> IncreaseCartProuct([FromQuery] IncreaseCartProduct_Command requst, CancellationToken ct = default)
        {
            IncreaseDecreaseQuantity_Result result = await _sender.Send(requst, ct);
            if (!result.IsSuccessful) return BadRequest(new { message = result.Message });
            return Ok(result.Message);
        }
    }
}