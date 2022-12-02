using Squads.Shared.Common;

namespace Squads.Shared.Payments;

public static class PaymentRequest
{
    public class GetIndex : CommonSearchRequest
    {
        public int? userId { get; set; }
    }

    public class GetDetail : IdRequest {}

    public class Delete : IdRequest {}

    public class Create
    {
        public PaymentDto.Mutate Payment { get; set; }
    }

    public class Edit : IdRequest
    {
        public PaymentDto.Mutate Payment { get; set; }
    }
}
