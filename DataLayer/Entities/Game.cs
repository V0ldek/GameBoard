using System;
using System.ComponentModel.DataAnnotations;


namespace GameBoard.DataLayer.Entities
{
    class Game
    {
        [Key]
        [Required]
        public String Name { get; set; }

        [Required]
        public Int16 MinimalPlayersNumber { get; set; }
    
        [Required]
        public Int16 MaximalPlayersNumber { get; set; }

    }
}
