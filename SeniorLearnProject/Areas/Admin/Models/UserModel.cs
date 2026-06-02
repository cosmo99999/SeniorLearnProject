using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SeniorLearnProject.Models.Identity;

namespace SeniorLearnProject.Areas.Admin.Models;
public class UserModel
{
    public string Id { get; set; } = default!;
    public int? MemberId { get; set; }
    [Required(ErrorMessage = "First name is required")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be less than 30 character and greater than 2")]
    public string FirstName {get;set;} = default!;
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be less than 30 character and greater than 2")]
    public string LastName {get;set;} = default!;
    public string Email {get;set;} = default!;
    public DateTime PaidUntil { get; set; }  = DateTime.Now;
    public bool[] RoleBools { get; set; }
    public string[] RoleStrings { get; set; } 
}
