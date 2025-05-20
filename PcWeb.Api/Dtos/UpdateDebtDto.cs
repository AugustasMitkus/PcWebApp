using PcWeb.Api.Models;

namespace PcWeb.Api.Dtos;

public record class UpdateDebtDto(
    TransType Type,
    Dictionary<int, decimal>? Distribution,
    decimal Amount = 0
);