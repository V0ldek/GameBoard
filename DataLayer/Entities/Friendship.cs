using System.ComponentModel.DataAnnotations.Schema;
using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class Friendship
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RequestedById { get; set; }
        public string RequestedToId { get; set; }
        public ApplicationUser RequestedBy { get; set; }
        public ApplicationUser RequestedTo { get; set; }

        public FriendshipStatus FriendshipStatus { get; set; }
    }
}
