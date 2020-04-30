using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Concurrency.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConcurrentAccountsWithRowVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "money", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrentAccountsWithRowVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConcurrentAccountsWithToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrentAccountsWithToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonConcurrentAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonConcurrentAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcurrentAccountsWithRowVersion");

            migrationBuilder.DropTable(
                name: "ConcurrentAccountsWithToken");

            migrationBuilder.DropTable(
                name: "NonConcurrentAccounts");
        }
    }
}
