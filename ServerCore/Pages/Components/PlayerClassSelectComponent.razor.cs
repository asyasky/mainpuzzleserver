using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using ServerCore.DataModel;
using ServerCore.Helpers;
using ServerCore.ModelBases;
using Microsoft.EntityFrameworkCore;

namespace ServerCore.Pages.Components
{
    public partial class PlayerClassSelectComponent
    {
        [Inject]
        public IDbContextFactory<PuzzleServerContext> ServerContextFactory { get; set; }

        [Parameter]
        public int UserId { get; set; }

        [Parameter]
        public EventRole CurrentUserEventRole { get; set; }

        [Parameter]
        public int EventId { get; set; }
        Event Event { get; set; }

        [Parameter]
        public bool IsTempClass { get; set; }

        // The data needed to display on the page
        // The TeamMember leaves the scope after being updated (but cached values stick around) so something that can be explicitly updated needs to be used for display
        public int SelectedPlayerClassID { get; set; }
        public DisplayPlayerClass CurrentTeamMemberDisplayPlayerClass { get; set; }
        public TeamMembers CurrentTeamMember { get; set; }
        public List<PlayerClass> AvailablePlayerClasses { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            using PuzzleServerContext context = await ServerContextFactory.CreateDbContextAsync();

            Event = await context.Events.FindAsync(EventId);
            CurrentTeamMember = await UserEventHelper.GetTeamMemberForPlayer(context, Event, UserId);

            // If a player doesn't have a class in the DB, then set the bound value to the ID for "no class selected"
            if (CurrentTeamMember.Class == null)
            {
                SelectedPlayerClassID = 123456789;
            }

            if (IsTempClass && CurrentTeamMember.TemporaryClass != null)
            {
                SelectedPlayerClassID = CurrentTeamMember.TemporaryClass.ID;
                AvailablePlayerClasses = await TeamHelper.GetAllPlayerClassesSorted(context, EventId);

                CurrentTeamMemberDisplayPlayerClass = new DisplayPlayerClass()
                {
                    ClassId = CurrentTeamMember.TemporaryClass.ID,
                    Name = CurrentTeamMember.TemporaryClass.Name,
                };
            }
            else
            {
                SelectedPlayerClassID = CurrentTeamMember.Class.ID;

                // This will get unassigned classes for a player or all classes for an admin
                // Admin UI currently shows the player's current class twice, that's a known bug and can be safely ignored
                AvailablePlayerClasses = await TeamHelper.GetAvailablePlayerClassesSorted(context, EventId, CurrentUserEventRole, CurrentTeamMember.Team.ID);

                CurrentTeamMemberDisplayPlayerClass = new DisplayPlayerClass()
                {
                    ClassId = CurrentTeamMember.Class.ID,
                    Name = CurrentTeamMember.Class.Name,
                };
            }

            await base.OnParametersSetAsync();
        }

        private async Task UpdatePlayerClassID()
        {
            using PuzzleServerContext context = await ServerContextFactory.CreateDbContextAsync();

            // The team member has to be updated because the context changed,
            // so anything that's going to be edited or might have changed since page load needs to be retrieve in this context
            CurrentTeamMember = await UserEventHelper.GetTeamMemberForPlayer(context, Event, UserId);

            // Update the database with the new PlayerClass, if it's "Unassigned" then set it to null
            await TeamHelper.SetPlayerClass(context, Event, CurrentUserEventRole, CurrentTeamMember, SelectedPlayerClassID, IsTempClass);

            if (IsTempClass)
            {
                AvailablePlayerClasses = await TeamHelper.GetAllPlayerClassesSorted(context, EventId);

                CurrentTeamMemberDisplayPlayerClass = new DisplayPlayerClass()
                {
                    ClassId = CurrentTeamMember.TemporaryClass.ID,
                    Name = CurrentTeamMember.TemporaryClass.Name,
                };
            }
            else
            {
                // Get the new list of unassigned PlayerClasses
                AvailablePlayerClasses = await TeamHelper.GetAvailablePlayerClassesSorted(context, EventId, CurrentUserEventRole, CurrentTeamMember.Team.ID);

                CurrentTeamMemberDisplayPlayerClass = new DisplayPlayerClass()
                {
                    ClassId = CurrentTeamMember.Class.ID,
                    Name = CurrentTeamMember.Class.Name,
                };
            }
        }
    }
}

public class DisplayPlayerClass
{
    public int ClassId { get; set; }
    public string Name { get; set; }
}