using Homm5Parser.Common;
using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser.Concrete {

    public class AdvMapSharedGroup {
        [XmlArrayItem("Item")]
        public List<FileRef> links { get; set; } = new List<FileRef>();
    }

    /// <summary>
    /// Парсит файлы героев в бд
    /// </summary>
    public class HeroesDataParser : IParser {

        private readonly IReadOnlyList<string> _possibleHeroesGroups = new List<string>() {
            "Academy", "Dungeon_Standart", "Fortress", "Haven_Standart", "Inferno_Standart",
            "Necropolis_Standart", "Preserve_Standart", "Stronghold"
        };
        private readonly string _specIconsFolder = $"{Paths.HommData}hero\\spec_icons\\";
        private readonly string _iconsFolder = $"{Paths.HommData}hero\\icons\\";

        private XmlSerializer _heroSerializer = new XmlSerializer(typeof(AdvMapHeroShared));
        private List<HeroDataModel> _models;
        private FilesDatabase _database;

        public HeroesDataParser(FilesDatabase database, List<HeroDataModel> models) {
            _database = database;
            _models = models;
            if (!Directory.Exists(_specIconsFolder)) {
                Directory.CreateDirectory(_specIconsFolder);
            }
            if(!Directory.Exists(_iconsFolder)) {
                Directory.CreateDirectory(_iconsFolder);
            }
        }

        public void Parse() {
            foreach (string group in _possibleHeroesGroups) {
                XDocument heroesDocument = XDocument.Parse(_database.GetTextFile($"MapObjects/_(AdvMapSharedGroup)/Heroes/{group}.xdb")!);
                XmlSerializer heroesGroupSerializer = new XmlSerializer(typeof(AdvMapSharedGroup));
                AdvMapSharedGroup heroesGroup = (AdvMapSharedGroup)heroesGroupSerializer.Deserialize(heroesDocument.CreateReader())!;
                foreach (FileRef link in heroesGroup.links) {
                    string href = link.href!;
                    string key = href.Replace("#xpointer(/AdvMapHeroShared)", string.Empty).Remove(0, 1);
                    string heroXdbString = _database.GetTextFile(key)!;
                    XDocument heroDoc = XDocument.Parse(heroXdbString);
                    AdvMapHeroShared hero = (AdvMapHeroShared)_heroSerializer.Deserialize(heroDoc.CreateReader())!;
                    _models.Add(ConvertToModel(hero, key));
                }
                string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
                File.WriteAllText($"{Paths.HommData}heroes.json", s);
            }
        }

        private HeroDataModel ConvertToModel(AdvMapHeroShared hero, string id) {
            HeroDataModel model = new HeroDataModel();
            model.Xdb = id;
            model.ClassId = hero.Class;
            model.SpecId = hero.Specialization;
            model.PrimarySkill = hero.PrimarySkill;
            model.Town = hero.TownType;
            model.BaseSkills = hero.Editable!.skills;
            model.BasePerks = hero.Editable!.perkIDs;
            model.BaseSpells = hero.Editable!.spellIDs;

            if(hero.SpecializationNameFileRef is not null && hero.SpecializationNameFileRef.href is not null && hero.SpecializationNameFileRef.href != string.Empty) {
                (model.SpecNamePath, model.SpecName) = CommonGenerators.TextFileFromKey(hero.SpecializationNameFileRef.href, _database, id);
            }

            if (hero.SpecializationDescFileRef is not null && hero.SpecializationDescFileRef.href is not null && hero.SpecializationDescFileRef.href != string.Empty) {
                (model.SpecDescPath, model.SpecDesc) = CommonGenerators.TextFileFromKey(hero.SpecializationDescFileRef.href, _database, id);
            }

            if (hero.SpecializationIcon is not null && hero.SpecializationIcon.href is not null && hero.SpecializationIcon.href != string.Empty) {
                (model.SpecIconPath, model.SpecIcon) = CommonGenerators.ImageFileFromKey(hero.SpecializationIcon.href.Replace("#xpointer(/Texture)", string.Empty), _database, id, hero.InternalName!, _specIconsFolder);
            }

            if (hero.FaceTexture is not null && hero.FaceTexture.href is not null && hero.FaceTexture.href != string.Empty) {
                (model.IconPath, model.Icon) = CommonGenerators.ImageFileFromKey(hero.FaceTexture.href.Replace("#xpointer(/Texture)", string.Empty), _database, id, hero.InternalName!, _iconsFolder);
            }

            if(hero.Editable is not null) {
                if (hero.Editable.NameFileRef is not null && hero.Editable.NameFileRef.href is not null && hero.Editable.NameFileRef.href != string.Empty) {
                    (model.NamePath, model.Name) = CommonGenerators.TextFileFromKey(hero.Editable.NameFileRef.href, _database, id);
                }
                if (hero.Editable.BiographyFileRef is not null && hero.Editable.BiographyFileRef.href is not null && hero.Editable.BiographyFileRef.href != string.Empty) {
                    (model.BioPath, model.Bio) = CommonGenerators.TextFileFromKey(hero.Editable.BiographyFileRef.href, _database, id);
                }
            }

            return model;
        }
    }
}
