using System.Linq;
using GameBoard.LogicLayer.Groups.Dtos;
using GameBoard.Models.User;

namespace GameBoard.Models.Groups
{
    public static class GroupDtoExtensions
    {
        public static GroupViewModel ToViewModel(this GroupDto groupDto) =>
            new GroupViewModel(groupDto.Id, groupDto.Name, groupDto.Users.Select(u => u.ToViewModel()));
    }
}