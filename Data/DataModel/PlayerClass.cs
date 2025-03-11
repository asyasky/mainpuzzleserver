using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerCore.DataModel
{
    /// <summary>
    /// Classes or categories that players can select as part of registration for the event
    /// These classes can be used for event flavor or to determine content shown to those players
    /// </summary>
    public class PlayerClass
    {
        /// <summary>
        /// The ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// The event the puzzle is a part of
        /// </summary>
        public int EventID { get; set; }

        /// <summary>
        /// The event the puzzle is a part of
        /// </summary>
        [Required]
        public virtual Event Event { get; set; }

        /// <summary>
        /// The name of the player class (e.g. Warrior, Bard, Astronaut, etc)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// If true then only one player on the team can be this class
        /// </summary>
        public bool OnlyOnePerTeam { get; set; }
    }
}
