using Homm5Parser.Common;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Homm5Parser.Entities {

    [Serializable]
    public class SpellTypeItem {
        public string? Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    [Serializable]
    [XmlRoot("Item")]
    public class SpellTypesTable {
        [XmlArrayItem("Item")]
        public List<SpellTypeItem> Entries { get; set; } = new List<SpellTypeItem>();
    }

    public class TypesParser {

        private FilesDatabase? _database;

        public TypesParser(FilesDatabase database) {
            _database = database;
        }

        public void Parse() {
            XDocument document = XDocument.Parse(_database!.GetTextFile("types.xml")!);
            IEnumerable <XElement> elements = document.XPathSelectElements("Base/SharedClasses/Item")!;
            IEnumerable<XElement> e = elements.Where(e => (e.Element("TypeName") != null) && e.Element("TypeName")!.Value == "SpellID");
            XmlSerializer xs = new XmlSerializer(typeof(SpellTypesTable));
            SpellTypesTable table = (SpellTypesTable)xs.Deserialize(e.First().CreateReader())!;
            foreach(SpellTypeItem item in table.Entries) {
                Console.WriteLine(item.Name);
            }
            //IEnumerable<XmlNode> spellsBaseNode = list.Cast<XmlNode>().Where(n => n.Name == "TypeName" && n.Value == "SpellID");
            //XmlNode spellNode = spellsBaseNode.First()!;
        }
    }
}
