using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class ExitingAnEvent : Migration
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
                filter: "ParticipationStatus <> 3 AND ParticipationStatus <> 4");
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
                filter: "ParticipationStatus <> 3");
        }
    }
}
