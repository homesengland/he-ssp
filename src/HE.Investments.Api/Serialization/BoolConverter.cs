using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.Investments.Api.Serialization;

internal sealed class BoolConverter : JsonConverter<bool>
{
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) =>
        writer.WriteBooleanValue(value);

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.String => bool.TryParse(reader.GetString(), out var b) ? b : throw new JsonException("String could not be parsed to bool."),
            _ => throw new JsonException($"Unsupported bool token type: {reader.TokenType}"),
        };
}
