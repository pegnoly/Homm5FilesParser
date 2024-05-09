using Homm5Parser.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Homm5Parser.Entities {

    /// <summary>
    /// Типы слотов артефактов
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ArtifactSlot {
        INVENTORY,
        PRIMARY,
        SECONDARY,
        HEAD,
        CHEST,
        NECK,
        FINGER,
        FEET,
        SHOULDERS,
        MISCSLOT1
    }

    /// <summary>
    /// Типы силы артефактов
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ArtifactType {
        ARTF_CLASS_MINOR,
        ARTF_CLASS_MAJOR,
        ARTF_CLASS_RELIC,
        ARTF_CLASS_GRAIL
    }

    /// <summary>
    /// Форма информации об артефакте для файлов игры.
    /// </summary>
    [Serializable]
    public class Artifact {
        public FileRef? NameFileRef { get; set; }
        public FileRef? DescriptionFileRef { get; set; }
        public ArtifactType Type { get; set; }
        public ArtifactSlot Slot { get; set; }
        public FileRef? Icon { get; set; }
        public int CostOfGold { get; set; }
    }

    /// <summary>
    /// Форма информации об артефакте для бд.
    /// </summary>
    [Serializable]
    public class ArtifactDataModel {
        public string? Id { get; set; }
        public string? NamePath { get; set; }
        public string? Name { get; set; }
        public string? DescPath { get; set; }
        public string? Desc { get; set; }
        public ArtifactType Type { get; set; }
        public ArtifactSlot Slot { get; set; }
        public string? IconPath { get; set; }
        public string? Icon { get; set; }
        public int Cost { get; set; }
    }
}
