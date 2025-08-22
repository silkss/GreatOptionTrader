using System.Collections.Generic;

namespace GreatOptionTrader.Services.Loaders;

public interface ILoader<TItem> {
    IEnumerable<TItem> LoadAll ();
    void Save (TItem item);
}
