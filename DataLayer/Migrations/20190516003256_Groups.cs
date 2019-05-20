using System;
using System.IO;
using GameBoard.DataLayer.Migrations.MigrationBuilderExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameBoard.DataLayer.Migrations
{
    public partial class Groups : Migration
    {
        private static readonly string MigrationUpScriptFilePath = 
            Path.Combine(
            AppContext.BaseDirectory,
            "Migrations/Scripts/20190516003256_Groups_Up.sql");

        private static readonly string MigrationDownScriptFilePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Migrations/Scripts/20190516003256_Groups_Down.sql");

        private static readonly string MigrationAddAllGroupToUserAccountsScriptFilePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Migrations/Scripts/20190516003256_AddAllGroupToUserAccounts.sql");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_OwnerId",
                table: "Group",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UserId",
                table: "GroupUser",
                column: "UserId");

            migrationBuilder.RunSqlScript(MigrationUpScriptFilePath);
            migrationBuilder.RunSqlScript(MigrationAddAllGroupToUserAccountsScriptFilePath);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.RunSqlScript(MigrationDownScriptFilePath);
        }
    }
}
