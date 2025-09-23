using Hedger.Models;
using Services.Loaders;

namespace Hedger.Services;
public class ContainersJsonLoader (string folder) : JsonLoader<Container>(folder) {
    protected override string buildName (Container item) => item.Id.ToString();
}
