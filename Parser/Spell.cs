using Homm5Parser.Common;
using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser.Concrete {
    [Serializable]
    public class SpellObject {
        public string? ID { get; set; }
        public FileRef? Obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_Spell_SpellID")]
    public class SpellsTable {
        [XmlArrayItem("Item")]
        public List<SpellObject>? objects { get; set; }
    }

    /// <summary>
    /// Парсит инфу о спеллах в бд
    /// </summary>
    public class SpellsDataParser : IParser  {

        private readonly string _spellsXdbKey = "GameMechanics/RefTables/UndividedSpells.xdb";
        private readonly string _iconsFolder = $"{Paths.HommData}spells\\";
        private XmlSerializer _spellSerializer = new XmlSerializer(typeof(Spell));

        private FilesDatabase _database;
        private List<SpellDataModel> _models;

        public SpellsDataParser(FilesDatabase database, List<SpellDataModel> models) {
            _database = database;
            _models = models;

            if (!Directory.Exists(_iconsFolder)) {
                Directory.CreateDirectory(_iconsFolder);
            }
        }

        public void Parse() {
            XmlSerializer spellsTableSerializer = new XmlSerializer(typeof(SpellsTable));
            XDocument spellsTableDocument = XDocument.Parse(_database.GetTextFile(_spellsXdbKey)!);
            SpellsTable spellsEntities = (SpellsTable)spellsTableSerializer.Deserialize(spellsTableDocument.CreateReader())!;
            foreach (SpellObject entity in spellsEntities.objects!) {
                if(entity.Obj is not null && entity.Obj.href is not null && entity.Obj.href != string.Empty) {
                    string spellKey = _database.GetActualKey(entity.Obj!.href!.Replace("#xpointer(/Spell)", string.Empty).Remove(0, 1), _spellsXdbKey)!;
                    if(spellKey is not null) {
                        string spellFile = _database.GetTextFile(spellKey)!;
                        Spell spell = (Spell)_spellSerializer.Deserialize(XDocument.Parse(spellFile).CreateReader())!;
                        _models.Add(ConvertToDataModel(spell, entity.ID!, spellKey));           
                    }
                }
            }
            string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText($"{Paths.HommData}spells.json", s);
        }

        private SpellDataModel ConvertToDataModel(Spell spell, string id, string key) {
            SpellDataModel model = new SpellDataModel();
            model.Id = id;
            model.Manacost = spell.TrainedCost;
            model.School = spell.MagicSchool;
            model.Level = spell.Level;
            model.HeroLevelToLearn = spell.RequiredHeroLevel;
            model.FirstSpDependedParam = spell.damage;
            model.SecondSpDependedParam = spell.duration;
            model.ResourceCost = spell.sSpellCost;
            if (spell.NameFileRef is not null && spell.NameFileRef.href is not null && spell.NameFileRef.href != string.Empty) {
                (model.NamePath, model.Name) = CommonGenerators.TextFileFromKey(spell.NameFileRef.href, _database, key);
            }
            if (spell.LongDescriptionFileRef is not null && spell.LongDescriptionFileRef.href is not null && spell.LongDescriptionFileRef.href != string.Empty) {
                (model.DescPath, model.Desc) = CommonGenerators.TextFileFromKey(spell.LongDescriptionFileRef.href, _database, key);
            }
            if (spell.Texture is not null && spell.Texture.href is not null && spell.Texture.href != string.Empty) {
                (model.IconPath, model.Icon) = CommonGenerators.ImageFileFromKey(spell.Texture.href.Replace("#xpointer(/Texture)", string.Empty), _database, key, id, _iconsFolder);
            }
            return model;
        }
    }
}
