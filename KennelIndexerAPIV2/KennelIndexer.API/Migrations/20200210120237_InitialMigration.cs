using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KennelIndexer.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ReasonsForBeingOnTheList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PictureId = table.Column<Guid>(nullable: false),
                    PictureUri = table.Column<string>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.PictureId);
                    table.ForeignKey(
                        name: "FK_Pictures_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "PersonId", "Address", "FirstName", "LastName", "ReasonsForBeingOnTheList" },
                values: new object[] { new Guid("05c6c2ab-c63b-4c78-818f-39565d57bcf5"), null, "Thomas", "Holmegaard", null });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "PersonId", "Address", "FirstName", "LastName", "ReasonsForBeingOnTheList" },
                values: new object[] { new Guid("05c6c2ab-c63b-4c78-818f-39565d57acf5"), "Lollevej 22", "John", "Bommer", "Han var pyskopat..." });

            migrationBuilder.InsertData(
                table: "Pictures",
                columns: new[] { "PictureId", "PersonId", "PictureUri" },
                values: new object[] { new Guid("c54f132b-79aa-4986-aab4-b0da907ce843"), new Guid("05c6c2ab-c63b-4c78-818f-39565d57bcf5"), "located/somewhere/lol.png" });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_PersonId",
                table: "Pictures",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
