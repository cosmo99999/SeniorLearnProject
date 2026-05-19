using System;

namespace SeniorLearnProject.Areas.Admin.Models;

public class UserModel
{
    public string FirstName {get;set;} = default!;
    public string LastName {get;set;} = default!;
    public string Email {get;set;} = default!;
    public DateTime paidUntil = DateTime.Now;
    public UserModel()
    {
        
    }
}
