using DDSReader;
using Homm5Parser.Common;
using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser.Concrete {

    [Serializable]
    public class CreatureObject {
        public string? ID { get;set; }
        public FileRef? Obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_Creature_CreatureType")]
    public class CreaturesTable {
        [XmlArrayItem("Item")]
        public List<CreatureObject>? objects { get; set; }
    }

    /// <summary>
    /// Парсит файлы существ в бд
    /// </summary>
    public class CreaturesDataParser : IParser {

        private readonly string _creaturesXdbKey = "GameMechanics/RefTables/Creatures.xdb";
        private readonly string _iconsFolder = $"{Paths.HommData}creatures\\";

        private FilesDatabase? _database;
        private List<CreatureDataModel>? _models;
        private XmlSerializer _creatureSerializer = new XmlSerializer(typeof(Creature));
        private XmlSerializer _visualSerializer = new XmlSerializer(typeof(CreatureVisual));

        public CreaturesDataParser(FilesDatabase database, List<CreatureDataModel> models) {
            _database = database;
            _models = models;

            if(!Directory.Exists(_iconsFolder)) {
                Directory.CreateDirectory(_iconsFolder);
            }
        }

        public void Parse() {
            XmlSerializer creaturesTableSerializer = new XmlSerializer(typeof(CreaturesTable));
            XDocument creaturesTableDocument = XDocument.Parse(_database!.GetTextFile(_creaturesXdbKey)!);
            CreaturesTable creaturesEntities = (CreaturesTable)creaturesTableSerializer.Deserialize(creaturesTableDocument.CreateReader())!;
            foreach (CreatureObject entity in creaturesEntities.objects!) {
                string key = entity.Obj!.href!.Replace('\\', '/').Replace("#xpointer(/Creature)", string.Empty).Remove(0, 1);
                XDocument creatureDocument = XDocument.Parse(_database.GetTextFile(key)!);
                Creature creature = (Creature)_creatureSerializer.Deserialize(creatureDocument.CreateReader())!;
                _models!.Add(ConvertToDataModel(creature, entity.ID!, key));
            }
            string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText($"{Paths.HommData}creatures.json", s);
        }

        private CreatureDataModel ConvertToDataModel(Creature creature, string id, string key) {
            CreatureDataModel model = new CreatureDataModel();
            model.Id = id;
            model.Attack = creature.AttackSkill;
            model.Defence = creature.DefenceSkill;
            model.Shots = creature.Shots;
            model.MinDamage = creature.MinDamage;
            model.MaxDamage = creature.MaxDamage;
            model.KnownSpells = creature.KnownSpells;
            model.Initiative = creature.Initiative;
            model.Speed = creature.Speed;
            model.ManaPoints = creature.SpellPoints;
            model.Cost = creature.Cost;
            model.Size = creature.CombatSize;
            model.Abilities = creature.Abilities!;
            model.Experience = creature.Exp;
            model.Power = creature.Power;
            model.Level = creature.CreatureTier;
            model.Grow = creature.WeeklyGrowth;
            model.Range = creature.Range;
            model.FirstElement = creature.MagicElement!.First;
            model.SecondElement = creature.MagicElement!.Second;
            model.Town = creature.CreatureTown;
            model.Health = creature.Health;
            model.BaseCreature = creature.BaseCreature;

            if (creature.Visual is not null && creature.Visual.href is not null && creature.Visual.href != string.Empty) {
                XDocument doc = XDocument.Parse(_database!.GetTextFile(creature.Visual!.href!.Replace("#xpointer(/CreatureVisual)", string.Empty).Remove(0, 1))!);
                CreatureVisual visual = (CreatureVisual)_visualSerializer.Deserialize(doc.CreateReader())!;
                if(visual.CreatureNameFileRef is not null && visual.CreatureNameFileRef.href is not null && visual.CreatureNameFileRef.href != string.Empty) {
                    (model.NamePath, model.Name) = CommonGenerators.TextFileFromKey(visual.CreatureNameFileRef.href, _database, key); 
                }
                if (visual.DescriptionFileRef is not null && visual.DescriptionFileRef.href is not null && visual.DescriptionFileRef.href  != string.Empty) {
                    (model.DescPath, model.Desc) = CommonGenerators.TextFileFromKey(visual.DescriptionFileRef.href, _database, key);
                }
                if (visual.Icon128 is not null && visual.Icon128.href is not null && visual.Icon128.href != string.Empty) {
                    (model.IconPath, model.Icon) = CommonGenerators.ImageFileFromKey(visual.Icon128!.href!.Replace("#xpointer(/Texture)", string.Empty), _database, key, id, _iconsFolder);
                }
            }

            return model;
        }
    }
}
