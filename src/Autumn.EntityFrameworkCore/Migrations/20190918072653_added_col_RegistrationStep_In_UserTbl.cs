using Microsoft.EntityFrameworkCore.Migrations;

namespace Autumn.Migrations
{
    public partial class added_col_RegistrationStep_In_UserTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistrationStep",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationStep",
                table: "AbpUsers");
        }
    }
}
