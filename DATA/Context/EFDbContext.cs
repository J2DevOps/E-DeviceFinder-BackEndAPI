using DATA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DATA.Context
{
    public class EFDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Item> Items { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }


        public EFDbContext(DbContextOptions<EFDbContext> Options) : base(Options)
        {

        }
    }
}
