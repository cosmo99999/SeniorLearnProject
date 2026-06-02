using SeniorLearnProject.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;


public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime paidUntil { get; set; } = DateTime.Now;
    public User User { get; set; } 
    public List<Enrolment> Enrolments { get; set; } = new();
    public List<DeliveryPlan> DeliveryPlans { get; set; } = new();

    public Member()
    {
        
    }
    public Member(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void AddDeliveryPlan(List<Lesson> lessons, bool isCourse)
    {
        var dp = new DeliveryPlan(lessons, isCourse)
        {
            //Member = this,
            MemberId = this.Id
        };

        // set back-reference on lessons
        if (dp.Lessons is not null)
        {
            foreach (var l in dp.Lessons)
            {
                l.DeliveryPlan = dp;
            }
        }

        DeliveryPlans.Add(dp);
    }
    public void AddEnrolments(List<Lesson> lessons)
    {
        foreach (var  l in lessons)
        {
            Enrolments.Add(new Enrolment(this, l));
        }
    }
}