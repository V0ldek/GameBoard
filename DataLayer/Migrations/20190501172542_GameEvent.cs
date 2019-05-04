using Microsoft.EntityFrameworkCore.Metadata;
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
                    Id = table.Column<int>(maxLength: 512, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    MeetingTime = table.Column<long>(nullable: false),
                    Place = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    GameEventId = table.Column<int>(nullable: false)
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
                    ParticipantId = table.Column<string>(nullable: false),
                    TakesPartInId = table.Column<int>(nullable: false),
                    ParticipationStatus = table.Column<string>(nullable: false, defaultValue: "PendingGuest")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEventInvitation", x => new { x.ParticipantId, x.TakesPartInId });
                    table.ForeignKey(
                        name: "FK_GameEventInvitation_User_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameEventInvitation_GameEvent_TakesPartInId",
                        column: x => x.TakesPartInId,
                        principalTable: "GameEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEventInvitation_TakesPartInId",
                table: "GameEventInvitation",
                column: "TakesPartInId");
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
