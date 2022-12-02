using Domain.Exceptions;
using Squads.Shared.Payments;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService service, ILogger<PaymentController> logger)
    {
        _paymentService = service;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<PaymentResponse.GetIndex> GetAll([FromQuery] PaymentRequest.GetIndex request)
    {
        return _paymentService.GetList(request);
    }

    [HttpGet("Id")]
    public ActionResult<PaymentResponse.GetDetail> Get([FromRoute] PaymentRequest.GetDetail request)
    {
        return _paymentService.Get(request);
    }

    [HttpPut("{paymentId}/markAsPaid")]
    public async Task<ActionResult<PaymentResponse.GetDetail>> MarkAsPaid([FromRoute] int paymentId)
    {
        return await _paymentService.MarkAsPaid(paymentId);
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete([FromRoute] PaymentRequest.Delete request)
    {
        _paymentService.Delete(request);
        return NoContent();
    }

    [HttpGet("GetWithUser")]
    public async Task<ActionResult<PaymentResponse.GetWithUser>> GetPayments()
    {
        return await _paymentService.GetPayments();
    }
}