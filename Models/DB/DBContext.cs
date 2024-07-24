using hospitalBackend.Models.DB.tables;
using hospitalBackend.Models.DB.viewes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hospitalBackend.Models.DB
{
    public class DBContext:IdentityDbContext
    {
        public DBContext(DbContextOptions options):base(options){}

        public DbSet<Department_tbl> Department_Tbl {  get; set; }
        public DbSet<Patients_tbl> Patients_tbl { get; set; }
        public DbSet<VisitTime_tbl> visitTime_Tbl { get; set; }
        public DbSet<Visit_tbl> visit_Tbls { get; set; }
        public DbSet<visit_view> visit_View { get; set; }
        public DbSet<ContactUsMsg_tbl> contactUsMsg_Tbl { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<visit_view>().ToView("visit-view");
            base.OnModelCreating(builder);
        }
    }
}
