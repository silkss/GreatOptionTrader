using GreatOptionTrader.Models;
using Services.Loaders;

namespace GreatOptionTrader.Services;
public class JsonOptionStrategiesContainerLoader(string folder) : JsonLoader<OptionStrategiesContainer>(folder) {
    protected override string buildName (OptionStrategiesContainer item) {
        return item.Guid.ToString();
    }
}
