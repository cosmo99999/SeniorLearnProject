using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;


public class Enrolment
{
    public int Id { get; set; }
    public Member Member { get; set; }
    public Lesson Lesson { get; set; }

    private Enrolment(){}
    public Enrolment(Member member, Lesson lesson)
    {
        Member = member;
        Lesson = lesson;
    }
}
