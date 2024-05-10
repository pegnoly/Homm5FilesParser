using DDSReader;
using Homm5Parser.Common;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser {
    public static class CommonSerializers {
        public static XmlSerializer Texture = new XmlSerializer(typeof(Texture));
    }

    public static class CommonGenerators {
        public static (string?, string?) TextFileFromKey(string key, FilesDatabase database, string baseFileKey) {
            //if (key.StartsWith("/")) {
            //    key = key.Remove(0, 1);
            //}
            string fileKey = database.GetActualKey(key, baseFileKey)!;
            if (fileKey is not null) {
                return (fileKey, database.GetTextFile(fileKey)!);
            }
            return (null, null);
        }

        public static (string?, string?) ImageFileFromKey(string key, FilesDatabase database, string baseFileKey, string id, string dirToSave) {
            //if (key.StartsWith("/")) {
            //    key = key.Remove(0, 1);
            //}
            string iconKey = database.GetActualKey(key, baseFileKey)!;
            if (iconKey is not null && iconKey != string.Empty) {
                Texture texture = (Texture)CommonSerializers.Texture.Deserialize(XDocument.Parse(database.GetTextFile(iconKey)!).CreateReader())!;
                if (texture.DestName is not null && texture.DestName.href != string.Empty) {
                    string imageKey = database.GetActualKey(texture.DestName.href!, iconKey)!;
                    if (imageKey is not null && imageKey != string.Empty) {
                        byte[] imageBytes = database.GetImageFile(imageKey)!;
                        if (imageBytes is not null) {
                            DDSImage image = new DDSImage(imageBytes);
                            string iconPngPath = $"{dirToSave}{id}.png";
                            image.Save(iconPngPath);
                            return ($"{iconKey}#xpointer(/Texture)", iconPngPath);
                        }
                    }
                }
            }
            return (null, null);
        }
    }
}
