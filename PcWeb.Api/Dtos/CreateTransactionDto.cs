using System.ComponentModel.DataAnnotations;
using PcWeb.Api.Models;

namespace PcWeb.Api.Dtos;

public record class CreateTransactionDto(
    [Required] TransType Type,
    [Required] int GroupId,
    [Required] int SenderId,
    decimal Amount,
    DateTime SettledAt);
