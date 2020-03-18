using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Autumn.Migrations
{
    public partial class Added_AutumnOnboarding_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AbpUsers",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    CountryCode = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CurrencySymbol = table.Column<string>(nullable: false),
                    RetirementAge = table.Column<int>(maxLength: 3, nullable: false),
                    LifeExpectancy = table.Column<int>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRetirementPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RetirementGoalOptions = table.Column<int>(nullable: true),
                    DesiredRetirementSum = table.Column<double>(nullable: true),
                    DesiredRetirementIncome = table.Column<double>(nullable: true),
                    DesiredLegacyAmount = table.Column<double>(nullable: true),
                    DesiredRetirementAge = table.Column<int>(maxLength: 2, nullable: true),
                    ReturnRate = table.Column<float>(nullable: true),
                    InitialSaved = table.Column<double>(maxLength: 15, nullable: true),
                    InitialOwed = table.Column<double>(maxLength: 15, nullable: true),
                    InitialNet = table.Column<double>(nullable: true),
                    RequiredSavings = table.Column<double>(nullable: true),
                    TotalMonthlyIncome = table.Column<double>(nullable: true),
                    TotalMonthlyExpences = table.Column<double>(nullable: true),
                    TotalMonthlySavings = table.Column<double>(maxLength: 15, nullable: true),
                    TotalLiabilities = table.Column<double>(maxLength: 15, nullable: true),
                    LikelyRetirementSum = table.Column<double>(nullable: true),
                    LikelyRetirementIncome = table.Column<double>(maxLength: 12, nullable: true),
                    LikelyRetirementLegacy = table.Column<double>(nullable: true),
                    LikelyRetirementAge = table.Column<double>(maxLength: 2, nullable: true),
                    DesiredRetirementProgress = table.Column<string>(nullable: true),
                    FamilyView = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRetirementPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRetirementPlans_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_CountryId",
                table: "AbpUsers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRetirementPlans_UserId",
                table: "UserRetirementPlans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Country_CountryId",
                table: "AbpUsers",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Country_CountryId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "UserRetirementPlans");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_CountryId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AbpUsers");
        }
    }
}
