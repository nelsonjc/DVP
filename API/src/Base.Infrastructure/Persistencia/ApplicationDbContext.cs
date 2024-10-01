using Microsoft.EntityFrameworkCore;
using TaskingSystem.Domain.Entities;
using System.Reflection;
using TaskingSystem.Infrastructure.Persistencia.EntityConfigurations;

namespace TaskingSystem.Infrastructure.Persistencia
{
    public class ApplicationDbContext : DbContext
    {
        private const string UserEntity = "USER";
        private const string TaskEntity = "TASK";

        // Constructor que acepta las opciones de configuración
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region security
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion

        #region dbo
        public DbSet<StatusSystem> StatusSystems { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemFlow> WorkItemFlows { get; set; }
        public DbSet<WorkItemLog> WorkItemLogs { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<WorkItem>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<WorkItemFlow>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<WorkItemLog>().Property(e => e.Id).ValueGeneratedNever();

            // Insertar datos de roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = DataConstant.ROL_ADMIN_ID,
                    Name = "Administrador",
                    Active = true,
                },
                new Role
                {
                    Id = DataConstant.ROL_SUPER_ID,
                    Name = "Supervisor",
                    Active = true,

                },
                new Role
                {
                    Id = DataConstant.ROL_EMPLO_ID,
                    Name = "Empleado",
                    Active = true,
                });

            // Insertar datos de usuarios
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = DataConstant.USER_ADMIN_ID,
                    FullName = "Usuario Administrador",
                    UserName = "admin",
                    Password = DataConstant.PASSWORD,
                    Active = true,
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdUserCreated = DataConstant.USER_ADMIN_ID
                },
                new User
                {
                    Id = DataConstant.USER_SUPER_ID,
                    FullName = "Usuario Supervidor",
                    UserName = "super",
                    Password = DataConstant.PASSWORD,
                    Active = true,
                    IdRole = DataConstant.ROL_SUPER_ID,
                    IdUserCreated = DataConstant.USER_ADMIN_ID
                },
                new User
                {
                    Id = DataConstant.USER_EMPLO_ID,
                    FullName = "Usuario Empleado",
                    UserName = "empleado",
                    Password = DataConstant.PASSWORD,
                    Active = true,
                    IdRole = DataConstant.ROL_EMPLO_ID,
                    IdUserCreated = DataConstant.USER_ADMIN_ID
                }
            );

            // Insertar datos de permisos
            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = DataConstant.PERMISSION_U_C_ID,
                    Entity = UserEntity,
                    ActionType = ActionTypeEnum.Create,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_U_R_ID,
                    Entity = UserEntity,
                    ActionType = ActionTypeEnum.Read,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_U_U_ID,
                    Entity = UserEntity,
                    ActionType = ActionTypeEnum.Update,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_U_D_ID,
                    Entity = UserEntity,
                    ActionType = ActionTypeEnum.Delete,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_T_C_ID,
                    Entity = TaskEntity,
                    ActionType = ActionTypeEnum.Create,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_T_R_ID,
                    Entity = TaskEntity,
                    ActionType = ActionTypeEnum.Read,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_T_U_ID,
                    Entity = TaskEntity,
                    ActionType = ActionTypeEnum.Update,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_T_D_ID,
                    Entity = TaskEntity,
                    ActionType = ActionTypeEnum.Delete,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_T_A_ID,
                    Entity = TaskEntity,
                    ActionType = ActionTypeEnum.Asigne,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                },
                new Permission
                {
                    Id = DataConstant.PERMISSION_T_CS_ID,
                    Entity = TaskEntity,
                    ActionType = ActionTypeEnum.ChangeStatus,
                    IdUserCreated = DataConstant.USER_ADMIN_ID,
                }
            );

            // Insertar datos de roles por permisos
            modelBuilder.Entity<RolePermission>().HasData(

                #region Permisos Rol Administrador

                // ○ Administrador: Puede crear usuarios.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_U_C_ID
                },
                // ○ Administrador: Puede ver usuarios.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_U_R_ID
                },
                // ○ Administrador: Puede editar.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_U_U_ID
                },
                // ○ Administrador: Puede eliminar usuarios.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_U_D_ID
                },
                // ○ Administrador: Puede crear tareas.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_T_C_ID
                },
                // ○ Administrador: Puede ver tareas.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_T_R_ID
                },
                // ○ Administrador: Puede tareas.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_T_U_ID
                },
                // ○ Administrador: Puede eliminar tareas.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_ADMIN_ID,
                    IdPermission = DataConstant.PERMISSION_T_D_ID
                },
                #endregion

                #region Permisos Rol Supervisor
                // ○ Supervisor: Puede asignar tareas a los empleados y cambiar el estado de las tareas.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_SUPER_ID,
                    IdPermission = DataConstant.PERMISSION_T_A_ID
                },
                new RolePermission
                {
                    IdRole = DataConstant.ROL_SUPER_ID,
                    IdPermission = DataConstant.PERMISSION_T_CS_ID
                },
                #endregion

                #region Permisos Rol Empleado

                //○ Empleado: Puede ver sus tareas asignadas y actualizar el estado de las mismas.
                new RolePermission
                {
                    IdRole = DataConstant.ROL_EMPLO_ID,
                    IdPermission = DataConstant.PERMISSION_T_R_ID
                },
                new RolePermission
                {
                    IdRole = DataConstant.ROL_EMPLO_ID,
                    IdPermission = DataConstant.PERMISSION_T_CS_ID
                }
                #endregion
            );

            // Insertar datos de estados
            modelBuilder.Entity<StatusSystem>().HasData(
                new StatusSystem
                {
                    Id = DataConstant.STATUS_PENDING_ID,
                    Entity = "TASK",
                    Code = "PENDING",
                    Name = "Pendiente",
                    Active = true,
                    IdUserCreated = DataConstant.USER_ADMIN_ID
                },
                new StatusSystem
                {
                    Id = DataConstant.STATUS_IN_PROCESS_ID,
                    Entity = "TASK",
                    Code = "IN_PROCESS",
                    Name = "En Proceso",
                    Active = true,
                    IdUserCreated = DataConstant.USER_ADMIN_ID
                },
                new StatusSystem
                {
                    Id = DataConstant.STATUS_COMPLETED_ID,
                    Entity = "TASK",
                    Code = "COMPLETED",
                    Name = "Completada",
                    Active = true,
                    IdUserCreated = DataConstant.USER_ADMIN_ID
                }
            );
        }
    }
}
