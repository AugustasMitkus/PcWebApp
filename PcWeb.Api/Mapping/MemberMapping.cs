using System;
using PcWeb.Api.Dtos;
using PcWeb.Api.Endpoints;
using PcWeb.Api.Models;

namespace PcWeb.Api.Mapping;

public static class MemberMapping
{
    public static Member ToEntity(this CreateMemberDto member)
    {
        return new Member()
        {
            FirstName = member.FirstName,
            LastName = member.LastName,
            GroupId = member.GroupId
        };
    }

    public static MemberDto ToDto(this Member member, List<OwedMoney> debts)
    {

        var memberId = member.Id;

        var relevantDebts = debts.Where(d => d.FromId == memberId || d.ToId == memberId);
        decimal totalDebt;
        if (!relevantDebts.Any())
            totalDebt = 0;
        else
        {
            totalDebt = relevantDebts.Sum(d =>
                d.FromId == memberId ? d.Amount : -d.Amount
            );
        }
        return new(
            member.Id,
            member.FirstName,
            member.LastName,
            totalDebt
        );
    }
    public static List<OwedMoneyDto> ToIndebted(this Member member, List<OwedMoney> debts, List<Member> members)
    {
        //a list of debts that include the current member
        //usually the entry is written from the FirstId member perspective (+Amount how much they're owed, -Amount how much they owe)
        var owes = debts.Where(x => x.FromId == member.Id || x.ToId == member.Id)
        .Select(x =>
        {
            bool isFirst = x.FromId == member.Id;
            int otherId = isFirst ? x.ToId : x.FromId;
            var otherMember = members.FirstOrDefault(m => m.Id == otherId);
            string otherFirstName = otherMember?.FirstName ?? "Unknown";
            string otherLastName = otherMember?.LastName ?? "Unknown";
            if (!isFirst)
            {
                return new OwedMoneyDto(x.FromId, otherFirstName, otherLastName, -x.Amount); //the member with the SecondId will have the opposite debt amount shown
            }
            else
            {
                return new OwedMoneyDto(x.ToId, otherFirstName, otherLastName, x.Amount);
            }
        }).ToList();

        
        return owes;
    }
}
