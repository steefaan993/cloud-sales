using BuildingBlocks.Converters;
using System.Text.Json.Serialization;

namespace Customer.Application.Dtos;

public record SoftwareLicenseData(
    string Vendor,
    string SoftwareName,
    int Quantity,
    string State,
    Guid ReferenceId,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo);
