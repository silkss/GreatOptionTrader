using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core;

public static class Utils {
    public static ObservableCollection<TSource> ToObservable<TSource> (
        this IEnumerable<TSource> sources
    ) => new ObservableCollection<TSource>(sources);
}
