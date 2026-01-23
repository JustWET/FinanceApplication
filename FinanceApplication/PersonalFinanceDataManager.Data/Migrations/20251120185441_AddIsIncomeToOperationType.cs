using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceDataManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsIncomeToOperationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncome",
                table: "OperationTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncome",
                table: "OperationTypes");
        }
    }
}
