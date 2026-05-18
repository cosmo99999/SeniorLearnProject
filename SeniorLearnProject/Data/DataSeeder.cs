using Microsoft.AspNetCore.Identity;
using SeniorLearnProject.Models;
using SeniorLearnProject.Models.Identity;

namespace SeniorLearnProject.Data
{
    public static class DataSeeder
    {
        public static void SeedRoles(SeniorLearnContext context)
        {
            context.Roles.Add(new Role(Role.Type.Admin));
            context.Roles.Add(new Role(Role.Type.Standard));
            context.Roles.Add(new Role(Role.Type.Professional));
            context.Roles.Add(new Role(Role.Type.Honorary));
            context.SaveChanges();
        }
        public static async Task SeedUsers(UserManager<User> um)
        {
            User admin = new User();
            admin.Email = "admin@seniorlearn.com";
            admin.UserName = "admin@seniorlearn.com";
            await um.CreateAsync(admin, "admin");
            User smember = new User();
            smember.Email = "smember@seniorlearn.com";
            smember.UserName = "smember@seniorlearn.com";
            await um.CreateAsync(smember, "member");
            User pmember = new User();
            pmember.Email = "smember@seniorlearn.com";
            pmember.UserName = "smember@seniorlearn.com";
            await um.CreateAsync(pmember, "member");
        }
        public static void SeedMembersAndLessons(SeniorLearnContext context)
        {
            List<Lesson> lessons1 = new List<Lesson>();
            List<Lesson> lessons2 = new List<Lesson>();

            for (int i = 0; i < 5; i++)
            {
                lessons1.Add(new Lesson
                (
                    $"Lesson {i}",
                    new DateTime(2022, 2, 1 + i),
                    new DateTime(2022, 2, 2 + i)
                ));
                lessons2.Add(new Lesson
                (
                    $"Lesson {i + 5}",
                    new DateTime(2027, 2, 1 + i),
                    new DateTime(2027, 2, 2 + i)
                ));
            }

            var sMember = new Member("rory", "coleman");
            var pMember = new Member("cory", "roleman");
            context.Members.Add(sMember);
            context.Members.Add(pMember);

            sMember.AddEnrolments(lessons1);
            pMember.AddDeliveryPlan(lessons1, true);
            sMember.AddEnrolments(lessons2);
            pMember.AddDeliveryPlan(lessons2, false);

            context.Members.Add(sMember);
            context.Members.Add(pMember);
            context.SaveChanges();
        }
    }
}
