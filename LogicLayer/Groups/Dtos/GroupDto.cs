using System.Collections.Generic;
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
        public IEnumerable<UserDto> Users { get; }

        public GroupDto(int groupId, [NotNull] string groupName, [NotNull] IEnumerable<UserDto> users)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
        }
    }
}