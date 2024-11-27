using Microsoft.AspNetCore.Mvc;
using MediatR;
using server.Application.Command_Operations.CartProduct;
using static server.Core.ResponseModels;
using server.Application.Query_Operations.CartProduct;
using server.Application.Command_Operations.CheckoutProducts;
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

        [HttpGet("get-cart-product-list")]
        public async Task<IActionResult> GetCartProductList([FromQuery] GetCartProducts_Query request, CancellationToken ct = default)
        {
            GetCartProductList_Result result = await _sender.Send(request,ct);
            if (!result.IsSuccessful) return BadRequest(new { message = result.Message });

            return Ok(result.CartProductList);
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
        public async Task<IActionResult> IncreaseCartProuct([FromQuery] IncreaseDecreaseCartProduct_Command requst, CancellationToken ct = default)
        {
            IncreaseDecreaseQuantity_Result result = await _sender.Send(requst, ct);
            if (!result.IsSuccessful) return BadRequest(new { message = result.Message });
            return Ok(result.Message);
        }


        [HttpGet("checkout-cart-product")]
        //Change to `HttpPatch` since it will change the `IsPaid` of the selected CartProducts to true.
        public async Task<IActionResult> CheckoutCartProduct([FromQuery] CheckoutCartProducts_Command request, CancellationToken ct = default)
        {
            CheckoutCartProductsResult result = await _sender.Send(request, ct);
            if (!result.IsCheckedOut)
            {
                return BadRequest(new { message = result.Message});
            }
            return Ok(new { products = result.ProductList});
        }
    }
}
