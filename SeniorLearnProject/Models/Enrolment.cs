using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;


public class Enrolment
{
    public int Id { get; set; }
    public List<Lesson> Lessons { get; set; }

    private Enrolment(){}
    public Enrolment(List<Lesson> lessons)
    {
        Lessons = lessons;
    }
}
