using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class GameEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameEvent",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 512, nullable: false),
                    MeetingTime = table.Column<long>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    CreatorId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEvent_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    GameEventId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => new { x.GameEventId, x.Name });
                    table.ForeignKey(
                        name: "FK_Game_GameEvent_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameEventInvitation",
                columns: table => new
                {
                    SendToId = table.Column<string>(nullable: false),
                    InvitedToId = table.Column<string>(nullable: false),
                    InvitationStatus = table.Column<string>(nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEventInvitation", x => new { x.SendToId, x.InvitedToId });
                    table.ForeignKey(
                        name: "FK_GameEventInvitation_GameEvent_InvitedToId",
                        column: x => x.InvitedToId,
                        principalTable: "GameEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameEventInvitation_User_SendToId",
                        column: x => x.SendToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEvent_CreatorId",
                table: "GameEvent",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventInvitation_InvitedToId",
                table: "GameEventInvitation",
                column: "InvitedToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "GameEventInvitation");

            migrationBuilder.DropTable(
                name: "GameEvent");
        }
    }
}
