using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditNewsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Newss",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Newss",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Newss_UserId1",
                table: "Newss",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Newss_Users_UserId1",
                table: "Newss",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Newss_Users_UserId1",
                table: "Newss");

            migrationBuilder.DropIndex(
                name: "IX_Newss_UserId1",
                table: "Newss");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Newss");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Newss");
        }
    }
}
