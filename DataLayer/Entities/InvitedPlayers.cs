using System;
using GameBoard.DataLayer.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameBoard.DataLayer.Entities
{
    class Invited_Players
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Key]
        [ForeignKey("GameEvent")]
        public string GameEventId { get; set; }

        //Można spróbować dokonać zmian, aby Invitation Status miało wartość dowolną.
        //Jeśli będzie czas w ten sposób: https://stackoverflow.com/a/38102266
        public InvitationStatus InvitationStatus { get; set; }
    }
}
