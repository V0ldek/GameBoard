using GameBoard.LogicLayer.Groups.Dtos;

namespace GameBoard.Models.Groups
{
    public static class GroupDtoExtensions
    {
        public static GroupViewModel ToViewModel(this GroupDto groupDto) =>
            new GroupViewModel(groupDto.GroupName, groupDto.Users);
    }
}
