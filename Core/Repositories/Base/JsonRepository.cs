using Core.Loaders;
using Core.Abstractions;
using System.Linq;
using System.Collections.ObjectModel;

namespace Core.Repositories.Base;

public abstract class JsonRepository<TDomain, TModel> where TModel : IJsonModel {
    private readonly JsonLoader<TModel> _loader;

    public ObservableCollection<TDomain> Items { get; }

    public JsonRepository(JsonLoader<TModel> loader) {
        _loader = loader;
        Items = _loader.Load().Select(toDomain).ToObservable();
    }

    protected abstract TDomain toDomain (TModel model);
    protected abstract TModel toModel (TDomain domain);

    public void Create(TDomain item) {
        Items.Add(item);
        _loader.Save(toModel(item));
    }
}
