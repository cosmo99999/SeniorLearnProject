using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorLearnProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedEnrolmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Members_MemberId",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Enrolments_EnrolmentId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_EnrolmentId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "EnrolmentId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Enrolments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Enrolments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_LessonId",
                table: "Enrolments",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Lessons_LessonId",
                table: "Enrolments",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Members_MemberId",
                table: "Enrolments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Lessons_LessonId",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Members_MemberId",
                table: "Enrolments");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_LessonId",
                table: "Enrolments");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Enrolments");

            migrationBuilder.AddColumn<int>(
                name: "EnrolmentId",
                table: "Lessons",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Enrolments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_EnrolmentId",
                table: "Lessons",
                column: "EnrolmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Members_MemberId",
                table: "Enrolments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Enrolments_EnrolmentId",
                table: "Lessons",
                column: "EnrolmentId",
                principalTable: "Enrolments",
                principalColumn: "Id");
        }
    }
}
