using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class AddedApplicationUserAndFriendRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_claim_user_UserId",
                table: "user_claim");

            migrationBuilder.DropForeignKey(
                name: "FK_user_login_user_UserId",
                table: "user_login");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_user_UserId",
                table: "user_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_token_user_UserId",
                table: "user_token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    UserFromId = table.Column<string>(nullable: true),
                    UserToId = table.Column<string>(nullable: true),
                    FriendRequestStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_UserToId",
                        column: x => x.UserToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_UserFromId",
                table: "FriendRequests",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_UserToId",
                table: "FriendRequests",
                column: "UserToId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_claim_AspNetUsers_UserId",
                table: "user_claim",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_login_AspNetUsers_UserId",
                table: "user_login",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_AspNetUsers_UserId",
                table: "user_role",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_token_AspNetUsers_UserId",
                table: "user_token",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_claim_AspNetUsers_UserId",
                table: "user_claim");

            migrationBuilder.DropForeignKey(
                name: "FK_user_login_AspNetUsers_UserId",
                table: "user_login");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_AspNetUsers_UserId",
                table: "user_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_token_AspNetUsers_UserId",
                table: "user_token");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "user");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_claim_user_UserId",
                table: "user_claim",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_login_user_UserId",
                table: "user_login",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_user_UserId",
                table: "user_role",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_token_user_UserId",
                table: "user_token",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
