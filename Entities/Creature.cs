using Homm5Parser.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;

namespace Homm5Parser.Entities {
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MagicElementType {
        ELEMENT_NONE,
        ELEMENT_FIRE,
        ELEMENT_AIR,
        ELEMENT_WATER,
        ELEMENT_EARTH
    }

    [Serializable]
    public class MagicElement {
        public MagicElementType First { get; set; }
        public MagicElementType Second { get; set; }
    }

    [Serializable]
    public class ResourceSet {
        public int Wood { get; set; }
        public int Ore { get; set; }
        public int Mercury { get; set; }
        public int Crystal { get; set; }
        public int Sulfur { get; set; }
        public int Gem { get; set; }
        public int Gold { get; set; }
    }

    [Serializable]
    public class CreatureKnownSpell {
        public string? Spell { get; set; }
        public Mastery Mastery { get; set; }
    }

    [Serializable]
    public class CreatureVisual {
        public FileRef? CreatureNameFileRef { get; set; }
        public FileRef? DescriptionFileRef { get; set; }
        public FileRef? Icon128 { get; set; }
    }

    /// <summary>
    /// Форма информации о существе для файлов игры.
    /// </summary>
    [Serializable]
    public class Creature {
        public int AttackSkill { get; set; }
        public int DefenceSkill { get; set; }
        public int Shots { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Speed { get; set; }
        public int Initiative { get; set; }
        public int Health { get; set; }
        [XmlArrayItem("Item")]
        public List<CreatureKnownSpell> KnownSpells { get; set; } = new List<CreatureKnownSpell>();
        public int SpellPoints { get; set; }
        public int Exp { get; set; }
        public int Power { get; set; }
        public int CreatureTier { get; set; }
        public TownType CreatureTown { get; set; }
        public MagicElement? MagicElement { get; set; } = new MagicElement();
        public int WeeklyGrowth { get; set; }
        public ResourceSet Cost { get; set; } = new ResourceSet();
        public int CombatSize { get; set; }
        public FileRef? Visual { get; set; }
        public int Range { get; set; }
        public string? BaseCreature { get; set; }
        [XmlArrayItem("Item")]
        public List<string>? Abilities { get; set; }
    }

    /// <summary>
    /// Форма информации о cуществе для бд.
    /// </summary>
    [Serializable]
    public class CreatureDataModel {
        public string? Id { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Shots { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Speed { get; set; }
        public int Initiative { get; set; }
        public int Health { get; set; }
        public List<CreatureKnownSpell> KnownSpells { get; set; } = new List<CreatureKnownSpell>();
        public int ManaPoints { get; set; }
        public int Experience { get; set; }
        public int Power { get; set; }
        public int Level { get; set; }
        public TownType Town { get; set; }
        // элементы для цепочек лиги
        public MagicElementType FirstElement { get; set; }
        public MagicElementType SecondElement { get; set; }
        public int Grow { get; set; }
        // для т7 существ, цена может быть не только в голде, но и в ресах
        public ResourceSet? Cost { get; set; }
        public int Size { get; set; }
        /// <summary>
        /// Путь к файлу имени в игре.
        /// </summary>
        public string? NamePath { get; set; }
        /// <summary>
        /// Путь к файлу описания в игре.
        /// </summary>
        public string? DescPath { get; set; }
        /// <summary>
        /// Путь к файлу иконки в игре.
        /// </summary>
        public string? IconPath { get; set; }
        /// <summary>
        /// Текстовое представление имени
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Текстовое представление описания
        /// </summary>
        public string? Desc { get; set; }
        /// <summary>
        /// Путь к файлу иконки, сконвертированной в пнг
        /// </summary>
        public string? Icon { get; set; }
        public int Range { get; set; }
        public string? BaseCreature { get; set; }
        public List<string> Abilities { get; set; } = new List<string>();
    }
}
