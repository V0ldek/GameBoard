using System;
using System.IO;
using GameBoard.DataLayer.Migrations.MigrationBuilderExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class AddedApplicationUserAndFriendship : Migration
    {
        private static readonly string MigrationUpScriptFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Migrations/Scripts/20190414094415_AddedApplicationUserAndFriendship_Up.sql");

        private static readonly string MigrationDownScriptFilePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Migrations/Scripts/20190414094415_AddedApplicationUserAndFriendship_Down.sql");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestedById = table.Column<string>(nullable: false),
                    RequestedToId = table.Column<string>(nullable: false),
                    FriendshipStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendship_User_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendship_User_RequestedToId",
                        column: x => x.RequestedToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_RequestedToId",
                table: "Friendship",
                column: "RequestedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_RequestedById_RequestedToId",
                table: "Friendship",
                columns: new[] { "RequestedById", "RequestedToId" },
                unique: true);

            migrationBuilder.RunSqlScript(MigrationUpScriptFilePath);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.RunSqlScript(MigrationDownScriptFilePath);
        }
    }
}
