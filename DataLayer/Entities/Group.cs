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

        public ICollection<GroupUser> GroupUsers { get; set; }

        public IEnumerable<ApplicationUser> Users => GroupUsers?.Select(gu => gu.User);
    }
}