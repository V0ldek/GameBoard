namespace GameBoard.DataLayer.Enums
{
    // If you want to change numbers, then change them in the trigger in AddedApplicationUserAndFriendship migration as well.
    public enum FriendshipStatus
    {
        PendingFriendRequest = 0,
        Rejected = 1,
        Lasts = 2
    }
}