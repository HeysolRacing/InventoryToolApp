using InventoryTool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InventoryTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<
                    Models.InventoryToolContext,
                    Migrations.Configuration>()); // Para que siempre busque la ultima version
            ApplicationDbContext db = new ApplicationDbContext(); // para conectar a la base de datos del membership
            CreateRoles(db);  // Creamos el metodo de Creacion de Roles
            CreateAdmin(db);  // Creamos el metodo de Creacion del Admnistrador
            AddPermissionsToAdmin(db); // Creamos el metodo de Permisos del Admnistrador
            db.Dispose();     // Liberamos el objeto y cerramos la base de datos
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermissionsToAdmin(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var user = userManager.FindByName("hsagaon@elementcorp.com");

            if (!userManager.IsInRole(user.Id, "InventoryView")) // Agregar Permisos
                userManager.AddToRole(user.Id, "InventoryView");

            if (!userManager.IsInRole(user.Id, "InventoryEdit")) 
                userManager.AddToRole(user.Id, "InventoryEdit");

            if (!userManager.IsInRole(user.Id, "InventoryCreate")) 
                userManager.AddToRole(user.Id, "InventoryCreate");

            if (!userManager.IsInRole(user.Id, "InventoryDelete")) 
                userManager.AddToRole(user.Id, "InventoryDelete");

            if (!userManager.IsInRole(user.Id, "Administrator"))
                userManager.AddToRole(user.Id, "Administrator");
        }

        private void CreateAdmin(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.FindByName("hsagaon@elementcorp.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "hsagaon@elementcorp.com",
                    Email= "hsagaon@elementcorp.com"
                };
                userManager.Create(user, "Happy@123");
            }

        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            // Se crean los roles
            if (!roleManager.RoleExists("Administrator"))
            {
                roleManager.Create(new IdentityRole("Administrator"));
            }
            if (!roleManager.RoleExists("InventoryView"))
            {
                roleManager.Create(new IdentityRole("InventoryView"));
            }
            if (!roleManager.RoleExists("InventoryEdit"))
            {
                roleManager.Create(new IdentityRole("InventoryEdit"));
            }
            if (!roleManager.RoleExists("InventoryCreate"))
            {
                roleManager.Create(new IdentityRole("InventoryCreate"));
            }
            if (!roleManager.RoleExists("InventoryDelete"))
            {
                roleManager.Create(new IdentityRole("InventoryDelete"));
            }
        }
    }
}
