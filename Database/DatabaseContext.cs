using CREDMicroService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CREDMicroService.Database
{
    public class DatabaseContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=DESKTOP-8ENM95I\SqlExpress;initial catalog=MicroServiceDB;Integrated Security=SSPI;");
            //optionsBuilder.UseSqlServer(@"Server=tcp:azsqlserverdatabase.database.windows.net,1433;Initial Catalog=microservicedb;Persist Security Info=False;User ID=azsa;Password=TcsP@ssw0rd@4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        }

    }
}
