using System;
using Microsoft.EntityFrameworkCore;
using PcWeb.Api.Data;
using PcWeb.Api.Dtos;
using PcWeb.Api.Mapping;
using PcWeb.Api.Models;

namespace PcWeb.Api.Endpoints;

public static class MembersEndpoint
{
    public static RouteGroupBuilder MapMembersEndpoints(this WebApplication app)
    {
        var g = app.MapGroup("members");

        //GET members of a certain group with the sum of debts
        g.MapGet("/group/{groupId}", (int groupId, GroupDbContext dbContext) =>
        {
            var debts = dbContext.OwedMoney.ToList();
            var members = dbContext.Members.Where(m => m.GroupId == groupId).Select(m => m.ToDto(debts));

            return members is null ? Results.NotFound() : Results.Ok(members);
        });
        //GET a specific member and the sum of the debt that he has with others
        g.MapGet("/{id}", (int id, GroupDbContext dbContext) =>
        {
            Member? member = dbContext.Members.Find(id);
            var debts = dbContext.OwedMoney.ToList();
            return member is null ? Results.NotFound() : Results.Ok(member.ToDto(debts));
        })
        .WithName("GetMember");
        //POST a new member to the group
        g.MapPost("/", (CreateMemberDto newMember, GroupDbContext dbContext) =>
        {

            Member member = newMember.ToEntity();

            dbContext.Members.Add(member);
            dbContext.SaveChanges();


            return Results.Ok(member.Id);
        })
        .WithParameterValidation();

        g.MapDelete("/{id}", (int id, GroupDbContext dbContext) =>
        {
            var debts = dbContext.OwedMoney.Where(debt => debt.FromId == id || debt.ToId == id).ToList();
            dbContext.OwedMoney.RemoveRange(debts);
            var member = dbContext.Members.Find(id);
            if (member != null)
            {
                dbContext.Members.Remove(member);
            }
            dbContext.SaveChanges();
            return Results.NoContent();
        });
        
        return g;

    }
}
