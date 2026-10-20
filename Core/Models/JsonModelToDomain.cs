namespace Core.Models;

static class JsonModelToDomain {
    public static Domain.InstrumentRight ToDomain (
        this Json.InstrumentRightJsonModel model
    ) => model switch {
        Json.InstrumentRightJsonModel.Put => Domain.InstrumentRight.Put,
        Json.InstrumentRightJsonModel.Call => Domain.InstrumentRight.Call,
        Json.InstrumentRightJsonModel.None => Domain.InstrumentRight.None
        _ => throw new System.Exception($"unsupported model: {typeof(Json.InstrumentRightJsonModel)}")
    };

    public static Domain.InstrumentType ToDomain (
        this Json.InstrumentTypeJsonModel model
    ) => model switch {
        Json.InstrumentTypeJsonModel.Future => Domain.InstrumentType.Future,
        Json.InstrumentTypeJsonModel.Option => Domain.InstrumentType.Option,
        _ => throw new System.Exception($"unsupported model: {typeof(Json.InstrumentTypeJsonModel)}")
    };

    public static Domain.Instrument ToDomain (
        this Json.InstrumentJsonModel model
    ) => new(
        right: model.Right.ToDomain(),
        type: model.Type.ToDomain(),
        id: model.Id,
        name: model.Name,
        symbol: model.Symbol,
        exchange: model.Exchange,
        expirationAt: model.ExpirationAt
    );
}

