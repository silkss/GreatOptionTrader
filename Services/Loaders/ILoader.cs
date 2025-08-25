using System.Collections.Generic;

namespace Services.Loaders;

public interface ILoader<TItem> {
    IEnumerable<TItem> LoadAll ();
    void Save (TItem item);
}
