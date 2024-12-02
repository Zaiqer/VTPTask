using Microsoft.EntityFrameworkCore.Migrations;

namespace ButaGroupTask.Migrations
{
    public partial class AddImageColumnToMembersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Members");
        }
    }
}
