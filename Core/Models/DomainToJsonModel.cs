namespace Core.Models;

static class DomainToJsonModel {
    public static Json.InstrumentRightJsonModel ToModel (
        this Domain.InstrumentRight domain
    ) => domain switch {
        Domain.InstrumentRight.Put => Json.InstrumentRightJsonModel.Put,
        Domain.InstrumentRight.Call => Json.InstrumentRightJsonModel.Call,
        Domain.InstrumentRight.None => Json.InstrumentRightJsonModel.None,
        _ => throw new System.Exception($"unsupported domain: {typeof(Domain.InstrumentRight)}")
    };

    public static Json.InstrumentTypeJsonModel ToModel (
        this Domain.InstrumentType domain
    ) => domain switch {
        Domain.InstrumentType.Future => Json.InstrumentTypeJsonModel.Future,
        Domain.InstrumentType.Option => Json.InstrumentTypeJsonModel.Option,
        _ => throw new System.Exception($"unsupported domain: {typeof(Domain.InstrumentType)}")
    };

    public static Json.InstrumentJsonModel ToModel (
        this Domain.Instrument domain
    ) => new(
        Right: domain.Right.ToModel(),
        Type: domain.Type.ToModel(),
        Id: domain.Id,
        Name: domain.Name,
        Symbol: domain.Symbol,
        Exchange: domain.Exchange,
        ExpirationAt: domain.ExpirationAt
    );
}
