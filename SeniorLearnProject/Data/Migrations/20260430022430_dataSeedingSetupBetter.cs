using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorLearnProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class dataSeedingSetupBetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_DeliveryPlans_DeliveryPlanId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryPlanId",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_DeliveryPlans_DeliveryPlanId",
                table: "Lessons",
                column: "DeliveryPlanId",
                principalTable: "DeliveryPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_DeliveryPlans_DeliveryPlanId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryPlanId",
                table: "Lessons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_DeliveryPlans_DeliveryPlanId",
                table: "Lessons",
                column: "DeliveryPlanId",
                principalTable: "DeliveryPlans",
                principalColumn: "Id");
        }
    }
}
