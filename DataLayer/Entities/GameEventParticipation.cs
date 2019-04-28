using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class GameEventParticipation
    {

        public string ParticipantId { get; set; }
        public ApplicationUser Paticipant { get; set; }
        public int TakesPartInId { get; set; }
        public GameEvent TakesPartIn { get; set; }

        public ParticipationStatus ParticipationStatus { get; set; }
    }
}
