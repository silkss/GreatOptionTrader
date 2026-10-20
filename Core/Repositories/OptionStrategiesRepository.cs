using Core.Domain;
using Core.Models;
using Core.Models.Json;

namespace Core.Repositories;

internal class OptionStrategiesRepository : Base.JsonRepository<OptionStrategy, OptionSstrategyJsonModel> {
    protected override OptionStrategy toDomain (
        OptionSstrategyJsonModel model
    ) => new(
        instrument: model.Instrument.ToDomain(),
        account: model.Account
    );

    protected override OptionSstrategyJsonModel toModel (
        OptionStrategy domain
    ) => new(
        Instrument: domain.Instrument.ToModel(),
        Account: domain.Account
    );
}
