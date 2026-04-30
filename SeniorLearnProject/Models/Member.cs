using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorLearnProject.Models;


public class Member
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<Enrolment> Enrolments { get; set; }
    public List<DeliveryPlan> DeliveryPlans { get; set; }
    public List<Role> Roles { get; set; }

    public Member(int id)
    {
        Id = id;
    }

    public void AddDeliveryPlan(List<Lesson> lessons, bool isCourse)
    {
        DeliveryPlans.Add(new DeliveryPlan(lessons, isCourse));
    }
    void AssignRole(RoleType roleType)
    {
        var role = FindRoleForType(roleType);
        if (role == null)
        {
            AddRoleWithType(roleType);
        }
        else
        {
            ActivateRole(role);
        }
    }
    void ActivateRole(Role role)
    {

    }
    void AddRoleWithType(RoleType roleType)
    {
        Roles.Add(new Role(roleType));
    }
    private bool IsProfesional()
    {
        foreach (Role r in Roles)
        {
            if (r.RoleType == RoleType.Professional)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsStandard()
    {
        foreach (Role r in Roles)
        {
            if (r.RoleType == RoleType.Standard)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsHonorary()
    {
        foreach (Role r in Roles)
        {
            if (r.RoleType == RoleType.Honorary)
            {
                return true;
            }
        }
        return false;
    }
    Role FindRoleForType(RoleType roleType)
    {
        foreach (Role r in Roles)
        {
            if (r.RoleType == roleType)
            {
                return r;
            }
        }
        return null;
    }
}