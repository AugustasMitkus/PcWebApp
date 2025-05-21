using System;
using PcWeb.Api.Dtos;
using PcWeb.Api.Models;

namespace PcWeb.Api.Mapping;

public static class DebtMapping
{
    public static List<OwedMoney> ToDebtList(this CreateDebtDto debt, int firstId, List<Member> members, List<OwedMoney> debts)
    {
        var newDebts = new List<OwedMoney>();

        foreach (var member in members)
        {
            if (member.Id == firstId) continue;

            //checks for any debt entries that may exist between the newly created member and any of other members
            var existingDebt = debts.FirstOrDefault(x =>
            (x.FromId == firstId && x.ToId == member.Id) ||
            (x.FromId == member.Id && x.ToId == firstId));

            if (existingDebt == null)
            {
                newDebts.Add(new OwedMoney
                {
                    FromId = firstId,
                    ToId = member.Id,
                });
            }
        }

        return newDebts;
    }

    public static List<OwedMoney> ToUpdatedList(this UpdateDebtDto debt, int id, List<Member> members, List<OwedMoney> debts)
    {
        var updatedDebts = new List<OwedMoney>();

        foreach (var member in members)
        {
            if (member.Id == id) continue;

            var existingDebt = debts.FirstOrDefault(x =>
            (x.FromId == id && x.ToId == member.Id) ||
            (x.FromId == member.Id && x.ToId == id));

            if (existingDebt != null)
            {
                //based on the type of transaction, calculates the money amount sent to a specific member
                decimal payment = debt.Type switch
                {
                    TransType.EqualSplit => debt.Amount / (members.Count - 1),
                    TransType.Percentage => debt.Distribution!
                    .TryGetValue(member.Id, out var percent) ? debt.Amount * (percent / 100m) : 0,
                    TransType.Dynamic => debt.Distribution!
                    .TryGetValue(member.Id, out var amount) ? amount : 0,
                    _ => 0
                };
                //if no money is sent to the member
                if (payment == 0) continue;

                if (id == existingDebt.FromId)
                    existingDebt.Amount += payment;
                else
                    existingDebt.Amount -= payment;
                updatedDebts.Add(existingDebt);
            }
        }

        return updatedDebts;
    }

}
