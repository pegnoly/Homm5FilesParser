using Homm5Parser.Concrete;
using Homm5Parser;
using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5FilesParser.Parser {
    public class AbilityObject {
        public string? ID { get; set; }
        public Ability? obj { get; set; }
    }

    [XmlRoot("Table_CreatureAbility_CombatAbility")]
    public class AbilitiesTable {
        [XmlArrayItem("Item")]
        public List<AbilityObject>? objects { get; set; }
    }

    public class AbilitiesDataParser : IParser {

        private readonly string _abilitiesXdbKey = "/GameMechanics/RefTables/CombatAbilities.xdb";

        private List<AbilityDataModel> _models;
        private FilesDatabase _database;

        public AbilitiesDataParser(FilesDatabase database, List<AbilityDataModel> models) {
            _database = database;
            _models = models;
        }

        public void Parse() {

            XmlSerializer abilitiesTableSerializer = new XmlSerializer(typeof(AbilitiesTable));
            XDocument abilitiesTableDocument = XDocument.Parse(_database.GetTextFile(_abilitiesXdbKey)!);
            AbilitiesTable abilitiesEntities = (AbilitiesTable)abilitiesTableSerializer.Deserialize(abilitiesTableDocument.CreateReader())!;

            foreach (AbilityObject entity in abilitiesEntities.objects!) {
                _models.Add(ConvertToDataModel(entity.obj!, entity.ID!));
            }
            string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText($"{Paths.HommData}abilities.json", s);
        }

        private AbilityDataModel ConvertToDataModel(Ability ability, string id) {
            AbilityDataModel model = new AbilityDataModel();
            model.Id = id;

            if (ability.NameFileRef is not null && ability.NameFileRef.href is not null && ability.NameFileRef.href != string.Empty) {
                (model.NamePath, model.Name) = CommonGenerators.TextFileFromKey(ability.NameFileRef.href, _database, _abilitiesXdbKey);
            }

            if (ability.DescriptionFileRef is not null && ability.DescriptionFileRef.href is not null && ability.DescriptionFileRef.href != string.Empty) {
                (model.DescPath, model.Desc) = CommonGenerators.TextFileFromKey(ability.DescriptionFileRef.href, _database, _abilitiesXdbKey);
            }

            return model;
        }
    }
}
