using System.Text.Json.Serialization;

namespace Core.Models.Json;

[JsonConverter(typeof(JsonStringEnumConverter<InstrumentRightJsonModel>))]
public enum InstrumentTypeJsonModel {
    Future,
    Option
}
