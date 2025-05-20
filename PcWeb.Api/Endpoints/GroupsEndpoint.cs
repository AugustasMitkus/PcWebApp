using System;
using System.Text.RegularExpressions;
using PcWeb.Api.Data;
using PcWeb.Api.Dtos;
using PcWeb.Api.Models;
using PcWeb.Api.Mapping;

namespace PcWeb.Api.Endpoints;

public static class GroupsEndpoint
{

    public static RouteGroupBuilder MapGroupsEndpoints(this WebApplication app)
    {

        var g = app.MapGroup("groups");

        g.MapGet("/", (GroupDbContext dbContext) =>
            dbContext.MemberGroups.Select(memberGroup => memberGroup.ToDto())
        );

        g.MapGet("/{id}", (int id, GroupDbContext dbContext) =>
        {
            MemberGroup? memberGroup = dbContext.MemberGroups.Find(id);

            return memberGroup is null ? Results.NotFound() : Results.Ok(memberGroup.ToDto());
        })
        .WithName("GetGroup");

        g.MapPost("/", (CreateMemberGroupDto newMemberGroup, GroupDbContext dbContext) =>
        {

            MemberGroup memberGroup = newMemberGroup.ToEntity();

            dbContext.MemberGroups.Add(memberGroup);
            dbContext.SaveChanges();


            return Results.CreatedAtRoute("GetGroup", new { id = memberGroup.Id }, memberGroup.ToDto());
        })
        .WithParameterValidation();


        return g;
    }
}
