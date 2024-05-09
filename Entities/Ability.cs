using Homm5Parser.Common;

namespace Homm5Parser.Entities {
    [Serializable]
    public class Ability {
        public FileRef? NameFileRef { get; set; }
        public FileRef? DescriptionFileRef { get; set; }
    }

    public class AbilityDataModel {
        public string? Id { get; set; }
        public string? NamePath { get; set; }
        public string? Name { get; set; }
        public string? DescPath { get; set; }
        public string? Desc { get; set; }
    }
}