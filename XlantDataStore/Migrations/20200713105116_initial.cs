﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XLantDataStore.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MLFSAdvisors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryID = table.Column<string>(nullable: true),
                    Title = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Grade = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Office = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MLFSAdvisors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MLFSReportingPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    FinancialYear = table.Column<string>(nullable: true),
                    ReportOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MLFSReportingPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryID = table.Column<string>(nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    MLFSAdvisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_MLFSAdvisors_MLFSAdvisorId",
                        column: x => x.MLFSAdvisorId,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryID = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false),
                    MLFSAdvisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddress_MLFSAdvisors_MLFSAdvisorId",
                        column: x => x.MLFSAdvisorId,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MLFSBudgets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvisorId = table.Column<string>(nullable: true),
                    MLFSReportPeriodId = table.Column<int>(nullable: false),
                    Budget = table.Column<decimal>(nullable: false),
                    MLFSAdvisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MLFSBudgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MLFSBudgets_MLFSAdvisors_MLFSAdvisorId",
                        column: x => x.MLFSAdvisorId,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MLFSCommissionRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvisorId = table.Column<int>(nullable: false),
                    StartingValue = table.Column<decimal>(nullable: false),
                    EndingValue = table.Column<decimal>(nullable: false),
                    Percentage = table.Column<decimal>(nullable: false),
                    MLFSAdvisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MLFSCommissionRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MLFSCommissionRates_MLFSAdvisors_MLFSAdvisorId",
                        column: x => x.MLFSAdvisorId,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Number",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryID = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false),
                    MLFSAdvisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Number", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Number_MLFSAdvisors_MLFSAdvisorId",
                        column: x => x.MLFSAdvisorId,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MLFSIncome",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IOReference = table.Column<string>(nullable: true),
                    MLFSReportPeriodId = table.Column<int>(nullable: true),
                    RelevantDate = table.Column<DateTime>(nullable: false),
                    Organisation = table.Column<string>(nullable: true),
                    AdvisorId = table.Column<string>(nullable: true),
                    ProviderName = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    JointClientName = table.Column<string>(nullable: true),
                    JointClientId = table.Column<string>(nullable: true),
                    Campaign = table.Column<string>(nullable: true),
                    CampaignSource = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    FeeStatus = table.Column<string>(nullable: true),
                    PlanType = table.Column<string>(nullable: true),
                    PlanNumber = table.Column<string>(nullable: true),
                    IsTopUp = table.Column<bool>(nullable: false),
                    IncomeType = table.Column<string>(nullable: true),
                    AdvisorId1 = table.Column<int>(nullable: true),
                    ReportingPeriodId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MLFSIncome", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MLFSIncome_MLFSAdvisors_AdvisorId1",
                        column: x => x.AdvisorId1,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MLFSIncome_MLFSReportingPeriods_ReportingPeriodId",
                        column: x => x.ReportingPeriodId,
                        principalTable: "MLFSReportingPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MLFSSales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IOId = table.Column<string>(nullable: true),
                    IOReference = table.Column<string>(nullable: true),
                    ReportPeriodId = table.Column<int>(nullable: true),
                    Organisation = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    JointClientName = table.Column<string>(nullable: true),
                    JointClientId = table.Column<string>(nullable: true),
                    AdvisorId = table.Column<string>(nullable: true),
                    AdvisorName = table.Column<string>(nullable: true),
                    ProviderName = table.Column<string>(nullable: true),
                    PlanType = table.Column<string>(nullable: true),
                    IsNew = table.Column<bool>(nullable: false),
                    RelevantDate = table.Column<DateTime>(nullable: false),
                    NetAmount = table.Column<decimal>(nullable: false),
                    VAT = table.Column<decimal>(nullable: false),
                    Investment = table.Column<decimal>(nullable: false),
                    OnGoingPercentage = table.Column<decimal>(nullable: false),
                    Paid = table.Column<decimal>(nullable: false),
                    Adjusted = table.Column<decimal>(nullable: false),
                    NotTakenUp = table.Column<bool>(nullable: false),
                    IncomeId = table.Column<int>(nullable: true),
                    PlanReference = table.Column<string>(nullable: true),
                    AdvisorId1 = table.Column<int>(nullable: true),
                    ReportingPeriodId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MLFSSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MLFSSales_MLFSAdvisors_AdvisorId1",
                        column: x => x.AdvisorId1,
                        principalTable: "MLFSAdvisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MLFSSales_MLFSIncome_IncomeId",
                        column: x => x.IncomeId,
                        principalTable: "MLFSIncome",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MLFSSales_MLFSReportingPeriods_ReportingPeriodId",
                        column: x => x.ReportingPeriodId,
                        principalTable: "MLFSReportingPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_MLFSAdvisorId",
                table: "Address",
                column: "MLFSAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress_MLFSAdvisorId",
                table: "EmailAddress",
                column: "MLFSAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSBudgets_MLFSAdvisorId",
                table: "MLFSBudgets",
                column: "MLFSAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSCommissionRates_MLFSAdvisorId",
                table: "MLFSCommissionRates",
                column: "MLFSAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSIncome_AdvisorId1",
                table: "MLFSIncome",
                column: "AdvisorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSIncome_ReportingPeriodId",
                table: "MLFSIncome",
                column: "ReportingPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSSales_AdvisorId1",
                table: "MLFSSales",
                column: "AdvisorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSSales_IncomeId",
                table: "MLFSSales",
                column: "IncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_MLFSSales_ReportingPeriodId",
                table: "MLFSSales",
                column: "ReportingPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Number_MLFSAdvisorId",
                table: "Number",
                column: "MLFSAdvisorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "EmailAddress");

            migrationBuilder.DropTable(
                name: "MLFSBudgets");

            migrationBuilder.DropTable(
                name: "MLFSCommissionRates");

            migrationBuilder.DropTable(
                name: "MLFSSales");

            migrationBuilder.DropTable(
                name: "Number");

            migrationBuilder.DropTable(
                name: "MLFSIncome");

            migrationBuilder.DropTable(
                name: "MLFSAdvisors");

            migrationBuilder.DropTable(
                name: "MLFSReportingPeriods");
        }
    }
}
