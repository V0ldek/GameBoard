namespace GameBoard.DataLayer.Entities
{
    public class GroupUser
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}