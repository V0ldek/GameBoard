using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;

namespace GameBoard.Models.Groups
{
    public class GroupViewModel
    {
        public int GroupId { get; }

        public string GroupName { get; }

        public ICollection<ApplicationUser> Users { get; }

        public GroupViewModel(int groupId, string groupName, ICollection<ApplicationUser> users)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
        }
    }
}
