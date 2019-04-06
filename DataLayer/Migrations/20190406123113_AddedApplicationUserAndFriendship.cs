using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserSmallerId = table.Column<string>(nullable: true),
                    UserGreaterId = table.Column<string>(nullable: true),
                    FriendshipStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_user_UserGreaterId",
                        column: x => x.UserGreaterId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_user_UserSmallerId",
                        column: x => x.UserSmallerId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserGreaterId",
                table: "Friendships",
                column: "UserGreaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserSmallerId_UserGreaterId",
                table: "Friendships",
                columns: new[] { "UserSmallerId", "UserGreaterId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");
        }
    }
}
