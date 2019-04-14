using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class AddedApplicationUserAndFriendship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 32, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestedById = table.Column<string>(nullable: false),
                    RequestedToId = table.Column<string>(nullable: false),
                    FriendshipStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_User_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_User_RequestedToId",
                        column: x => x.RequestedToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_RequestedToId",
                table: "Friendships",
                column: "RequestedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_RequestedById_RequestedToId",
                table: "Friendships",
                columns: new[] { "RequestedById", "RequestedToId" },
                unique: true);

            migrationBuilder.Sql(
                "CREATE TRIGGER UniqueLastingOrPendingFriendship " +
                "ON Friendships " +
                "INSTEAD OF INSERT " +
                "AS " +
                "BEGIN " +
                "   DECLARE @friendshipStatus INT; " +
                "   SET @friendshipStatus = " +
                "       (SELECT Friendships.FriendshipStatus " +
                "        FROM Friendships, INSERTED " +
                "        WHERE INSERTED.RequestedToId = Friendships.RequestedById " +
                "            AND INSERTED.RequestedById = Friendships.RequestedToId); " +
                // Checking if RequestedTo have already sent an friend request to RequestedBy.
                "   IF @friendshipStatus = 0 THROW 50000, 'PendingFromRequestedTo', 1; " +
                "   IF @friendshipStatus = 2 THROW 50002, 'Lasts', 1; " +
                // @friendshipStatus IS NULL or @friendshipStatus = 1, so we can proceed.
                "   SET @friendshipStatus = " +
                "       (SELECT Friendships.FriendshipStatus " +
                "        FROM Friendships, INSERTED " +
                "        WHERE INSERTED.RequestedById = Friendships.RequestedById " +
                "            AND INSERTED.RequestedToId = Friendships.RequestedToId); " +
                " " +
                "   IF @friendshipStatus IS NULL " +
                "       INSERT INTO Friendships (RequestedById, RequestedToId, FriendshipStatus) " +
                "           (SELECT RequestedById, RequestedToId, FriendshipStatus FROM INSERTED); " + // better way to do this? the trigger isn't called recursively.
                "   ELSE IF @friendshipStatus = 0 THROW 50001, 'PendingFromRequestedBy', 1; " +
                "   ELSE IF @friendshipStatus = 2 THROW 50002, 'Lasts', 1; " +
                "   ELSE IF @friendshipStatus = 1 " +
                "       UPDATE Friendships SET Friendships.FriendshipStatus = INSERTED.FriendshipStatus " +
                "           FROM INSERTED " +
                "           WHERE INSERTED.RequestedById = Friendships.RequestedById " +
                "               AND INSERTED.RequestedToId = Friendships.RequestedToId; " + // code repetition, better way to do this?
                "   ELSE THROW 50004, 'NotSupportedValue', 1; " +
                "   SELECT Id FROM Friendships f WHERE @@ROWCOUNT > 0 AND f.Id = scope_identity(); " +
                "END ");

            migrationBuilder.Sql(
                "DROP TRIGGER IF EXISTS CascadeDeleteFriendships; ");

            migrationBuilder.Sql(
                "CREATE TRIGGER CascadeDeleteFriendships " +
                "ON [User] " +
                "AFTER DELETE " +
                "AS " +
                "BEGIN " +
                "   DELETE FROM Friendships " +
                "   FROM DELETED " +
                "   WHERE RequestedById = DELETED.Id OR RequestedToId = DELETED.Id; " +
                "END ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.Sql(
                "DROP TRIGGER IF EXISTS CascadeDeleteFriendships; ");
        }
    }
}
