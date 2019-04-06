using System.ComponentModel.DataAnnotations.Schema;
using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class Friendship
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserSmallerId { get; set; }
        public string UserGreaterId { get; set; }
        public ApplicationUser UserSmaller { get; set; }
        public ApplicationUser UserGreater { get; set; }

        public WhoSentLatestRequest WhoSent;
        public FriendshipStatus FriendshipStatus { get; set; }
    }
}
