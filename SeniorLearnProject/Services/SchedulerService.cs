using Microsoft.EntityFrameworkCore;
using SeniorLearnProject.Data;
using SeniorLearnProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Services;

public class SchedulerService
{
    private SeniorLearnContext _context;
    public SchedulerService(SeniorLearnContext context)
    {
        _context = context;
       
    }

    public void AddDeliveryPlan(List<Lesson> lessoons, Member m, bool isCourse)
    {
        m.AddDeliveryPlan(lessoons, isCourse);
    }

    public async Task<bool> DoConflictingLessonsExist(Lesson lesson)
    {
        string start = lesson.Start.ToString();
        string end = lesson.End.ToString();
        bool result = await _context.Lessons.FromSql(
        $"""
            Select * FROM [Lessons] WHERE EXISTS (SELECT 1 WHERE 
             Start < {end} AND End > {start} )
        """
        ).AnyAsync();
        return result;
    }
    public async Task<List<Lesson>> GetFutureLessons()
    {
        var result = await _context.Lessons.FromSql(
        $"""
            SELECT * FROM [Lessons] WHERE [Lessons].[End] > GETDATE()
        """
        ).ToListAsync();


        Console.WriteLine(result);
        return result;
    }

}
