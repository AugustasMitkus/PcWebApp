using System;
using System.Text.RegularExpressions;
using PcWeb.Api.Data;
using PcWeb.Api.Dtos;
using PcWeb.Api.Mapping;
using PcWeb.Api.Models;

namespace PcWeb.Api.Endpoints;

public static class DebtsEndpoint
{
    public static RouteGroupBuilder MapDebtsEndpoints(this WebApplication app)
    {
        var g = app.MapGroup("debts");

        //GET debts that a specific user has with the rest of the members
        g.MapGet("/{id}", (int id, GroupDbContext dbContext) =>
        {
            Member? member = dbContext.Members.Find(id);
            var debts = dbContext.OwedMoney.Where(x => x.FromId == id || x.ToId == id).ToList();
            return member is null ? Results.NotFound() : Results.Ok(member.ToIndebtedDto(debts)); //
        }).WithName("GetDebts");
        
        //POST debts of a member with every single other member
        g.MapPost("/{id}", (int id, CreateDebtDto newDebt, GroupDbContext dbContext) =>
        {
            Member? member = dbContext.Members.Find(id);
            if (member is null) return Results.NotFound();

            List<Member> members = [.. dbContext.Members.Where(x => x.GroupId == member.GroupId)];
            List<OwedMoney> debts = [.. dbContext.OwedMoney.Where(x => x.FromId == id || x.ToId == id)];

            List<OwedMoney> newDebts = newDebt.ToDebtList(id,members, debts);

            dbContext.OwedMoney.UpdateRange(newDebts);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute("GetDebts", new { id = member.Id }, newDebts.ToDto());
        });
        //Update each instance of debt with another member that a specific member has (done after making a transaction)
        g.MapPut("/{id}", (int id, UpdateDebtDto updatedDebt, GroupDbContext dbContext) =>
        {
            var member = dbContext.Members.Find(id);
            if (member is null) return Results.NotFound();
            List<Member> members = [.. dbContext.Members.Where(x => x.GroupId == member.GroupId)];

            List<OwedMoney> debts = [.. dbContext.OwedMoney.Where(x => x.FromId == id || x.ToId == id)];

            var updatedDebts = updatedDebt.ToUpdatedList(id,members, debts);

            dbContext.OwedMoney.UpdateRange(updatedDebts);
            dbContext.SaveChanges();

            return Results.Ok("Updated the debts.");
        });

        return g;
    }
}
