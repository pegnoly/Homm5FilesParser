using Homm5Parser.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;

namespace Homm5Parser.Entities {

    /// <summary>
    /// Магические школы.
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MagicSchool {
        MAGIC_SCHOOL_DESTRUCTIVE,
        MAGIC_SCHOOL_DARK,
        MAGIC_SCHOOL_LIGHT,
        MAGIC_SCHOOL_SUMMONING,
        MAGIC_SCHOOL_ADVENTURE,
        MAGIC_SCHOOL_RUNIC,
        MAGIC_SCHOOL_WARCRIES,
        MAGIC_SCHOOL_SPECIAL,
        MAGIC_SCHOOL_NONE
    }

    /// <summary>
    /// Форма информации о спелле для файлов игры
    /// </summary>
    [Serializable]
    public class Spell {
        public FileRef? NameFileRef { get; set; }
        public FileRef? LongDescriptionFileRef { get; set; }
        public FileRef? Texture { get; set; }
        public int Level { get; set; }
        public MagicSchool MagicSchool { get; set; }
        public int RequiredHeroLevel { get; set; }
        public int TrainedCost { get; set; }
        [XmlArrayItem("Item")]
        public List<SpellFormulaInfo>? damage { get; set; }
        [XmlArrayItem("Item")]
        public List<SpellFormulaInfo>? duration { get; set; }
        public ResourceSet? sSpellCost { get; set; }
    }

    [Serializable]
    public class SpellFormulaInfo {
        public float Base { get; set; }
        public float PerPower { get; set; }
    }

    /// <summary>
    /// Форма информации о спелле для бд
    /// </summary>
    [Serializable]
	public class SpellDataModel {
		public string? Id { get; set; }
        /// <summary>
        /// Путь к файлу имени в игре
        /// </summary>
        public string? NamePath { get; set; }
        /// <summary>
        /// Текстовое представление имени
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Путь к файлу описания в игре
        /// </summary>
        public string? DescPath { get; set; }
        /// <summary>
        /// Текстовое представление описания
        /// </summary>
        public string? Desc { get; set; }
        /// <summary>
        /// Путь к файлу иконки в игре
        /// </summary>
        public string? IconPath { get; set; }
        /// <summary>
        /// Путь к пнг файлу иконки
        /// </summary>
        public string? Icon { get; set; }
        public int Level { get; set; }
		public MagicSchool School { get; set; }
        /// <summary>
        /// Актуально для кличей и походок
        /// </summary>
        public int HeroLevelToLearn { get; set; }
        public int Manacost { get; set; }
        // Зависимости от колды эффектов спелла. В самих файлах это damage и duration, но для разных спеллов разные эффекты, так что хз, юзабельно или нет.
        public List<SpellFormulaInfo>? FirstSpDependedParam { get; set; }
        public List<SpellFormulaInfo>? SecondSpDependedParam { get; set; }
        /// <summary>
        /// Стоимость в ресах(актуально для рун)
        /// </summary>
        public ResourceSet? ResourceCost { get; set; }
	}
}
