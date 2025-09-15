using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Services.Loaders;

public abstract class JsonLoader<TItem>(string folder) : ILoader<TItem> {
    private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions() {
        WriteIndented = true
    };

    protected abstract string buildName (TItem item);

    public IEnumerable<TItem> LoadAll() {
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        foreach (var path in Directory.GetFiles(folder, "*.json")) {
            var content = File.ReadAllText(path);
            var item = JsonSerializer.Deserialize<TItem>(content);
            if (item != null) yield return item;
        }
        yield break;
    }

    public void Save (TItem item) {
        var path = folder +"//" + buildName(item) + ".json";
        var content = JsonSerializer.Serialize(item, serializerOptions);
        File.WriteAllText(path, content);
    }
}
