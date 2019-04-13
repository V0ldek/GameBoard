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
                    RequestedById = table.Column<string>(nullable: true),
                    RequestedToId = table.Column<string>(nullable: true),
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
                unique: true,
                filter: "[RequestedById] IS NOT NULL AND [RequestedToId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");
        }
    }
}
