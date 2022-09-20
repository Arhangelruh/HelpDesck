using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace HelpDesk.DAL.Migrations
{
    public partial class ChangeStatusModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTime",
                schema: "event");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "Statuses",
                type: "character varying(127)",
                maxLength: 127,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(127)",
                oldMaxLength: 127,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Access",
                table: "Statuses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Queue",
                table: "Statuses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "Queue",
                table: "Statuses");

            migrationBuilder.EnsureSchema(
                name: "event");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "Statuses",
                type: "character varying(127)",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(127)",
                oldMaxLength: 127);

            migrationBuilder.CreateTable(
                name: "EventTime",
                schema: "event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTime", x => x.Id);
                });
        }
    }
}
