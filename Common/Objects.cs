using System.Xml.Serialization;

namespace Homm5Parser.Common {
    /// <summary>
    /// Описывает тег, содержащий три значения одного типа(например Pos или Color)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Vec3<T> {
        public T? x { get; set; }

        public T? y { get; set; }

        public T? z { get; set; }
    }

    /// <summary>
    /// Описывает тег, представляющий собой ссылку на другой файл.
    /// </summary>
    [Serializable]
    public class FileRef {
        [XmlAttribute]
        public string? href { get; set; }
    }

    [Serializable]
    public class Texture {
        public FileRef? DestName { get; set; }
    }
}
