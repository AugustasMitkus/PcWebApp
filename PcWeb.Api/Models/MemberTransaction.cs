using System;

namespace PcWeb.Api.Models;

//transaction type, how the money is split
public enum TransType
{
    EqualSplit,
    Percentage,
    Dynamic
}

public class MemberTransaction
{
    public int Id { get; set; }
    public TransType Type { get; set; }
    public int GroupId { get; set; }
    public MemberGroup Group { get; set; } = null!;
    public int SenderId { get; set; }
    public Member Sender { get; set; } = null!;
    public decimal Amount { get; set; } = 0;
    public DateTime SettledAt { get; set; } = DateTime.UtcNow;
}
