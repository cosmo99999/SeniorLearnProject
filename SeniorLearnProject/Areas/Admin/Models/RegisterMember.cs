using System;

namespace SeniorLearnProject.Areas.Admin.Models;

public class RegisterMember
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string Email {get;set;}
    public DateTime paidUntil = DateTime.Now;

    //public
    
}
