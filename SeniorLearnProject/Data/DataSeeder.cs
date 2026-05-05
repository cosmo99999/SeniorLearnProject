using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Models;

namespace SeniorLearnProject.Data
{
    public static class DataSeeder
    {
        public static void SeedData(SeniorLearnContext context)
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

            var sMember = new Member();
            var pMember = new Member();
            sMember.AddRoleWithType(RoleType.Standard);
            pMember.AddRoleWithType(RoleType.Professional);
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
