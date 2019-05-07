using System.Collections.Generic;
using System.Linq;

namespace GameBoard.DataLayer.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public string Name { get; set; }

        public ICollection<GroupUser> GroupUser { get; set; }

        public ICollection<ApplicationUser> Users
        {
            get => GroupUser.Select(gu => gu.User).ToList();
            set => GroupUser = value.Select(
                u => new GroupUser
                {
                    Group = this,
                    User = u
                }).ToList();
        }
    }
}