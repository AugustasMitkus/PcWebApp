namespace PcWeb.Api.Dtos;

public record class OwedMoneyDto(
    int Id,
    string FirstName,
    string LastName,
    decimal DebtSum
);
