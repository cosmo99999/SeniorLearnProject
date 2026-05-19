using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using SeniorLearnProject.Models;
using SeniorLearnProject.Models.Identity;

namespace SeniorLearnProject.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(SeniorLearnContext context, UserManager<User> um)
        {
            var dataExists = context.Users.Any<User>();
                if (!dataExists)
                {
                    DataSeeder.SeedRoles(context);
                    await SeedUsers(um, context);
                }
        }
        public static void SeedRoles(SeniorLearnContext context)
        {
            context.Roles.Add(new Role(Role.Type.Admin));
            context.Roles.Add(new Role(Role.Type.Standard));
            context.Roles.Add(new Role(Role.Type.Professional));
            context.Roles.Add(new Role(Role.Type.Honorary));
            context.SaveChanges();
        }
        public static async Task SeedUsers(UserManager<User> um, SeniorLearnContext context)
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
            pmember.Email = "pmember@seniorlearn.com";
            pmember.UserName = "pmember@seniorlearn.com";
            await um.CreateAsync(pmember, "member");

            await um.AddToRoleAsync(admin, "admin");
            await um.AddToRoleAsync(smember, "standard");
            await um.AddToRoleAsync(pmember, "professional");

            var adminsRole = context.UserRoles.First(u => u.UserId == admin.Id);
            var smembersRole = context.UserRoles.First(u => u.UserId == smember.Id);
            var pmembersRole = context.UserRoles.First(u => u.UserId == pmember.Id);
            adminsRole.IsActive = true;
            smembersRole.IsActive = true;
            pmembersRole.IsActive = true;
            context.SaveChanges();
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
