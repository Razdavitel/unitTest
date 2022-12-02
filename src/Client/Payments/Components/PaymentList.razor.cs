using Squads.Shared.Payments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Squads.Client.Payments.Components;

public partial class PaymentList
{
    private MudTable<PaymentDto.WithUser>? _table;
    private List<PaymentDto.WithUser> _payments = new();
        private HashSet<PaymentDto.WithUser> _selection = new(); 

    [Inject] private IPaymentService _service { get; set; }
    [Inject] private IDialogService _dialog { get; set; }

    private DateTime? _startDate { get; set; }
    private DateTime? _untill { get; set; } = DateTime.Today.AddDays(1); //Add one to include today.
    private string? _userName { get; set; }
    private bool _unpaid { get; set; } = true;

    private AutoComplete _autoComplete = new();

    protected override async Task OnInitializedAsync()
    {
        await GetPayments();
    }

    private async Task GetPayments()
    {
        PaymentResponse.GetWithUser res = await _service.GetPayments();
        _payments = res.Payments;
        _autoComplete.setAutoCompleteNames(_payments.Select(p => $"{p.User.FirstName} {p.User.LastName}").ToList());
    }

    public class AutoComplete
    {
        private List<string> _names = new();

        public void setAutoCompleteNames(List<string> names)
        {
            _names = names;
        }

        public async Task<IEnumerable<string>> Search(string name) 
        {
            if(string.IsNullOrEmpty(name))
                return new string[0];

            return _names.Where(n => n.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }   

    public async Task ConfirmPayment()
    {
        DialogParameters parameters = new DialogParameters { ["Payments"] = _selection };
        var res = _dialog.Show<ConfirmPaymentDialog>("Bevestig betaling voor gebruikers:", parameters);
        var result = await res.Result;
        if(!result.Cancelled)
        {
            foreach (var payment in _selection)
            {
                payment.PayedAt = DateTime.Now;
                var del = await _service.MarkAsPaid(payment.Id);
                Console.WriteLine($"Deleting payment {del.Payment.Id}, payed at {del.Payment.PayedAt}");
            }
        }
    }

    private bool FilterSome(PaymentDto.WithUser payment)
    {
        bool filtered = false;
        DateTime start = _startDate == null ? DateTime.MinValue : (DateTime) _startDate;
        DateTime end = _untill == null ? DateTime.MaxValue : (DateTime) _untill;
        string userName = $"{payment.User.FirstName} {payment.User.LastName}";

        if(payment.CreatedOn >= start && payment.CreatedOn <= end)
            filtered = true;

        if((payment.PayedAt == null) == _unpaid)
        {
            filtered = filtered && true;
        }
        else
        {
            filtered = false;
        }

        if(_userName == null || userName.Contains(_userName, StringComparison.OrdinalIgnoreCase))        
        {
            filtered = filtered && true;
        }
        else
        {
            filtered = false;
        }

        return filtered;
    }
}