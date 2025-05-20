using System.Formats.Asn1;
using Microsoft.OpenApi.Writers;

namespace PcWeb.Api.Dtos;

public record class CreateDebtDto(
    int GroupId
);