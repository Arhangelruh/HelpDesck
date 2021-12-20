using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpDesk.DAL.Migrations
{
    public partial class DeleteConnectionProblemProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Profiles_ProfileId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_ProfileId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "ProfileCreatorId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Problems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileCreatorId",
                table: "Problems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Problems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_ProfileId",
                table: "Problems",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Profiles_ProfileId",
                table: "Problems",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
