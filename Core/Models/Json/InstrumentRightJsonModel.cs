using System.Text.Json.Serialization;

namespace Core.Models.Json;

[JsonConverter(typeof(JsonStringEnumConverter<InstrumentRightJsonModel>))]
public enum InstrumentRightJsonModel {
    None,
    Put,
    Call
}
