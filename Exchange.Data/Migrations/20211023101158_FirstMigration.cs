using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exchange.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exchange");

            migrationBuilder.CreateTable(
                name: "daily_rates",
                schema: "exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "varchar", nullable: false),
                    name = table.Column<string>(type: "varchar", nullable: false),
                    currency_name = table.Column<string>(type: "varchar", nullable: false),
                    rate = table.Column<decimal>(type: "money", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_rates", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_rates",
                schema: "exchange");
        }
    }
}
