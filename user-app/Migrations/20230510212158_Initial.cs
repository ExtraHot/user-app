using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace user_app.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    user_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.user_group_id);
                });

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    user_state_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.user_state_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    user_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_state_id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGroupId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    UserStateId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Users_UserGroups_UserGroupId1",
                        column: x => x.UserGroupId1,
                        principalTable: "UserGroups",
                        principalColumn: "user_group_id");
                    table.ForeignKey(
                        name: "FK_Users_UserGroups_user_group_id",
                        column: x => x.user_group_id,
                        principalTable: "UserGroups",
                        principalColumn: "user_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UserStates_UserStateId1",
                        column: x => x.UserStateId1,
                        principalTable: "UserStates",
                        principalColumn: "user_state_id");
                    table.ForeignKey(
                        name: "FK_Users_UserStates_user_state_id",
                        column: x => x.user_state_id,
                        principalTable: "UserStates",
                        principalColumn: "user_state_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "user_group_id", "code", "description" },
                values: new object[,]
                {
                    { new Guid("02f83e73-a729-4248-8a63-98472896253f"), 1, "Admin role" },
                    { new Guid("dd8735ae-0711-4e0c-960f-e10629fea835"), 0, "User role" }
                });

            migrationBuilder.InsertData(
                table: "UserStates",
                columns: new[] { "user_state_id", "code", "description" },
                values: new object[,]
                {
                    { new Guid("4940fe7e-8ade-42f6-b311-b7d86d1acc0e"), 0, "Active user status" },
                    { new Guid("75542e5b-3d6a-4541-8e75-e49ae7824e9c"), 1, "Blocked user status" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_code",
                table: "UserGroups",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_login",
                table: "Users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_user_group_id",
                table: "Users",
                column: "user_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_user_state_id",
                table: "Users",
                column: "user_state_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroupId1",
                table: "Users",
                column: "UserGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserStateId1",
                table: "Users",
                column: "UserStateId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserStates_code",
                table: "UserStates",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
