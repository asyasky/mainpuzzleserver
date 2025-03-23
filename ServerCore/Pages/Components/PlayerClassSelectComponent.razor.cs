using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using ServerCore.DataModel;
using ServerCore.Helpers;
using ServerCore.ModelBases;
using System;
using System.Diagnostics.Eventing.Reader;

namespace ServerCore.Pages.Components
{
    public partial class PlayerClassSelectComponent
    {
        public int SelectedPlayerClassID { get; set; }
        public List<PlayerClass> AvailablePlayerClasses { get; set; }

        [Parameter]
        public int UserId { get; set; }

        public TeamMembers CurrentTeamMember { get; set; }

        [Parameter]
        public EventRole CurrentUserEventRole { get; set; }

        [Parameter]
        public int EventId { get; set; }
        Event Event { get; set; }

        [Parameter]
        public bool IsTempClass { get; set; }

        [Inject]
        PuzzleServerContext context { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Event = await context.Events.FindAsync(EventId);
            CurrentTeamMember = await UserEventHelper.GetTeamMemberForPlayer(context, Event, UserId);

            if(IsTempClass)
            {
                AvailablePlayerClasses = await TeamHelper.GetAllPlayerClassesSorted(context, EventId);
            }
            else
            {
                // This will get unassigned classes for a player or all classes for an admin
                // Admin UI currently shows the player's current class twice, that's a known bug and can be safely ignored
                AvailablePlayerClasses = await TeamHelper.GetAvailablePlayerClassesSorted(context, EventId, CurrentUserEventRole, CurrentTeamMember.Team.ID);
            }

            await base.OnParametersSetAsync();
        }

        private async Task UpdatePlayerClassID()
        {
            // Update the database with the new PlayerClass, if it's "Unassigned" then set it to null
            await TeamHelper.SetPlayerClass(context, Event, CurrentUserEventRole, CurrentTeamMember, SelectedPlayerClassID, IsTempClass);

            if (IsTempClass)
            {
                AvailablePlayerClasses = await TeamHelper.GetAllPlayerClassesSorted(context, EventId);
            }
            else
            {
                // Get the new list of unassigned PlayerClasses
                AvailablePlayerClasses = await TeamHelper.GetAvailablePlayerClassesSorted(context, EventId, CurrentUserEventRole, CurrentTeamMember.Team.ID);
            }
        }
    }
}
