namespace PcWeb.Api.Dtos;

public record class IndebtMemberDto(
    int Id,
    string FirstName,
    string LastName,
    int GroupId,
    List<OwedMoneyDto> Debt
);
