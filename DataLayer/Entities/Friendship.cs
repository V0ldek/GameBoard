using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class Friendship
    {
        public int Id { get; set; }

        public string RequestedById { get; set; }
        public string RequestedToId { get; set; }
        public ApplicationUser RequestedBy { get; set; }
        public ApplicationUser RequestedTo { get; set; }

        public FriendshipStatus FriendshipStatus { get; set; }
    }
}