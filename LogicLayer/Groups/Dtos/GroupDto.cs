using System;
using System.Collections.Generic;
using System.Text;
using GameBoard.DataLayer.Entities;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Groups.Dtos
{
    public sealed class GroupDto
    {
        [NotNull]
        public int GroupId { get; }

        [NotNull]
        public string GroupName { get; }

        [NotNull]
        public ICollection<ApplicationUser> Users { get; }

        public GroupDto([NotNull] int groupId, [NotNull] string groupName, [NotNull] ICollection<ApplicationUser> users)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
        }
    }
}
