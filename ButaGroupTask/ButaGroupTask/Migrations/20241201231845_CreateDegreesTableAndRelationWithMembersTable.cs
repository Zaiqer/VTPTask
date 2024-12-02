using Microsoft.EntityFrameworkCore.Migrations;

namespace ButaGroupTask.Migrations
{
    public partial class CreateDegreesTableAndRelationWithMembersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_DegreeId",
                table: "Members",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Degrees_DegreeId",
                table: "Members",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Degrees_DegreeId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.DropIndex(
                name: "IX_Members_DegreeId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Members");
        }
    }
}
