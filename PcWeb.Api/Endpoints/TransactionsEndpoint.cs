using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PcWeb.Api.Data;
using PcWeb.Api.Dtos;
using PcWeb.Api.Mapping;
using PcWeb.Api.Models;

namespace PcWeb.Api.Endpoints;

public static class TransactionsEndpoint
{
    public static RouteGroupBuilder MapTransactionsEndpoints(this WebApplication app)
    {
        var g = app.MapGroup("transactions");
        //GET transaction of a specific group
        g.MapGet("/group/{groupId}", (int groupId, GroupDbContext dbContext) =>
        {
            var transactions = dbContext.Transactions.Where(m => m.GroupId == groupId).Include(t => t.Sender)
                .Include(t => t.Group).Select(m => m.ToDto()).ToList();

            return transactions.Count == 0 ? Results.NotFound() : Results.Ok(transactions);
        });
        //POST a new transaction
        g.MapPost("/", (CreateTransactionDto newTransaction, GroupDbContext dbContext) =>
        {
            MemberTransaction transaction = newTransaction.ToEntity();
            dbContext.Transactions.Add(transaction);
            dbContext.SaveChanges();
            return Results.Ok();
        }


        );
        return g;
    }
}
