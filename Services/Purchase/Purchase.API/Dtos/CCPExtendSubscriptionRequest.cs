using BuildingBlocks.Converters;
using System.Text.Json.Serialization;

namespace Purchase.API.Dtos;

public record CCPExtendSubscriptionRequest(
    Guid SubscriptionId,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo,
    int ExtensionPeriodInMonths);
