using Homm5Parser.Entities;
using Homm5Parser.Concrete;
using Homm5FilesParser.Parser;

namespace Homm5Parser {
    public class Parser {
        private List<IParser> _parsers = new List<IParser>();
        private FilesDatabase? _database;

        private List<CreatureDataModel> _creaturesData = new List<CreatureDataModel>();
        private List<HeroDataModel> _heroesData = new List<HeroDataModel>();
        private List<ArtifactDataModel> _artifactsData = new List<ArtifactDataModel>();
        private List<HeroClassDataModel> _classesData = new List<HeroClassDataModel>();
        private List<SkillDataModel> _skillsData = new List<SkillDataModel>();
        private List<SpellDataModel> _spellsData = new List<SpellDataModel>();
        private List<AbilityDataModel> _abilitiesData = new List<AbilityDataModel>();

        public Parser() {
            _database = new FilesDatabase();
            _parsers.Add(new CreaturesDataParser(_database, _creaturesData));
            _parsers.Add(new HeroesDataParser(_database, _heroesData));
            _parsers.Add(new ArtifactsDataParser(_database, _artifactsData));
            _parsers.Add(new HeroSkillDataParser(_database, _skillsData));
            _parsers.Add(new HeroClassDataParser(_database, _classesData));
            _parsers.Add(new SpellsDataParser(_database, _spellsData));
            _parsers.Add(new AbilitiesDataParser(_database, _abilitiesData));
        }

        public void Run(List<FileInfo> possibleFiles) {
            if(possibleFiles.Count() > 0) {
                _database!.Create(possibleFiles);
                foreach(IParser parser in _parsers) {
                    new Thread(() => {
                        parser.Parse();
                    }).Start();
                }
            }
        }
    }
}
