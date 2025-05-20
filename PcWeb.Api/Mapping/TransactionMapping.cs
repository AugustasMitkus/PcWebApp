using System;
using System.Transactions;
using PcWeb.Api.Dtos;
using PcWeb.Api.Models;

namespace PcWeb.Api.Mapping;

public static class TransactionMapping
{
    public static MemberTransaction ToEntity(this CreateTransactionDto transaction)
    {
        return new MemberTransaction()
        {
            Type = transaction.Type,
            GroupId = transaction.GroupId,
            SenderId = transaction.SenderId,
            Amount = transaction.Amount
        };
    }

    public static TransactionDto ToDto(this MemberTransaction transaction)
    {

        return new(
            transaction.Id,
            transaction.Type.ToString(),
            transaction.GroupId,
            transaction.Sender.FirstName,
            transaction.Sender.LastName,
            transaction.Amount,
            transaction.SettledAt
        );
    }
}
