using System.Linq;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Groups.Dtos
{
    public static class GroupExtensions
    {
        public static GroupDto ToDto(this Group group) => new GroupDto(
            group.Id,
            group.Name,
            group.Users.Select(u => u.ToDto()).ToList());
    }
}