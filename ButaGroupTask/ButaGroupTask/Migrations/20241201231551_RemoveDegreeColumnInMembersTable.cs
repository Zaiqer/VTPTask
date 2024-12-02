using Microsoft.EntityFrameworkCore.Migrations;

namespace ButaGroupTask.Migrations
{
    public partial class RemoveDegreeColumnInMembersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
