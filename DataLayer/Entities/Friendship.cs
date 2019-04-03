using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class Friendship
    {
        public string Id { get; set; }
        //[Key, Column(Order = 0)]
        public string UserFromId { get; set; }
        //[Key, Column(Order = 1)]
        public string UserToId { get; set; }
        public ApplicationUser UserFrom { get; set; }
        public ApplicationUser UserTo { get; set; }

        public FriendshipStatus FriendshipStatus { get; set; }
    }
}
