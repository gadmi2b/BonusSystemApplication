using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonusSystemApplication.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEng = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameRus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workprojects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workprojects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastNameEng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstNameEng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastNameRus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstNameRus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleNameRus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Pid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    PositionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FormGlobalAccess",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    WorkprojectId = table.Column<long>(type: "bigint", nullable: true),
                    WorkrojectId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormGlobalAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormGlobalAccess_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormGlobalAccess_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormGlobalAccess_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormGlobalAccess_Workprojects_WorkrojectId",
                        column: x => x.WorkrojectId,
                        principalTable: "Workprojects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LastSavedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSavedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallKpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWpmHox = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsProposalForBonusPayment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsObjectivesFreezed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ObjectivesEmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsObjectivesSignedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsObjectivesRejectedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ObjectivesManagerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsObjectivesSignedByManager = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ObjectivesApproverSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsObjectivesSignedByApprover = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsResultsFreezed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ResultsEmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResultsSignedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsResultsRejectedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ResultsManagerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResultsSignedByManager = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ResultsApproverSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResultsSignedByApprover = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ManagerId = table.Column<long>(type: "bigint", nullable: true),
                    ApproverId = table.Column<long>(type: "bigint", nullable: true),
                    WorkprojectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Forms_Workprojects_WorkprojectId",
                        column: x => x.WorkprojectId,
                        principalTable: "Workprojects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormLocalAccess",
                columns: table => new
                {
                    FormId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormLocalAccess", x => new { x.FormId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FormLocalAccess_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormLocalAccess_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectivesResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Statement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsKey = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsMeasurable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Threshold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Challenge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeightFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KpiUpperLimit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyCheck = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achieved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectivesResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectivesResults_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormGlobalAccess_DepartmentId",
                table: "FormGlobalAccess",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormGlobalAccess_TeamId",
                table: "FormGlobalAccess",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FormGlobalAccess_UserId",
                table: "FormGlobalAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormGlobalAccess_WorkrojectId",
                table: "FormGlobalAccess",
                column: "WorkrojectId");

            migrationBuilder.CreateIndex(
                name: "IX_FormLocalAccess_UserId",
                table: "FormLocalAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ApproverId",
                table: "Forms",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_EmployeeId_WorkprojectId_Period_Year",
                table: "Forms",
                columns: new[] { "EmployeeId", "WorkprojectId", "Period", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ManagerId",
                table: "Forms",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_WorkprojectId",
                table: "Forms",
                column: "WorkprojectId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectivesResults_FormId_Row",
                table: "ObjectivesResults",
                columns: new[] { "FormId", "Row" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_NameEng",
                table: "Positions",
                column: "NameEng",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_NameRus",
                table: "Positions",
                column: "NameRus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Pid",
                table: "Users",
                column: "Pid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PositionId",
                table: "Users",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Workprojects_Name",
                table: "Workprojects",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormGlobalAccess");

            migrationBuilder.DropTable(
                name: "FormLocalAccess");

            migrationBuilder.DropTable(
                name: "ObjectivesResults");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workprojects");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
