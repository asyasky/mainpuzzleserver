using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using ServerCore.DataModel;

namespace ServerCore.Pages.Events
{
    public partial class PlayerClassComponent
    {
        [Parameter]
        public int EventId { get; set; }

        [Inject]
        public PuzzleServerContext _context { get; set; }

        List<PlayerClass> PlayerClasses = new List<PlayerClass>();
    }
}
