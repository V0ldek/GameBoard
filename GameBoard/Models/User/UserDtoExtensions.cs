using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.Models.User
{
    public static class UserDtoExtensions
    {
        public static UserViewModel ToViewModel(this UserDto userDto) =>
            new UserViewModel(userDto.Id, userDto.UserName, userDto.Email);
    }
}