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
        public GameEvent InvitedTo{ get; set; }

        public InvitationStatus InvitationStatus { get; set; }
    }
}
