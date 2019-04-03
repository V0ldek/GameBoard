using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class FriendRequest
    {
        public string Id { get; set; }
        //[Key, Column(Order = 0)]
        public string UserFromId { get; set; }
        //[Key, Column(Order = 1)]
        public string UserToId { get; set; }
        public ApplicationUser UserFrom { get; set; }
        public ApplicationUser UserTo { get; set; }

        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}
