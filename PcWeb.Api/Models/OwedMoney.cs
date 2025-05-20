using System;

namespace PcWeb.Api.Models;

public class OwedMoney
{
    public int Id { get; set; }
    public int FromId { get; set; }
    public Member FirstMember { get; set; } = null!;
    public int ToId { get; set; }
    public Member SecondMember { get; set; } = null!;
    public decimal Amount { get; set; } = 0;
}
