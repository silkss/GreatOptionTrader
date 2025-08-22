using GreatOptionTrader.Models;

namespace GreatOptionTrader.Services.Loaders;
public class JsonOptionStrategiesContainerLoader(string folder) : JsonLoader<OptionStrategiesContainer>(folder) {
    protected override string buildName (OptionStrategiesContainer item) {
        return item.Guid.ToString();
    }
}
