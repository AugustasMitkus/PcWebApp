namespace PcWeb.Api.Dtos;

public record class MemberDto(
    int Id,
    string FirstName,
    string LastName,
    int GroupId
);
