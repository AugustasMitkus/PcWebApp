using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PcWeb.Api.Dtos;

public record class CreateMemberDto(
    [Required][StringLength(20)] string FirstName,
    [Required][StringLength(20)] string LastName,
    [Required] int GroupId
);
