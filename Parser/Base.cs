using SevenZipExtractor;
using System.Text;

namespace Homm5Parser {

    public interface IParser {
        void Parse();
    }

    /// <summary>
    /// База данных файлов игры для парсинга.
    /// </summary>
    public class FilesDatabase {

        private List<string> _keys = new List<string>();
        private Dictionary<string, string> _textFiles = new Dictionary<string, string>();
        // файлы изображений храним как массивы байтов, т.к. того требует DDSReader класс(хранение в формате DDSImage выдавало ошибку, не смог разобраться пока)
        private Dictionary<string, byte[]> _imageFiles = new Dictionary<string, byte[]>();
        private Dictionary<string, DateTime> _modifiedTimes = new Dictionary<string, DateTime>();
        private Dictionary<string, string> _directories = new Dictionary<string, string>();

        /// <summary>
        /// Чекает паки в указанной папке и производит словарь актуальных файлов игры и их содержимого.
        /// </summary>
        /// <param name="directory"></param>
        public void Create(List<FileInfo> possibleFiles) {
            foreach(FileInfo file in possibleFiles) {
                using (ArchiveFile zip = new ArchiveFile(file.FullName)) {
                    foreach (Entry entry in zip.Entries) {
                        if (IsTextFile(entry.FileName)) {
                            ProcessTextFile(entry);
                        }
                        else if(IsImageFile(entry.FileName)) {
                            ProcessImageFile(entry);
                        }
                    }
                }
            }
        }

        private void ProcessTextFile(Entry entry) {
            if (!_textFiles.ContainsKey(entry.FileName) || (_modifiedTimes[entry.FileName] < entry.LastWriteTime)) {
                MemoryStream stream = new MemoryStream();
                entry.Extract(stream);
                byte[] b = stream.ToArray();
                string s = string.Empty;
                // текст файлы имеют разную с xml-никами кодировку...
                if(entry.FileName.EndsWith(".txt")) {
                    s = Encoding.Unicode.GetString(b);
                }
                else {
                    s = Encoding.UTF8.GetString(b);
                }
                string key = $"/{entry.FileName.Replace("\\", "/")}";
                _textFiles[key] = s;
                _modifiedTimes[key] = entry.LastWriteTime;
                _directories[key] = CheckDirectory(key);
                if(!_keys.Contains(key)) {
                    _keys.Add(key);
                }
            }
        }

        private void ProcessImageFile(Entry entry) {
            if (!_imageFiles.ContainsKey(entry.FileName) || (_modifiedTimes[entry.FileName] < entry.LastWriteTime)) {
                string key = $"/{entry.FileName.Replace("\\", "/")}";
                MemoryStream stream = new MemoryStream();
                entry.Extract(stream);
                _imageFiles[key] = stream.ToArray();
                _modifiedTimes[key] = entry.LastWriteTime;
                _directories[key] = CheckDirectory(key);
                if (!_keys.Contains(key)) {
                    _keys.Add(key);
                }
            }
        }

        private bool IsTextFile(string fileName) {
            return (fileName.EndsWith(".xdb") || fileName.EndsWith(".xml") || fileName.EndsWith(".txt"));
        }

        private bool IsImageFile(string fileName) {
            return (fileName.EndsWith(".dds") || fileName.EndsWith(".tga"));
        }

        private string CheckDirectory(string path) {
            string name = path.Split("/").Last();
            return path.Replace(name, string.Empty);
        }

        /// <summary>
        /// Определяет ключ файла в базе относительно ключа файла, в котором содержится ссылка на проверяемый.
        /// Суть в чем - в файлах игры могут быть указаны ссылки на другие файлы с путем, записанным относительно базового файла, этот метод позволяет получить абсолютный путь для таких случаев.
        /// </summary>
        /// <param name="key">Ключ проверяемого файла</param>
        /// <param name="basePath">Ключ файла, содержащего ссылку на проверяемый файл</param>
        /// <returns></returns>
        public string? GetActualKey(string key, string basePath) {
            if(_keys.Contains(key)) {
                return key;
            }
            string absoluteKey = _directories[basePath] + key;
            if(_keys.Contains(absoluteKey)) {
                return absoluteKey;
            }
            return null;
        }

        public string? GetTextFile(string key) {
            if (!_textFiles.ContainsKey(key)) {
                return null;
            }
            return _textFiles[key];
        }

        public byte[]? GetImageFile(string key) {
            if (!_imageFiles.ContainsKey(key)) {
                return null;
            }
            return _imageFiles[key];
        }
    }
}
