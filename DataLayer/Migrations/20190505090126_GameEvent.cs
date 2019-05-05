using GameBoard.DataLayer.Migrations.MigrationBuilderExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace GameBoard.DataLayer.Migrations
{
    public partial class GameEvent : Migration
    {
        private static readonly string MigrationUpScriptFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Migrations/Scripts/GameEvent_Up.sql");

        private static readonly string MigrationDownScriptFilePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Migrations/Scripts/GameEvent_Down.sql");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 48, nullable: true),
                    MeetingTime = table.Column<long>(nullable: true),
                    Place = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    GameEventId = table.Column<int>(nullable: false),
                    GameStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_GameEvent_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameEventParticipation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParticipantId = table.Column<string>(nullable: false),
                    TakesPartInId = table.Column<int>(nullable: false),
                    ParticipationStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEventParticipation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEventParticipation_User_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameEventParticipation_GameEvent_TakesPartInId",
                        column: x => x.TakesPartInId,
                        principalTable: "GameEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_GameEventId_Name",
                table: "Game",
                columns: new[] { "GameEventId", "Name" },
                unique: true,
                filter: "GameStatus = 'ExistsOnTheList'");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipation_ParticipantId",
                table: "GameEventParticipation",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipation_TakesPartInId",
                table: "GameEventParticipation",
                column: "TakesPartInId",
                unique: true,
                filter: "ParticipationStatus = 'Creator'");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipation_TakesPartInId_ParticipantId",
                table: "GameEventParticipation",
                columns: new[] { "TakesPartInId", "ParticipantId" },
                unique: true,
                filter: "ParticipationStatus <> 'RejectedGuest'");

            migrationBuilder.RunSqlScript(MigrationUpScriptFilePath);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "GameEventParticipation");

            migrationBuilder.DropTable(
                name: "GameEvent");

            migrationBuilder.RunSqlScript(MigrationDownScriptFilePath);
        }
    }
}
