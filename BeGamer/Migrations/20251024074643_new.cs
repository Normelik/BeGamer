using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeGamer.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEventParticipant_AspNetUsers_UserId",
                table: "GameEventParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEventParticipant",
                table: "GameEventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_GameEventParticipant_UserId",
                table: "GameEventParticipant");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GameEventParticipant",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameEventId",
                table: "GameEventParticipant",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEventParticipant",
                table: "GameEventParticipant",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipant_GameEventId",
                table: "GameEventParticipant",
                column: "GameEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventParticipant_AspNetUsers_Id",
                table: "GameEventParticipant",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEventParticipant_AspNetUsers_Id",
                table: "GameEventParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEventParticipant",
                table: "GameEventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_GameEventParticipant_GameEventId",
                table: "GameEventParticipant");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameEventParticipant",
                newName: "UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameEventId",
                table: "GameEventParticipant",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEventParticipant",
                table: "GameEventParticipant",
                columns: new[] { "GameEventId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameEventParticipant_UserId",
                table: "GameEventParticipant",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventParticipant_AspNetUsers_UserId",
                table: "GameEventParticipant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
