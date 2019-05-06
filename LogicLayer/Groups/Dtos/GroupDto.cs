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
        public string GroupName { get; }

        [NotNull]
        public ICollection<ApplicationUser> Users { get; }

        public GroupDto([NotNull] string groupName, [NotNull] ICollection<ApplicationUser> users)
        {
            GroupName = groupName;
            Users = users;
        }
    }
}
