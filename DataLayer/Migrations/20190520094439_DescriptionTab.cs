using System;
using System.IO;
using GameBoard.DataLayer.Migrations.MigrationBuilderExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class DescriptionTab : Migration
    {
        private static readonly string MigrationUpScriptFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Migrations/Scripts/20190520094439_DescriptionTab_Up.sql");

        private static readonly string MigrationDownScriptFilePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Migrations/Scripts/20190520094439_DescriptionTab_Down.sql");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DescriptionTab",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameEventId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionTab", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DescriptionTab_GameEvent_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionTab_GameEventId",
                table: "DescriptionTab",
                column: "GameEventId",
                unique: true);

            migrationBuilder.RunSqlScript(MigrationUpScriptFilePath);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescriptionTab");

            migrationBuilder.RunSqlScript(MigrationDownScriptFilePath);
        }
    }
}
