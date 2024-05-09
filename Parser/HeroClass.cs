﻿using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser.Concrete {
    [Serializable]
    public class HeroClassObject {
        public string? ID { get; set; }
        public HeroClass? obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_HeroClassDesc_HeroClass")]
    public class HeroClassTable {
        [XmlArrayItem("Item")]
        public List<HeroClassObject>? objects { get; set; }
    }

    public class HeroClassDataParser: IParser {

        private readonly string _heroClassXdbKey = "GameMechanics/RefTables/HeroClass.xdb";

        private FilesDatabase _database;
        private List<HeroClassDataModel> _models;

        public HeroClassDataParser(FilesDatabase database, List<HeroClassDataModel> models) {
            _database = database;
            _models = models;
        }

        public void Parse() {
            XmlSerializer heroClassTableSerializer = new XmlSerializer(typeof(HeroClassTable));
            XDocument heroClassTableDocument = XDocument.Parse(_database!.GetTextFile(_heroClassXdbKey)!);
            HeroClassTable heroClassEntities = (HeroClassTable)heroClassTableSerializer.Deserialize(heroClassTableDocument.CreateReader())!;
            foreach (HeroClassObject entity in heroClassEntities.objects!) {
                _models.Add(ConvertToDataModel(entity.obj!, entity.ID!));
            }
            string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText($"{Paths.HommData}classes.json", s);
        }

        private HeroClassDataModel ConvertToDataModel(HeroClass source, string id) {
            HeroClassDataModel model = new HeroClassDataModel();
            model.Id = id;
            model.AttributesProbs = source.AttributesProbs;
            model.SkillsProbs = source.SkillsProbs;
            if (source.Name is not null && source.Name!.href != string.Empty) {
                string nameKey = _database.GetActualKey(source.Name.href!, _heroClassXdbKey)!;
                if (nameKey is not null) {
                    model.NamePath = nameKey;
                    model.Name = _database.GetTextFile(nameKey);
                }
            }
            return model;
        }
    }
}
