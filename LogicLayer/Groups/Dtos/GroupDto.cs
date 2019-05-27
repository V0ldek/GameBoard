using System.Collections.Generic;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Groups.Dtos
{
    public sealed class GroupDto
    {
        public int Id { get; }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public IEnumerable<UserDto> Users { get; }

        public GroupDto(int id, [NotNull] string name, [NotNull] IEnumerable<UserDto> users)
        {
            Id = id;
            Name = name;
            Users = users;
        }
    }
}