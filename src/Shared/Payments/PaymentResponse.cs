namespace Squads.Shared.Payments;

public static class PaymentResponse
{
    public class GetIndex
    {
        public List<PaymentDto.Index> Payments { get; set; } = new();
        public int TotalAmount { get; set; }
    }

    public class GetDetail 
    {
        public PaymentDto.Detail Payment { get; set; }
    }

    public class GetWithUser
    {
        public List<PaymentDto.WithUser> Payments { get; set; } = new();
        public int TotalAmount { get; set; }
    }
}
