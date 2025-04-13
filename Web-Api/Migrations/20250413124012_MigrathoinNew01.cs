using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web_Api.Migrations
{
    /// <inheritdoc />
    public partial class MigrathoinNew01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TypeCooperation",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdProject",
                table: "Costs");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "Costs");

            migrationBuilder.DropColumn(
                name: "TypeCosts",
                table: "Costs");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdTeam",
                table: "Teams",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdCosts",
                table: "Costs",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Teams",
                newName: "IdTeam");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Costs",
                newName: "IdCosts");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeCooperation",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProject",
                table: "Costs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Percent",
                table: "Costs",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeCosts",
                table: "Costs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    IdProject = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTeam = table.Column<int>(type: "integer", nullable: true),
                    DateClosure = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.IdProject);
                    table.ForeignKey(
                        name: "FK_Projects_Teams_IdTeam",
                        column: x => x.IdTeam,
                        principalTable: "Teams",
                        principalColumn: "IdTeam");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IdTeam",
                table: "Projects",
                column: "IdTeam",
                unique: true);
        }
    }
}
