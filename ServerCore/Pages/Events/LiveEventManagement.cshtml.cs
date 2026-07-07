using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using ServerCore.DataModel;
using ServerCore.ModelBases;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace ServerCore.Pages.Events
{
    [Authorize(Policy = "IsEventAdmin")]
    public class LiveEventManagementModel : EventSpecificPageModel
    {
        [Inject]
        PuzzleServerContext PuzzleServerContext { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        public LiveEventManagementModel(PuzzleServerContext serverContext, UserManager<IdentityUser> manager) : base(serverContext, manager)
        {
        }

        public void OnGet()
        {
        }
    }
}
