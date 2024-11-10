using MediatR;
using Microsoft.AspNetCore.Mvc;
using server.Application.Command_Operations.Vouchers;
using server.Application.Query_Operations.Vouchers;
using static server.Core.ResponseModels;
namespace server.API.Voucher
{
    public class VoucherController : Controller
    {
        private readonly ISender _sender;
        public VoucherController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet("get-voucher")]
        public async Task<IActionResult> GetVoucher([FromQuery] GetVoucher_Command request, CancellationToken ct = default)
        {
            GetVoucher_Result result = await _sender.Send(request,ct);

            if (!result.IsRetrieved) return BadRequest( new { message = result.ErrorMessage});

            return Ok(result.Voucher);
        }

        [HttpPost("create-voucher")]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucher_Command request, CancellationToken ct = default)
        {
            CreateVoucher_Result result = await _sender.Send(request, ct);

            if (!result.IsCreated) return BadRequest(new { message = result.Message});

            return Ok(result.Message);
        }

    }
}
