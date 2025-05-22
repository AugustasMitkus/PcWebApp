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
            if (member == null) return Results.NotFound();
            var debts = dbContext.OwedMoney.Where(x => x.FromId == id || x.ToId == id).ToList();
            var members = dbContext.Members.Where(x => x.GroupId == member.GroupId).ToList();
            return Results.Ok(member.ToIndebted(debts, members));
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

            return Results.Ok("Debts have been posted");
        });
        //Update each instance of debt with another member that a specific member has (done after making a transaction)
        g.MapPut("/{id}", (int id, UpdateDebtDto updatedDebt, GroupDbContext dbContext) =>
        {
            if ((int)updatedDebt.Type == 1)
            {
                decimal total = updatedDebt.Distribution.Values.Sum();
                if (Math.Abs(total - 100m) > 0.01m)
        {
            return Results.BadRequest($"Percentages must sum to 100%.");
        }
            }
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
