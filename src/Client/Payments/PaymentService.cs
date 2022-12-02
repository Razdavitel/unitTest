using Squads.Shared.Payments;
using Microsoft.AspNetCore.Components;
using Ardalis.GuardClauses;
using System.Net.Http.Json;

namespace Squads.Client.Payments;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _client; 
    private const string _ENDPOINT = "api/payment";

    public PaymentService(HttpClient client)
    {
        _client = client;
    }

    public PaymentResponse.GetDetail Create(PaymentRequest.Create request)
    {
        throw new NotImplementedException();
    }

    public void Delete(PaymentRequest.Delete request)
    {
        throw new NotImplementedException();
    }

    public PaymentResponse.GetDetail Edit(PaymentRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public PaymentResponse.GetDetail Get(PaymentRequest.GetDetail request)
    {
        throw new NotImplementedException();
    }

    public PaymentResponse.GetIndex GetList(PaymentRequest.GetIndex request)
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentResponse.GetWithUser> GetPayments()
    {
        PaymentResponse.GetWithUser? res = await _client.GetFromJsonAsync<PaymentResponse.GetWithUser>($"{_ENDPOINT}/GetWithUser");
        Guard.Against.Null(res);
        return res;
    }

    public async Task<PaymentResponse.GetDetail> MarkAsPaid(int paymentId)
    {
        PaymentRequest.Edit req = new();
        req.Id = paymentId;
        var res = await _client.PutAsJsonAsync<PaymentRequest.Edit>($"{_ENDPOINT}/{paymentId}/MarkAsPaid", req);
        Guard.Against.Null(res);
        PaymentResponse.GetDetail? payment = await res.Content.ReadFromJsonAsync<PaymentResponse.GetDetail>();
        Guard.Against.Null(payment);
        return payment;
    }
}