using System.Collections.Generic;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Groups.Dtos
{
    public sealed class GroupDto
    {
        public int GroupId { get; }

        [NotNull]
        public string GroupName { get; }

        [NotNull]
        public ICollection<UserDto> Users { get; }

        public GroupDto(int groupId, [NotNull] string groupName, [NotNull] ICollection<UserDto> users)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
        }
    }
}