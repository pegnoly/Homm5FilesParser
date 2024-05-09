using Homm5Parser.Common;
using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser.Concrete {


    [Serializable]
    public class SkillObject {
        public string? ID { get; set; }
        [XmlElement("obj")]
        public Skill? Skill { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_HeroSkill_SkillID")]
    public class SkillsTable {
        [XmlArrayItem("Item")]
        public List<SkillObject> objects { get; set; } = new List<SkillObject>();
    }

    public class HeroSkillDataParser : IParser {

        private readonly string _heroSkillsXdbKey = "GameMechanics/RefTables/Skills.xdb";
        private readonly string _iconsFolder = $"{Paths.HommData}skills\\";

        private List<SkillDataModel> _models;
        private FilesDatabase _database;

        public HeroSkillDataParser(FilesDatabase database, List<SkillDataModel> models) {
            _database = database;
            _models = models;

            if(!Directory.Exists(_iconsFolder)) {
                Directory.CreateDirectory(_iconsFolder);
            }
        }

        public void Parse() {
            XmlSerializer skillsTableSerializer = new XmlSerializer(typeof(SkillsTable));
            XDocument skillsTableDocument = XDocument.Parse(_database.GetTextFile(_heroSkillsXdbKey)!);
            SkillsTable heroSkillEntities = (SkillsTable)skillsTableSerializer.Deserialize(skillsTableDocument.CreateReader())!;
            foreach (SkillObject entity in heroSkillEntities.objects!) {
                _models.Add(ConvertToDataModel(entity.Skill!, entity.ID!));
            }
            string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText($"{Paths.HommData}skills.json", s);
        }

        private SkillDataModel ConvertToDataModel(Skill source, string id) {
            SkillDataModel model = new SkillDataModel();
            model.Id = id;
            model.Dependencies = source.SkillPrerequisites!;
            model.BasicSkill = source.BasicSkillID;
            model.HeroClass = source.HeroClass;
            if (source.Texture is not null) {
                int texturesCount = 1;
                foreach (FileRef textureRef in source.Texture) {
                    if (textureRef.href is not null && textureRef.href != string.Empty) {
                        (string? path, string? icon) = CommonGenerators.ImageFileFromKey(textureRef.href.Replace("#xpointer(/Texture)", string.Empty),
                                                                                        _database, _heroSkillsXdbKey, $"{id}_{texturesCount}", _iconsFolder);
                        if (path is not null && icon is not null) {
                            model.IconsPaths.Add(path);
                            model.Icons.Add(icon);
                        }
                        texturesCount++;
                    }
                }
            }
            if (source.NameFileRef is not null) {
                foreach(FileRef nameRef in  source.NameFileRef) { 
                    if(nameRef.href is not null && nameRef.href != string.Empty) {
                        (string? path, string? name) = CommonGenerators.TextFileFromKey(nameRef.href, _database, _heroSkillsXdbKey);
                        if (path is not null && name is not null) {
                            model.NamesPaths.Add(path);
                            model.Names.Add(name);
                        }
                    }
                }
            }
            if (source.DescriptionFileRef is not null) {
                foreach (FileRef descRef in source.DescriptionFileRef) {
                    if (descRef.href is not null && descRef.href != string.Empty) {
                        (string? path, string? desc) = CommonGenerators.TextFileFromKey(descRef.href, _database, _heroSkillsXdbKey);
                        if (path is not null && desc is not null) {
                            model.DescsPaths.Add(path);
                            model.Descs.Add(desc);
                        }
                    }
                }
            }
            return model;
        }
    }
}
