using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core;

internal class FoldersHolder {
    private readonly string _applicationFolder;
    public FoldersHolder (string applicationName) {
        _applicationFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            applicationName);

        if (!Directory.Exists(_applicationFolder)) { Directory.CreateDirectory(_applicationFolder); }
    }
}
