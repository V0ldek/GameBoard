using System;
using GameBoard.DataLayer.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameBoard.DataLayer.Entities
{
    public class GameEventInvitation
    {

        public string SendToId { get; set; }
        public ApplicationUser SendTo { get; set; }
        public string InvitedToId { get; set; }
        public GameEvent IvitedTo{ get; set; }

        //Można spróbować dokonać zmian, aby InvitationStatus miało wartość dowolną.
        //Jeśli będzie czas w ten sposób: https://stackoverflow.com/a/38102266
        public InvitationStatus InvitationStatus { get; set; }
    }
}
