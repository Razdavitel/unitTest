using MudBlazor;

namespace Squads.Client.Sessions.Components
{
    public partial class TimePicker
    {
        MudTimePicker _picker;
        TimeSpan? time = new TimeSpan(00, 45, 00);
        private bool autoClose;
    }
}