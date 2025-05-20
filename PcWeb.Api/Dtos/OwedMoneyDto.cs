namespace PcWeb.Api.Dtos;

public record class OwedMoneyDto(
    int MemberId,
    decimal Amount
);
