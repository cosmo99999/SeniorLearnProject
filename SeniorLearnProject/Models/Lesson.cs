using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;

public class Lesson
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Title { get; set; }
    public DeliveryPlan DeliveryPlan { get; set; }

    public Lesson(){}
    public Lesson(DateTime start, DateTime end, string title)
    {
        Start = start;
        End = end;
        Title = title;
    }

    public bool HasOverlap(Lesson l)
    {
        if (l.Start < this.End && l.End > this.Start)
        {
            return true;
        }
        return false;
    }

}
