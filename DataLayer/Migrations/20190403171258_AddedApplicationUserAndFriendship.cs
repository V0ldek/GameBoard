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
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    RequestedById = table.Column<string>(nullable: true),
                    RequestedToId = table.Column<string>(nullable: true),
                    FriendshipStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_user_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_user_RequestedToId",
                        column: x => x.RequestedToId,
                        principalTable: "user",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");
        }
    }
}
