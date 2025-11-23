using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RenameFinancialBalancesToBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_FinancialBalances_FinancialBalanceId",
                table: "Payments");

            // Rename table
            migrationBuilder.RenameTable(
                name: "FinancialBalances",
                newName: "Balances");

            // Rename columns in Balances table
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Balances",
                newName: "PlannedAmount");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Balances",
                newName: "Description");

            // Rename index in Balances table
            migrationBuilder.RenameIndex(
                name: "IX_FinancialBalances_ProfileId",
                table: "Balances",
                newName: "IX_Balances_ProfileId");

            // Rename primary key
            migrationBuilder.RenameIndex(
                name: "PK_FinancialBalances",
                table: "Balances",
                newName: "PK_Balances");

            // Rename foreign key column in Payments table
            migrationBuilder.RenameColumn(
                name: "FinancialBalanceId",
                table: "Payments",
                newName: "BalanceId");

            // Rename index in Payments table
            migrationBuilder.RenameIndex(
                name: "IX_Payments_FinancialBalanceId",
                table: "Payments",
                newName: "IX_Payments_BalanceId");

            // Re-add foreign key constraint with new name
            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Balances_BalanceId",
                table: "Payments",
                column: "BalanceId",
                principalTable: "Balances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Balances_BalanceId",
                table: "Payments");

            // Rename foreign key column in Payments table back
            migrationBuilder.RenameColumn(
                name: "BalanceId",
                table: "Payments",
                newName: "FinancialBalanceId");

            // Rename index in Payments table back
            migrationBuilder.RenameIndex(
                name: "IX_Payments_BalanceId",
                table: "Payments",
                newName: "IX_Payments_FinancialBalanceId");

            // Rename primary key back
            migrationBuilder.RenameIndex(
                name: "PK_Balances",
                table: "Balances",
                newName: "PK_FinancialBalances");

            // Rename index in Balances table back
            migrationBuilder.RenameIndex(
                name: "IX_Balances_ProfileId",
                table: "Balances",
                newName: "IX_FinancialBalances_ProfileId");

            // Rename columns in Balances table back
            migrationBuilder.RenameColumn(
                name: "PlannedAmount",
                table: "Balances",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Balances",
                newName: "Category");

            // Rename table back
            migrationBuilder.RenameTable(
                name: "Balances",
                newName: "FinancialBalances");

            // Re-add foreign key constraint with old name
            migrationBuilder.AddForeignKey(
                name: "FK_Payments_FinancialBalances_FinancialBalanceId",
                table: "Payments",
                column: "FinancialBalanceId",
                principalTable: "FinancialBalances",
                principalColumn: "Id");
        }
    }
}
