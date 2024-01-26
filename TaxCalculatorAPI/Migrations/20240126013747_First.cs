using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxCalculatorAPI.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialSecurityRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeInsuranceRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployerInsuranceRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeInsuranceBonusRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployerInsuranceBonusRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeInsuranceCareRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeInsuranceCareChildFreeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeInsuranceCareChildDiscountRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeMinChildrenDiscount = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeMaxChildrenDiscount = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployerInsuranceCareRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeePensionRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployerPensionRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployeeUnemploymentRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    EmployerUnemploymentRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    InsuranceMaxGross = table.Column<decimal>(type: "TEXT", nullable: false),
                    PensionAndUnimploymentMaxGross = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialSecurityRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    TaxFreeBasicFlat = table.Column<int>(type: "INTEGER", nullable: false),
                    TaxFreeEmployeeFlat = table.Column<int>(type: "INTEGER", nullable: false),
                    TaxFreeChildGrowingFlat = table.Column<int>(type: "INTEGER", nullable: false),
                    TaxFreeChildFlat = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxTaxLevel = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinLevelForSolidarityTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    SolidaryTaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ChurchTaxRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trackings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VisitCounter = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxInformationStep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxInformationId = table.Column<int>(type: "INTEGER", nullable: false),
                    StepAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxInformationStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxInformationStep_TaxInformation_TaxInformationId",
                        column: x => x.TaxInformationId,
                        principalTable: "TaxInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxInformationStep_TaxInformationId",
                table: "TaxInformationStep",
                column: "TaxInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialSecurityRates");

            migrationBuilder.DropTable(
                name: "TaxInformationStep");

            migrationBuilder.DropTable(
                name: "Trackings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TaxInformation");
        }
    }
}
