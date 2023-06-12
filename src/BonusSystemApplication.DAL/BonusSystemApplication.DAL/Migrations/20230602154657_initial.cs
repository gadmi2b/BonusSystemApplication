using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonusSystemApplication.DAL.Migrations
{
    public partial class initial : Migration
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
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreObjectivesFrozen = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AreResultsFrozen = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastSavedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSavedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
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
                name: "Conclusions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverallKpi = table.Column<double>(type: "float", nullable: true),
                    IsProposalForBonusPayment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conclusions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conclusions_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
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
                    Objective_Statement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Objective_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Objective_IsKey = table.Column<bool>(type: "bit", nullable: false),
                    Objective_IsMeasurable = table.Column<bool>(type: "bit", nullable: false),
                    Objective_Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Objective_Threshold = table.Column<double>(type: "float", nullable: true),
                    Objective_Target = table.Column<double>(type: "float", nullable: true),
                    Objective_Challenge = table.Column<double>(type: "float", nullable: true),
                    Objective_WeightFactor = table.Column<double>(type: "float", nullable: true),
                    Objective_KpiUpperLimit = table.Column<double>(type: "float", nullable: true),
                    Result_KeyCheck = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result_Achieved = table.Column<double>(type: "float", nullable: true),
                    Result_Kpi = table.Column<double>(type: "float", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Signatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForObjectivesEmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForObjectivesIsSignedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForObjectivesIsRejectedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForObjectivesManagerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForObjectivesIsSignedByManager = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForObjectivesApproverSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForObjectivesIsSignedByApprover = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForResultsEmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForResultsIsSignedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForResultsIsRejectedByEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForResultsManagerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForResultsIsSignedByManager = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForResultsApproverSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForResultsIsSignedByApprover = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signatures_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Definitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsWpmHox = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ManagerId = table.Column<long>(type: "bigint", nullable: true),
                    ApproverId = table.Column<long>(type: "bigint", nullable: true),
                    WorkprojectId = table.Column<long>(type: "bigint", nullable: false),
                    FormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Definitions_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Definitions_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Definitions_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Definitions_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Definitions_Workprojects_WorkprojectId",
                        column: x => x.WorkprojectId,
                        principalTable: "Workprojects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GlobalAccess",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    WorkprojectId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalAccess_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GlobalAccess_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GlobalAccess_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlobalAccess_Workprojects_WorkprojectId",
                        column: x => x.WorkprojectId,
                        principalTable: "Workprojects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LocalAccess",
                columns: table => new
                {
                    FormId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalAccess", x => new { x.FormId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LocalAccess_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocalAccess_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conclusions_FormId",
                table: "Conclusions",
                column: "FormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_ApproverId",
                table: "Definitions",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_EmployeeId_WorkprojectId_Period_Year",
                table: "Definitions",
                columns: new[] { "EmployeeId", "WorkprojectId", "Period", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_FormId",
                table: "Definitions",
                column: "FormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_ManagerId",
                table: "Definitions",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_WorkprojectId",
                table: "Definitions",
                column: "WorkprojectId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalAccess_DepartmentId",
                table: "GlobalAccess",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalAccess_TeamId",
                table: "GlobalAccess",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalAccess_UserId",
                table: "GlobalAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalAccess_WorkprojectId",
                table: "GlobalAccess",
                column: "WorkprojectId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalAccess_UserId",
                table: "LocalAccess",
                column: "UserId");

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
                name: "IX_Signatures_FormId",
                table: "Signatures",
                column: "FormId",
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
                name: "Conclusions");

            migrationBuilder.DropTable(
                name: "Definitions");

            migrationBuilder.DropTable(
                name: "GlobalAccess");

            migrationBuilder.DropTable(
                name: "LocalAccess");

            migrationBuilder.DropTable(
                name: "ObjectivesResults");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropTable(
                name: "Workprojects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
