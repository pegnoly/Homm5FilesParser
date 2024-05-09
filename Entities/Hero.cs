using Homm5Parser.Common;
using System.Xml.Serialization;

namespace Homm5Parser.Entities {

    [Serializable]
    public class HeroSkill {
        public Mastery Mastery { get; set; }
        public string? SkillID { get; set; }
    }
    /// <summary>
    /// Форма информации для заполнения тега Editable у героя в игре
    /// </summary>
    [Serializable]
    public class Editable {
        public FileRef? NameFileRef { get; set; }
        public FileRef? BiographyFileRef { get; set; }
        public int Offence { get; set; }
        public int Defence { get; set; }
        public int Spellpower { get; set; }
        public int Knowledge { get; set; }
        [XmlArrayItem("Item")]
        public List<HeroSkill> skills { get; set; } = new List<HeroSkill>();
        [XmlArrayItem("Item")]
        public List<string> perkIDs { get; set; } = new List<string>();
        [XmlArrayItem("Item")]
        public List<string> spellIDs { get; set; } = new List<string>();
        public bool Ballista { get; set; }
        public bool FirstAidTent { get; set; }
        public bool AmmoCart { get; set; }
        [XmlArrayItem("Item")]
        public List<string> FavoriteEnemies { get; set; } = new List<string>();
        public int TalismanLevel { get; set; } = 0;
    }

    /// <summary>
    /// Форма информации о герое для файлов игры
    /// </summary>
    [Serializable]
    public class AdvMapHeroShared {
        public string? InternalName { get; set; }
        public string? Class { get; set; }
        public string? Specialization { get; set; }
        public HeroSkill? PrimarySkill { get; set; }
        public FileRef? SpecializationNameFileRef { get; set; }
        public FileRef? SpecializationDescFileRef { get; set; }
        public FileRef? SpecializationIcon { get; set; }
        public FileRef? FaceTexture { get; set; }
        public TownType TownType { get; set; }
        public Editable? Editable { get; set; }
    }

    /// <summary>
    /// Форма информации о герое для бд
    /// </summary>
    [Serializable]
    public class HeroDataModel {
        public string? Xdb { get; set; }
        public string? ClassId { get; set; }
        public string? SpecId { get; set; }
        public HeroSkill? PrimarySkill { get; set; }
        public string? SpecNamePath { get; set; }
        public string? SpecName { get; set; }
        public string? SpecDescPath { get; set; }
        public string? SpecDesc { get; set; }
        public string? SpecIconPath { get; set; }
        public string? SpecIcon { get; set; }
        public string? IconPath { get; set; }
        public string? Icon { get; set; }
        public TownType Town { get; set; }
        public string? NamePath { get; set; }
        public string? Name { get; set; }
        public string? BioPath { get; set; }
        public string? Bio { get; set; }
        public List<HeroSkill>? BaseSkills { get; set; }
        public List<string>? BasePerks { get; set; }
        public List<string>? BaseSpells { get; set; }
    }

}
