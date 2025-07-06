using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoCollections_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoSharedCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TodoCollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoSharedCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoSharedCollections_TodoCollections_TodoCollectionId",
                        column: x => x.TodoCollectionId,
                        principalTable: "TodoCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoSharedCollections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    TodoCollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoTasks_TodoCollections_TodoCollectionId",
                        column: x => x.TodoCollectionId,
                        principalTable: "TodoCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoCollections_OwnerId",
                table: "TodoCollections",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoSharedCollections_TodoCollectionId",
                table: "TodoSharedCollections",
                column: "TodoCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoSharedCollections_UserId",
                table: "TodoSharedCollections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodoCollectionId",
                table: "TodoTasks",
                column: "TodoCollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoSharedCollections");

            migrationBuilder.DropTable(
                name: "TodoTasks");

            migrationBuilder.DropTable(
                name: "TodoCollections");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
