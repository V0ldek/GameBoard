using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class RemovingFromGameEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameEventParticipation_TakesPartInId_ParticipantId",
                table: "GameEventParticipation");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipation_TakesPartInId_ParticipantId",
                table: "GameEventParticipation",
                columns: new[] { "TakesPartInId", "ParticipantId" },
                unique: true,
                filter: "ParticipationStatus <> 3 AND ParticipationStatus <> 4 AND ParticipationStatus <> 5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameEventParticipation_TakesPartInId_ParticipantId",
                table: "GameEventParticipation");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipation_TakesPartInId_ParticipantId",
                table: "GameEventParticipation",
                columns: new[] { "TakesPartInId", "ParticipantId" },
                unique: true,
                filter: "ParticipationStatus <> 3 AND ParticipationStatus <> 4");
        }
    }
}
