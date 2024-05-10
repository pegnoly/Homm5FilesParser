using Homm5Parser;
using Homm5Parser.Entities;
using Newtonsoft.Json;

IReadOnlyList<string> _fileDirectories = new List<string>() {
    "data", "Maps", "UserMODs"
};

IReadOnlyList<string> _hommFilesExtensions = new List<string>() {
    ".pak", ".h5m", ".h5c", ".h5u"
};

string appPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", string.Empty);

Console.WriteLine("Укажите путь к папке героев: ");
string? hommPath = Console.ReadLine();
Paths.HommData = hommPath + "data\\";

List<FileInfo> fileInfos = new List<FileInfo>();
foreach (string directory in _fileDirectories) {
    string dirPath = $"{hommPath}{directory}\\";
    foreach (FileInfo fileInfo in new DirectoryInfo(dirPath).GetFiles()) {
        if (_hommFilesExtensions.Contains(fileInfo.Extension)) {
            fileInfos.Add(fileInfo);
        }
    }
}
Parser parser = new Parser();
parser.Run(fileInfos);