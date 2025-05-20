using System;
using PcWeb.Api.Dtos;
using PcWeb.Api.Models;

namespace PcWeb.Api.Mapping;

public static class GroupMapping
{
    public static MemberGroup ToEntity(this CreateMemberGroupDto memberGroup)
    {
        return new MemberGroup()
        {
            Name = memberGroup.Name
        };
    }

    public static MemberGroupDto ToDto(this MemberGroup memberGroup)
    {
        return new(
            memberGroup.Id,
            memberGroup.Name
        );
    }
}
