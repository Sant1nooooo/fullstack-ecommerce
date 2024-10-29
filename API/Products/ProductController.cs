using MediatR;
using Microsoft.AspNetCore.Mvc;
using server.Application.Command_Operations.Products;
using server.Application.Models;
using server.Application.Query_Operations.Products;
using static server.Core.ResponseModels;

namespace server.API.Products
{
    [Route("api[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ISender _sender;
        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("get-product-list")]
        public async Task<IActionResult> GetProductList([FromQuery] GetProductList_Query request, CancellationToken ct = default) 
        {
            IEnumerable<ProductSubImages>? productList = await _sender.Send(request, ct);

            if (!productList!.Any()) return BadRequest( new { message = "WARNING: No existing product!"});

            return Ok(productList);
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct([FromQuery] GetProduct_Query request, CancellationToken ct = default)
        {
            GetProduct_Result result = await _sender.Send(request, ct);

            if (result.IsNotExisting) return BadRequest(new { message = result.Message });

            return Ok(result.Product);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromQuery] CreateProduct_Command request, CancellationToken ct = default)
        {
            var result = await _sender.Send(request, ct);
            if (result.IsExisting)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Message);
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProduct_Command request, CancellationToken ct = default)
        {
            //This action will also delete the similar product in the `CartProduct` table or products na hindi na nabibili.
            DeleteProduct_Result result = await _sender.Send(request, ct);
            if (!result.IsSuccessful)
            {
                return BadRequest(new { message = result.Message});
            }
            return Ok(result.Message);
        }

        [HttpPatch("update-visibility")]
        public async Task<IActionResult> ProductVisibility([FromQuery] UpdateProductVisibility_Command request, CancellationToken ct = default)
        {
            //This action will also hide or unhide the similar product in the `CartProduct` table or products na hindi na nabibili.
            UpdateProductVisibility_Result? result = await _sender.Send(request, ct);
            if (!result.IsSuccessful) return BadRequest(new { message = result.Message });
            return Ok(result.Message);
            
        }

        [HttpPatch("discount-product")]
        public async Task<IActionResult> DiscountProduct([FromQuery] DiscountProduct_Command request, CancellationToken ct = default)
        {
            DiscountedProduct_Result result = await _sender.Send(request, ct);

            if (!result!.IsSuccessful) return BadRequest(new { message = result.Message});

            return Ok(result.Message);
        }
    }   
}
