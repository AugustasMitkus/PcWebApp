using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PcWeb.Api.Models;

public class Member
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int GroupId { get; set; }
    public MemberGroup? MemberGroup { get; set; }
}