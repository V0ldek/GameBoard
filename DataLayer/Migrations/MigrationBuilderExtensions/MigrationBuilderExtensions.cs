using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace GameBoard.DataLayer.Migrations.MigrationBuilderExtensions
{
    internal static class MigrationBuilderExtensions
    {
        public static OperationBuilder<SqlOperation> RunSqlScript(this MigrationBuilder builder, string scriptPath) => builder.Sql(File.ReadAllText(scriptPath));
    }
}
