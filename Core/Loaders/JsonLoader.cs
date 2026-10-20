using Core.Abstractions;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Core.Loaders;

public class JsonLoader<T> where T: IJsonModel {
    private readonly string _folder;
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions() {
        WriteIndented = true
    };

    public JsonLoader(string folder) {
        _folder = folder;
    }

    public IEnumerable<T> Load () {
        foreach (var path in Directory.GetFiles(_folder)) {
            using (var reader = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                if (JsonSerializer.Deserialize<T>(reader) is T item) {
                    yield return item;
                }
            }
        }

        yield break;
    }

    public void Save (T item) {
        var path = $"{_folder}\\{item.Id}.json";
        using (var writer = new FileStream(path, FileMode.Create, FileAccess.Write)) {
            JsonSerializer.Serialize(writer, _serializerOptions);
        }
    }
}
