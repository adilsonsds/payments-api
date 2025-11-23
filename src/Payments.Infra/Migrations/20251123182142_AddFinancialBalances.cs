using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinancialBalanceId",
                table: "Payments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProfileId = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Category = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialBalances_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FinancialBalanceId",
                table: "Payments",
                column: "FinancialBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialBalances_ProfileId",
                table: "FinancialBalances",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_FinancialBalances_FinancialBalanceId",
                table: "Payments",
                column: "FinancialBalanceId",
                principalTable: "FinancialBalances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_FinancialBalances_FinancialBalanceId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "FinancialBalances");

            migrationBuilder.DropIndex(
                name: "IX_Payments_FinancialBalanceId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FinancialBalanceId",
                table: "Payments");
        }
    }
}
