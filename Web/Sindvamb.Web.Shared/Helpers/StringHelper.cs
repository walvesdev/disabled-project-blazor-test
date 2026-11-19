using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sindvamb.Web.Shared.Helpers
{
    public class StringHelper : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // deserialize numbers as strings.
            //switch (reader.TokenType)
            //{
            //    case JsonTokenType.Number:
            //        return reader.GetInt32().ToString();
            //    case JsonTokenType.String:
            //        return reader.GetString();
            //    case JsonTokenType.None:
            //        return reader.GetString();
            //    case JsonTokenType.StartObject:
            //        return reader.GetString();
            //    case JsonTokenType.EndObject:
            //        return reader.GetString();
            //    case JsonTokenType.StartArray:
            //        return reader.GetString();
            //    case JsonTokenType.EndArray:
            //        return reader.GetString();
            //    case JsonTokenType.PropertyName:
            //        return reader.GetString();
            //    case JsonTokenType.Comment:
            //        return reader.GetString();
            //    case JsonTokenType.True:
            //        return reader.GetString();
            //    case JsonTokenType.False:
            //        return reader.GetString();
            //    case JsonTokenType.Null:
            //        return reader.GetString();
            //    default:
            //        throw new JsonException("Tipo não conhecido");

            //}
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }


        
    }
}
