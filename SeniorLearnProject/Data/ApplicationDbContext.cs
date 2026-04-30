using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public  DbSet<Lesson> Lessons{ get; set; }
        public  DbSet<Member> Members{ get; set; }
        public  DbSet<Enrolment> Enrolments { get; set; }
        public  DbSet<DeliveryPlan> DeliveryPlans { get; set; }
        public  DbSet<Role> Roles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.Entity<Lesson>().ToTable("Lessons");
            mb.Entity<Member>().ToTable("Members");
            mb.Entity<Enrolment>().ToTable("Enrolments");
            mb.Entity<DeliveryPlan>().ToTable("DeliveryPlans");
            mb.Entity<Role>().ToTable("Roles");


        }
        
        public void 
    }
}
