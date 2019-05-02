using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.UserSearch.Dtos
{
    public static class ApplicationUserExtension
    {
        public static UserDto ToDto(this ApplicationUser applicationUser) =>
            new UserDto(applicationUser.Id, applicationUser.UserName, applicationUser.Email);
    }
}