using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;

public class DeliveryPlan
{
    public int Id { get; set; }
    public bool IsCourse { get; set; }
    public List<Lesson> Lessons { get; private set; } = new();
    public int? MemberId { get; set; }

    public DeliveryPlan(){}
    public DeliveryPlan(List<Lesson> lessons, bool isCourse)
    {
        Lessons = lessons;
        IsCourse = isCourse;
    }
}
