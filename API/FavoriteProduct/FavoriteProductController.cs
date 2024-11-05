using MediatR;
using Microsoft.AspNetCore.Mvc;
using server.Application.Command_Operations.FavoriteProduct;
using server.Application.Query_Operations.FavoriteProduct;
using static server.Core.ResponseModels;

namespace server.API.FavoriteProduct
{
    [Route("api[controller]")]
    [ApiController]
    public class FavoriteProductController : Controller
    {
        private readonly ISender _sender;
        public FavoriteProductController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("get-favorite-products")]
        public async Task<IActionResult> GetFavoriteProducts([FromQuery] GetFavoriteProducts_Query request, CancellationToken ct = default)
        {
            GetFavoriteProduct_Result result = await _sender.Send(request, ct);
            if (!result.IsSuccessful) return BadRequest(new { message = result.Message });
            return Ok(result.FavoriteProductList);
        }
        
        [HttpPost("mark-favorite-product")]
        public async Task<IActionResult> AddFavoriteProduct([FromQuery] MarkFavoriteProduct_Command request, CancellationToken ct = default)
        {
            FavoriteProduct_Result result = await _sender.Send(request, ct);
            if (!result.IsMarked) return BadRequest(new { message = result.Message});
            return Ok(result.Message);
        }

        [HttpDelete("delete-favorite-product")]
        public async Task<IActionResult> DeleteFavoriteProduct([FromQuery] DeleteFavoriteProduct_Command request, CancellationToken ct = default)
        {
            DeleteFavoriteProduct_Result result = await _sender.Send(request, ct);
            if (!result.IsDeleted) return BadRequest(new { message = result.Message });
            return Ok(result.Message);
        }
    }
}
