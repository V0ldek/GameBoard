using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static bool UserNameEquals(this ApplicationUser applicationUser, string userName) =>
            applicationUser.NormalizedUserName == userName.ToUpper();
    }
}