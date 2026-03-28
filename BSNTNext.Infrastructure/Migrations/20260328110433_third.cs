using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSNTNext.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_seedHistory",
                table: "seedHistory");

            migrationBuilder.RenameTable(
                name: "seedHistory",
                newName: "SeedHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeedHistory",
                table: "SeedHistory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconSvg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BadgeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModulePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PermissionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControllerAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulePermissions_AppModules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "AppModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserModuleAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    GrantedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModuleAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModuleAccesses_AppModules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "AppModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModuleAccesses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserModulePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModulePermissionId = table.Column<int>(type: "int", nullable: false),
                    GrantedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModulePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModulePermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserModulePermissions_ModulePermissions_ModulePermissionId",
                        column: x => x.ModulePermissionId,
                        principalTable: "ModulePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermissions_ModuleId",
                table: "ModulePermissions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleAccesses_ModuleId",
                table: "UserModuleAccesses",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleAccesses_UserId",
                table: "UserModuleAccesses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModulePermissions_ModulePermissionId",
                table: "UserModulePermissions",
                column: "ModulePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModulePermissions_UserId",
                table: "UserModulePermissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModuleAccesses");

            migrationBuilder.DropTable(
                name: "UserModulePermissions");

            migrationBuilder.DropTable(
                name: "ModulePermissions");

            migrationBuilder.DropTable(
                name: "AppModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeedHistory",
                table: "SeedHistory");

            migrationBuilder.RenameTable(
                name: "SeedHistory",
                newName: "seedHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_seedHistory",
                table: "seedHistory",
                column: "Id");
        }
    }
}
