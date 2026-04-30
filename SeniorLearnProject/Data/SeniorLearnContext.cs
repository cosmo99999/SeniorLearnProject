using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Data
{
    public class SeniorLearnContext : IdentityDbContext
    {
        public  DbSet<Lesson> Lessons { get; set; }
        public  DbSet<Member> Members { get; set; }
        public  DbSet<Enrolment> Enrolments { get; set; }
        public  DbSet<DeliveryPlan> DeliveryPlans { get; set; }
        public  DbSet<Role> Roles { get; set; }
        public SeniorLearnContext(DbContextOptions<SeniorLearnContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
        protected override async void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
        }
        
    }
}
