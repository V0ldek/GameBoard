using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;

namespace GameBoard.Models.Groups
{
    public class GroupViewModel
    {
        public string GroupName { get; }

        public ICollection<ApplicationUser> Users { get; }

        public GroupViewModel(string groupName, ICollection<ApplicationUser> users)
        {
            GroupName = groupName;
            Users = users;
        }
    }
}
