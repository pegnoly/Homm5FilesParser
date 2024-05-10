using Homm5Parser.Entities;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Homm5Parser.Concrete {

    [Serializable]
    public class ArtifactObject {
        public string? ID { get; set; }
        public Artifact? obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_DBArtifact_ArtifactEffect")]
    public class ArtifactsTable {
        [XmlArrayItem("Item")]
        public List<ArtifactObject>? objects { get; set; }
    }

    public class ArtifactsDataParser: IParser {

        private readonly string _artifactsXdbKey = "/GameMechanics/RefTables/Artifacts.xdb";
        private readonly string _iconsFolfer = $"{Paths.HommData}artifacts\\";

        private List<ArtifactDataModel> _models;
        private FilesDatabase _database;

        public ArtifactsDataParser(FilesDatabase database, List<ArtifactDataModel> models) {
            _database = database;
            _models = models;

            if(!Directory.Exists(_iconsFolfer)) {
                Directory.CreateDirectory(_iconsFolfer);
            }
        }

        public void Parse() {

            XmlSerializer artTableSerializer = new XmlSerializer(typeof(ArtifactsTable));
            XDocument artTableDocument = XDocument.Parse(_database.GetTextFile(_artifactsXdbKey)!);
            ArtifactsTable artifactsEntities = (ArtifactsTable)artTableSerializer.Deserialize(artTableDocument.CreateReader())!;

            foreach (ArtifactObject entity in artifactsEntities.objects!) {
                _models.Add(ConvertToDataModel(entity.obj!, entity.ID!));
            }
            string s = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText($"{Paths.HommData}arts.json", s);
        }

        private ArtifactDataModel ConvertToDataModel(Artifact artifact, string id) {
            ArtifactDataModel model = new ArtifactDataModel();
            model.Id = id;
            model.Type = artifact.Type;
            model.Slot = artifact.Slot;
            model.Cost = artifact.CostOfGold;

            if (artifact.NameFileRef is not null && artifact.NameFileRef.href is not null && artifact.NameFileRef.href != string.Empty) {
                (model.NamePath, model.Name) = CommonGenerators.TextFileFromKey(artifact.NameFileRef.href, _database, _artifactsXdbKey);
            }

            if (artifact.DescriptionFileRef is not null && artifact.DescriptionFileRef.href is not null && artifact.DescriptionFileRef.href != string.Empty) {
                (model.DescPath, model.Desc) = CommonGenerators.TextFileFromKey(artifact.DescriptionFileRef.href, _database, _artifactsXdbKey);
            }

            if (artifact.Icon is not null && artifact.Icon.href is not null && artifact.Icon.href != string.Empty) {
                (model.IconPath, model.Icon) = CommonGenerators.ImageFileFromKey(artifact.Icon.href.Replace("#xpointer(/Texture)", string.Empty), _database, _artifactsXdbKey, id, _iconsFolfer);
            }
            return model;
        }
    }
}
