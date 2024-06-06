using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Residences_PlaceOfResidenceId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Users_UserId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_PlaceOfResidenceId",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "PlaceOfResidenceId",
                table: "People",
                newName: "ResidenceId");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "Residences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "People",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Residences_PersonId",
                table: "Residences",
                column: "PersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Users_UserId",
                table: "People",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Residences_People_PersonId",
                table: "Residences",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Users_UserId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_Residences_People_PersonId",
                table: "Residences");

            migrationBuilder.DropIndex(
                name: "IX_Residences_PersonId",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Residences");

            migrationBuilder.RenameColumn(
                name: "ResidenceId",
                table: "People",
                newName: "PlaceOfResidenceId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "People",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_People_PlaceOfResidenceId",
                table: "People",
                column: "PlaceOfResidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Residences_PlaceOfResidenceId",
                table: "People",
                column: "PlaceOfResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Users_UserId",
                table: "People",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
