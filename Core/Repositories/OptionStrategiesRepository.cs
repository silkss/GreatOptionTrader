using Core.Loaders;
using Core.Models;

namespace Core.Repositories;

internal class OptionStrategiesRepository : Base.JsonRepository<Domain.OptionStrategy, Models.Json.OptionSstrategyJsonModel> {

    public OptionStrategiesRepository (JsonLoader<Models.Json.OptionSstrategyJsonModel> loader) : base(loader) { }

    protected override Domain.OptionStrategy toDomain (
        Models.Json.OptionSstrategyJsonModel model
    ) => new(
        id: model.Id,
        account: model.Account,
        instrument: model.Instrument.ToDomain()
    );

    protected override Models.Json.OptionSstrategyJsonModel toModel (
        Domain.OptionStrategy domain
    ) => new(
        Id: domain.Id,
        Account: domain.Account,
        Instrument: domain.Instrument.ToModel()
    );
}
