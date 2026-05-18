using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;


public class Payment
{
    public int id {get; set;}
    public Member member {get; set;}
    public DateTime date {get; set;}
}
