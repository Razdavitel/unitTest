namespace Squads.Shared.Payments;

public interface IPaymentService
{
    PaymentResponse.GetIndex GetList(PaymentRequest.GetIndex request);
    PaymentResponse.GetDetail Get(PaymentRequest.GetDetail request);
    void Delete (PaymentRequest.Delete request);
    PaymentResponse.GetDetail Edit(PaymentRequest.Edit request);
    Task<PaymentResponse.GetDetail> MarkAsPaid(int paymentId);
    PaymentResponse.GetDetail Create(PaymentRequest.Create request);
    Task<PaymentResponse.GetWithUser> GetPayments();
}
