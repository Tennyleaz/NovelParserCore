using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NovelSiteParser.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookLinks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    IndexPage = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueLinks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    IndexNumber = table.Column<int>(nullable: false),
                    BookLinkId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueLinks_BookLinks_BookLinkId",
                        column: x => x.BookLinkId,
                        principalTable: "BookLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChapterLinks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Downloaded = table.Column<bool>(nullable: false),
                    IndexNumber = table.Column<int>(nullable: false),
                    IssueLinkId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterLinks_IssueLinks_IssueLinkId",
                        column: x => x.IssueLinkId,
                        principalTable: "IssueLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLinks_IssueLinkId",
                table: "ChapterLinks",
                column: "IssueLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueLinks_BookLinkId",
                table: "IssueLinks",
                column: "BookLinkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterLinks");

            migrationBuilder.DropTable(
                name: "IssueLinks");

            migrationBuilder.DropTable(
                name: "BookLinks");
        }
    }
}
