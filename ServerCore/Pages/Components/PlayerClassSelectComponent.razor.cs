using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using ServerCore.DataModel;
using ServerCore.Helpers;
using ServerCore.ModelBases;
using System;

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
        public int EventId { get; set; }
        Event Event { get; set; }

        [Inject]
        PuzzleServerContext context { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Event = await context.Events.FindAsync(EventId);
            CurrentTeamMember = await UserEventHelper.GetTeamMemberForPlayer(context, Event, UserId);
            AvailablePlayerClasses = await TeamHelper.GetAvailableClasses(context, EventId, CurrentTeamMember.Team.ID);

            // TODO: Add admin override for all player classes (don't filter to available)
            await base.OnParametersSetAsync();
        }

        private async Task UpdatePlayerClassID()
        {
            CurrentTeamMember.Class = await UserEventHelper.GetPlayerClassFromID(context, SelectedPlayerClassID);
            await context.SaveChangesAsync();
            AvailablePlayerClasses = await TeamHelper.GetAvailableClasses(context, EventId, CurrentTeamMember.Team.ID);
        }
    }
}
