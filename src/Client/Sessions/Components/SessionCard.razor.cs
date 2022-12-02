
using Domain.Sessions;
using Microsoft.AspNetCore.Components;

namespace Squads.Client.Sessions.Components
{
    public partial class SessionCard
    {
        public bool isOpen { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Description { get; set; }
        [Parameter]
        public DateTime StartsAt { get; set; }
        [Parameter]
        public DateTime EndsAt { get; set; }
    }
}