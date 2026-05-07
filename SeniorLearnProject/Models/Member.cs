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
        DeliveryPlans.Add(new DeliveryPlan(lessons, isCourse));
    }
    public void AddEnrolments(List<Lesson> lessons)
    {
        foreach (var  l in lessons)
        {
            Enrolments.Add(new Enrolment(this, l));
        }
    }
}