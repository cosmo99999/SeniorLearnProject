namespace SeniorLearnProject.Models;


public class Enrolment
{
    public int Id { get; set; }
    public Member Member { get; set; }
    public Lesson Lesson { get; set; }

    public Enrolment(){}
    public Enrolment(Member member, Lesson lesson)
    {
        Member = member;
        Lesson = lesson;
    }

    public bool HasOverlap(Lesson l)
    {
        return Lesson.HasOverlap(l);
    }

}
