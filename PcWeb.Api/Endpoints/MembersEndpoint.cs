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

        //GET members of a certain group
        g.MapGet("/group/{groupId}", (int groupId, GroupDbContext dbContext) =>
        {
            var members = dbContext.Members.Where(m => m.GroupId == groupId).Select(m => m.ToDto());

            return members is null ? Results.NotFound() : Results.Ok(members);
        });
        //GET a specific member
        g.MapGet("/{id}", (int id, GroupDbContext dbContext) =>
        {
            Member? member = dbContext.Members.Find(id);

            return member is null ? Results.NotFound() : Results.Ok(member.ToDto());
        })
        .WithName("GetMember");
        //POST a new member to the group
        g.MapPost("/", (CreateMemberDto newMember, GroupDbContext dbContext) =>
        {

            Member member = newMember.ToEntity();

            dbContext.Members.Add(member);
            dbContext.SaveChanges();


            return Results.CreatedAtRoute("GetMember", new { id = member.Id }, member.ToDto());
        })
        .WithParameterValidation();

        g.MapDelete("/{id}", (int id, GroupDbContext dbContext) =>
        {
            dbContext.Members.Where(member => member.Id == id).ExecuteDelete();

            return Results.NoContent();
        });
        
        return g;

    }
}
