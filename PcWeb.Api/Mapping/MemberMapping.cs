using System;
using PcWeb.Api.Dtos;
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

    public static MemberDto ToDto(this Member member)
    {
        return new(
            member.Id,
            member.FirstName,
            member.LastName,
            member.GroupId
        );
    }
    public static IndebtMemberDto ToIndebtedDto(this Member member, List<OwedMoney> debts)
    {
        //a list of debts that include the current member
        
        //usually the entry is written from the FirstId member perspective (+Amount how much they're owed, -Amount how much they owe)
        var owes = debts.Where(x => x.FromId == member.Id || x.ToId == member.Id)
        .Select(x =>
        {
            bool isFirst = x.FromId == member.Id;
            if (!isFirst)
                return new OwedMoneyDto(x.FromId, -x.Amount); //the member with the SecondId will have the opposite debt amount shown
            else
                return new OwedMoneyDto(x.ToId, x.Amount);
        }).ToList();

        
        return new(
                member.Id,
                member.FirstName,
                member.LastName,
                member.GroupId,
                owes
            );
    }
}
