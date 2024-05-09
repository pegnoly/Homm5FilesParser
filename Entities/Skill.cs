using Homm5Parser.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;

namespace Homm5Parser.Entities {
    /// <summary>
    /// Типы мастерства скилла
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Mastery {
        MASTERY_NONE,
        MASTERY_BASIC,
        MASTERY_ADVANCED,
        MASTERY_EXPERT,
        MASTERY_EXTRA_EXPERT
    }

    [Serializable]
	public class SkillPrerequisite {
		public string? Class { get; set; }
		[XmlArrayItem("Item")]
        public List<string>? dependenciesIDs { get; set; }
    }

    /// <summary>
    /// Форма информации о скилле героя для игры
    /// </summary>
    [Serializable]
	public class Skill {
        [XmlArrayItem("Item")]
        public List<FileRef>? Texture { get; set; }
        [XmlArrayItem("Item")]
		public List<FileRef>? NameFileRef { get; set; }
        [XmlArrayItem("Item")]
        public List<FileRef>? DescriptionFileRef { get; set; }
        public string? SkillType { get; set; }
        public string? HeroClass { get; set; }
		public string? BasicSkillID { get; set; }
		[XmlArrayItem("Item")]
		public List<SkillPrerequisite>? SkillPrerequisites { get; set; }
    }

    /// <summary>
    /// Форма информации о скилле героя для бд
    /// </summary>
    public class SkillDataModel {
		public string? Id { get; set; }
        // может быть несколько описаний/иконок, т.к. есть базовые/расовые скиллы с несколькими уровнями прокачки
        public List<string> IconsPaths { get; set; } = new List<string>();
        public List<string> Icons { get; set; } = new List<string>();
        public List<string> NamesPaths { get; set; } = new List<string>();
        public List<string> Names { get; set; } = new List<string>();
        public List<string> DescsPaths { get; set; } = new List<string>();
        public List<string> Descs { get; set; } = new List<string>();
		public string? HeroClass { get; set; }
		public string? BasicSkill { get; set; }
        public List<SkillPrerequisite> Dependencies { get; set; } = new List<SkillPrerequisite>();
	}
}
