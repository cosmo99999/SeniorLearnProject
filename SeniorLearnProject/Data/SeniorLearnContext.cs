using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Models;
using SeniorLearnProject.Models.Identity;

namespace SeniorLearnProject.Data
{
    public class SeniorLearnContext : IdentityDbContext<User, Role, string, 
                                      IdentityUserClaim<string>, UserRole, 
                                      IdentityUserLogin<string>, IdentityRoleClaim<string>, 
                                      IdentityUserToken<string>>
    {
        public  DbSet<Lesson> Lessons { get; set; }
        public  DbSet<Member> Members { get; set; }
        public  DbSet<Enrolment> Enrolments { get; set; }
        public  DbSet<DeliveryPlan> DeliveryPlans { get; set; }
        public SeniorLearnContext(DbContextOptions<SeniorLearnContext> options)
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
            
        }
        
    }
}
