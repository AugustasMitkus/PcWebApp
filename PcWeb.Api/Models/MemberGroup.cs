using System;

namespace PcWeb.Api.Models;

public class MemberGroup
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Member> Members { get; set; } = [];
}
