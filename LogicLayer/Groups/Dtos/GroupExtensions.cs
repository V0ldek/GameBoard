using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.Groups.Dtos
{
    public static class GroupExtensions
    {
        public static GroupDto ToDto(this Group group) => new GroupDto(
            group.Id,
            group.Name,
            group.Users);
    }
}