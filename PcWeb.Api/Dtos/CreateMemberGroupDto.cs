using System.ComponentModel.DataAnnotations;

namespace PcWeb.Api.Dtos;

public record class CreateMemberGroupDto(
    [Required][StringLength(20)] string Name);
