namespace PcWeb.Api.Dtos;

public record class TransactionDto(
    int Id,
    string Type,
    int GroupId,
    string FirstName,
    string LastName,
    decimal Amount,
    DateTime SettledAt
);
