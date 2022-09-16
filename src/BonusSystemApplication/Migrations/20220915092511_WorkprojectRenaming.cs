using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonusSystemApplication.Migrations
{
    public partial class WorkprojectRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormGlobalAccess_Workprojects_WorkrojectId",
                table: "FormGlobalAccess");

            migrationBuilder.DropIndex(
                name: "IX_FormGlobalAccess_WorkrojectId",
                table: "FormGlobalAccess");

            migrationBuilder.DropColumn(
                name: "WorkrojectId",
                table: "FormGlobalAccess");

            migrationBuilder.CreateIndex(
                name: "IX_FormGlobalAccess_WorkprojectId",
                table: "FormGlobalAccess",
                column: "WorkprojectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormGlobalAccess_Workprojects_WorkprojectId",
                table: "FormGlobalAccess",
                column: "WorkprojectId",
                principalTable: "Workprojects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormGlobalAccess_Workprojects_WorkprojectId",
                table: "FormGlobalAccess");

            migrationBuilder.DropIndex(
                name: "IX_FormGlobalAccess_WorkprojectId",
                table: "FormGlobalAccess");

            migrationBuilder.AddColumn<long>(
                name: "WorkrojectId",
                table: "FormGlobalAccess",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormGlobalAccess_WorkrojectId",
                table: "FormGlobalAccess",
                column: "WorkrojectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormGlobalAccess_Workprojects_WorkrojectId",
                table: "FormGlobalAccess",
                column: "WorkrojectId",
                principalTable: "Workprojects",
                principalColumn: "Id");
        }
    }
}
