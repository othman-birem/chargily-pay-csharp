﻿using System.Text.Json.Serialization;

namespace Chargily.Pay.V2.Internal.Responses;

internal record PriceResponse : BaseObjectResponse
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
    public List<object>? Metadata { get; init; } = new();
    [JsonPropertyName("product_id")]
    public string ProductId { get; init; }
}