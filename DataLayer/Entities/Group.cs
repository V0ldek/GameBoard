using System.Collections.Generic;

namespace GameBoard.DataLayer.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}