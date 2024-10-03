using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskingSystem.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "security",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "security",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "varchar(250)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Password = table.Column<string>(type: "varchar(500)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IdRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUserUpdated = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.IdRole,
                        principalSchema: "security",
                        principalTable: "Role",
                        principalColumn: "IdRole");
                    table.ForeignKey(
                        name: "FK_User_User_IdUserCreated",
                        column: x => x.IdUserCreated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK_User_User_IdUserUpdated",
                        column: x => x.IdUserUpdated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "security",
                columns: table => new
                {
                    IdPermission = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUserUpdated = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.IdPermission);
                    table.ForeignKey(
                        name: "FK_Permission_User_IdUserCreated",
                        column: x => x.IdUserCreated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK_Permission_User_IdUserUpdated",
                        column: x => x.IdUserUpdated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "StatusSystem",
                schema: "dbo",
                columns: table => new
                {
                    IdStatusSystem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Entity = table.Column<string>(type: "varchar(100)", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUserUpdated = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusSystem", x => x.IdStatusSystem);
                    table.ForeignKey(
                        name: "FK_StatusSystem_User_IdUserCreated",
                        column: x => x.IdUserCreated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK_StatusSystem_User_IdUserUpdated",
                        column: x => x.IdUserUpdated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "security",
                columns: table => new
                {
                    IdPermission = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.IdRole, x.IdPermission });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_IdPermission",
                        column: x => x.IdPermission,
                        principalSchema: "security",
                        principalTable: "Permission",
                        principalColumn: "IdPermission");
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_IdRole",
                        column: x => x.IdRole,
                        principalSchema: "security",
                        principalTable: "Role",
                        principalColumn: "IdRole");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                schema: "dbo",
                columns: table => new
                {
                    IdTask = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IdStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUserUpdated = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.IdTask);
                    table.ForeignKey(
                        name: "FK_Task_StatusSystem_IdStatus",
                        column: x => x.IdStatus,
                        principalSchema: "dbo",
                        principalTable: "StatusSystem",
                        principalColumn: "IdStatusSystem");
                    table.ForeignKey(
                        name: "FK_Task_User_IdUserCreated",
                        column: x => x.IdUserCreated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK_Task_User_IdUserUpdated",
                        column: x => x.IdUserUpdated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK_User_Task",
                        column: x => x.IdUser,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "WorkItemFlow",
                schema: "dbo",
                columns: table => new
                {
                    IdTaskFlow = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdWorkItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPreviousStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNewStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Observation = table.Column<string>(type: "varchar(1000)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFlow", x => x.IdTaskFlow);
                    table.ForeignKey(
                        name: "FK_Flow_Task",
                        column: x => x.IdWorkItem,
                        principalSchema: "dbo",
                        principalTable: "Task",
                        principalColumn: "IdTask");
                    table.ForeignKey(
                        name: "FK_WorkItemFlow_StatusSystem_IdNewStatus",
                        column: x => x.IdNewStatus,
                        principalSchema: "dbo",
                        principalTable: "StatusSystem",
                        principalColumn: "IdStatusSystem");
                    table.ForeignKey(
                        name: "FK_WorkItemFlow_StatusSystem_IdPreviousStatus",
                        column: x => x.IdPreviousStatus,
                        principalSchema: "dbo",
                        principalTable: "StatusSystem",
                        principalColumn: "IdStatusSystem");
                    table.ForeignKey(
                        name: "FK_WorkItemFlow_Task_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: "dbo",
                        principalTable: "Task",
                        principalColumn: "IdTask");
                    table.ForeignKey(
                        name: "FK_WorkItemFlow_User_IdUserCreated",
                        column: x => x.IdUserCreated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "WorkItemLog",
                schema: "dbo",
                columns: table => new
                {
                    IdTaskLog = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdWorkItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeEvent = table.Column<string>(type: "varchar(50)", nullable: false),
                    LogEvent = table.Column<string>(type: "varchar(1000)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdUserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLog", x => x.IdTaskLog);
                    table.ForeignKey(
                        name: "FK_Log_Task",
                        column: x => x.IdWorkItem,
                        principalSchema: "dbo",
                        principalTable: "Task",
                        principalColumn: "IdTask");
                    table.ForeignKey(
                        name: "FK_WorkItemLog_Task_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: "dbo",
                        principalTable: "Task",
                        principalColumn: "IdTask");
                    table.ForeignKey(
                        name: "FK_WorkItemLog_User_IdUserCreated",
                        column: x => x.IdUserCreated,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Role",
                columns: new[] { "IdRole", "Active", "Name" },
                values: new object[,]
                {
                    { new Guid("4cca98ee-83fb-4357-a851-2635841e056e"), true, "Supervisor" },
                    { new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4"), true, "Administrador" },
                    { new Guid("dd381045-9efa-40f8-afd1-f46879b8c2d7"), true, "Empleado" }
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "User",
                columns: new[] { "IdUser", "Active", "DateCreated", "DateUpdated", "FullName", "IdRole", "IdUserCreated", "IdUserUpdated", "Password", "UserName" },
                values: new object[] { new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), true, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6826), null, "Usuario Administrador", new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4"), new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null, "10000.FuFwFM9Ir84N+zAMeRkAUg==.pr10sR5IPgFVX/FVknK7qSd9Z5MUiM3Y6GpgyC0Y6Z0=", "admin" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Permission",
                columns: new[] { "IdPermission", "ActionType", "DateCreated", "DateUpdated", "Entity", "IdUserCreated", "IdUserUpdated" },
                values: new object[,]
                {
                    { new Guid("02573a63-e73f-47c4-98ef-10e27557ebe0"), 2, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6886), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("0aab002a-26b9-4460-b0ce-dcebfd232aa6"), 1, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6884), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("4630b441-bc83-4f1f-9600-47802b5bd685"), 2, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6880), null, "USER", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("59113ba2-fbeb-4356-af8c-858a5f7eb787"), 1, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6877), null, "USER", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("844fd0a0-a715-445d-ad1e-c39fd7033e39"), 6, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6892), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("939fc5b4-484d-46b2-b0b6-523e780670f0"), 3, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6881), null, "USER", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("9e5333a7-651d-4001-970a-b5ee57583198"), 4, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6882), null, "USER", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("d34acf6b-7cc0-4bb8-872d-9f7c7189bd1e"), 4, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6890), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("f6f008cf-9f05-477f-bf78-83fa14275b45"), 3, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6887), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null },
                    { new Guid("fc8779b9-0ca7-4105-8876-453f38a3b0ca"), 5, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6891), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "StatusSystem",
                columns: new[] { "IdStatusSystem", "Active", "Code", "DateCreated", "DateUpdated", "Entity", "IdUserCreated", "IdUserUpdated", "Name" },
                values: new object[,]
                {
                    { new Guid("5e9962dc-a63b-423a-b584-b75b46496f56"), true, "COMPLETED", new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6951), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null, "Completada" },
                    { new Guid("b1313626-e0c9-4c5f-816e-db9ee365079a"), true, "IN_PROCESS", new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6949), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null, "En Proceso" },
                    { new Guid("d40b4c74-d443-4786-b06c-da82691319fa"), true, "PENDING", new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6947), null, "TASK", new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null, "Pendiente" }
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "User",
                columns: new[] { "IdUser", "Active", "DateCreated", "DateUpdated", "FullName", "IdRole", "IdUserCreated", "IdUserUpdated", "Password", "UserName" },
                values: new object[,]
                {
                    { new Guid("11695b42-fc05-47ea-b2e5-c96e78460b2b"), true, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6852), null, "Usuario Supervidor", new Guid("4cca98ee-83fb-4357-a851-2635841e056e"), new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null, "10000.FuFwFM9Ir84N+zAMeRkAUg==.pr10sR5IPgFVX/FVknK7qSd9Z5MUiM3Y6GpgyC0Y6Z0=", "super" },
                    { new Guid("1b823b39-13bc-4aa9-ad80-b8183f317dab"), true, new DateTime(2024, 10, 2, 23, 9, 54, 520, DateTimeKind.Utc).AddTicks(6854), null, "Usuario Empleado", new Guid("dd381045-9efa-40f8-afd1-f46879b8c2d7"), new Guid("f2d08e38-24bf-49b7-929b-d21f065dd6e2"), null, "10000.FuFwFM9Ir84N+zAMeRkAUg==.pr10sR5IPgFVX/FVknK7qSd9Z5MUiM3Y6GpgyC0Y6Z0=", "empleado" }
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "RolePermission",
                columns: new[] { "IdPermission", "IdRole" },
                values: new object[,]
                {
                    { new Guid("02573a63-e73f-47c4-98ef-10e27557ebe0"), new Guid("4cca98ee-83fb-4357-a851-2635841e056e") },
                    { new Guid("0aab002a-26b9-4460-b0ce-dcebfd232aa6"), new Guid("4cca98ee-83fb-4357-a851-2635841e056e") },
                    { new Guid("844fd0a0-a715-445d-ad1e-c39fd7033e39"), new Guid("4cca98ee-83fb-4357-a851-2635841e056e") },
                    { new Guid("f6f008cf-9f05-477f-bf78-83fa14275b45"), new Guid("4cca98ee-83fb-4357-a851-2635841e056e") },
                    { new Guid("fc8779b9-0ca7-4105-8876-453f38a3b0ca"), new Guid("4cca98ee-83fb-4357-a851-2635841e056e") },
                    { new Guid("02573a63-e73f-47c4-98ef-10e27557ebe0"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("0aab002a-26b9-4460-b0ce-dcebfd232aa6"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("4630b441-bc83-4f1f-9600-47802b5bd685"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("59113ba2-fbeb-4356-af8c-858a5f7eb787"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("939fc5b4-484d-46b2-b0b6-523e780670f0"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("9e5333a7-651d-4001-970a-b5ee57583198"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("d34acf6b-7cc0-4bb8-872d-9f7c7189bd1e"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("f6f008cf-9f05-477f-bf78-83fa14275b45"), new Guid("c7d636c0-54be-472a-9a53-ba17ece3d4d4") },
                    { new Guid("02573a63-e73f-47c4-98ef-10e27557ebe0"), new Guid("dd381045-9efa-40f8-afd1-f46879b8c2d7") },
                    { new Guid("844fd0a0-a715-445d-ad1e-c39fd7033e39"), new Guid("dd381045-9efa-40f8-afd1-f46879b8c2d7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_IdUserCreated",
                schema: "security",
                table: "Permission",
                column: "IdUserCreated");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_IdUserUpdated",
                schema: "security",
                table: "Permission",
                column: "IdUserUpdated");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                schema: "security",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_IdPermission",
                schema: "security",
                table: "RolePermission",
                column: "IdPermission");

            migrationBuilder.CreateIndex(
                name: "IX_StatusSystem_IdUserCreated",
                schema: "dbo",
                table: "StatusSystem",
                column: "IdUserCreated");

            migrationBuilder.CreateIndex(
                name: "IX_StatusSystem_IdUserUpdated",
                schema: "dbo",
                table: "StatusSystem",
                column: "IdUserUpdated");

            migrationBuilder.CreateIndex(
                name: "IX_StatusSystem_Name",
                schema: "dbo",
                table: "StatusSystem",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_IdStatus",
                schema: "dbo",
                table: "Task",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Task_IdUser",
                schema: "dbo",
                table: "Task",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Task_IdUserCreated",
                schema: "dbo",
                table: "Task",
                column: "IdUserCreated");

            migrationBuilder.CreateIndex(
                name: "IX_Task_IdUserUpdated",
                schema: "dbo",
                table: "Task",
                column: "IdUserUpdated");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRole",
                schema: "security",
                table: "User",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdUserCreated",
                schema: "security",
                table: "User",
                column: "IdUserCreated");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdUserUpdated",
                schema: "security",
                table: "User",
                column: "IdUserUpdated");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                schema: "security",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemFlow_IdNewStatus",
                schema: "dbo",
                table: "WorkItemFlow",
                column: "IdNewStatus");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemFlow_IdPreviousStatus",
                schema: "dbo",
                table: "WorkItemFlow",
                column: "IdPreviousStatus");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemFlow_IdUserCreated",
                schema: "dbo",
                table: "WorkItemFlow",
                column: "IdUserCreated");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemFlow_IdWorkItem",
                schema: "dbo",
                table: "WorkItemFlow",
                column: "IdWorkItem");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemFlow_WorkItemId",
                schema: "dbo",
                table: "WorkItemFlow",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemLog_IdUserCreated",
                schema: "dbo",
                table: "WorkItemLog",
                column: "IdUserCreated");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemLog_IdWorkItem",
                schema: "dbo",
                table: "WorkItemLog",
                column: "IdWorkItem");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemLog_WorkItemId",
                schema: "dbo",
                table: "WorkItemLog",
                column: "WorkItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "security");

            migrationBuilder.DropTable(
                name: "WorkItemFlow",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WorkItemLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "security");

            migrationBuilder.DropTable(
                name: "Task",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StatusSystem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "security");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "security");
        }
    }
}
