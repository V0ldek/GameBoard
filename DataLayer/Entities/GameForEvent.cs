using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GameBoard.DataLayer.Entities
{
    class GameForEvent
    {
        [Key]
        [ForeignKey("Game")]
        public string GameId { get; set; }

        [Key]
        [ForeignKey("User")]
        public string GameEventId { get; set; }
    }
}
