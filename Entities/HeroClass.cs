using Homm5Parser.Common;
using System.Xml.Serialization;

namespace Homm5Parser.Entities {

    [Serializable]
    public class SkillProb {
        public string? SkillID { get; set; }
        public int Prob { get; set; }
    }

    [Serializable]
    public class AttributeProbs {
        public int OffenceProb { get; set; }
        public int DefenceProb { get; set; }
        public int SpellpowerProb { get; set; }
        public int KnowledgeProb { get; set; }
    }

    [Serializable]
    public class HeroClass {
        public FileRef? Name { get; set; }
        [XmlArrayItem("Item")]
        public List<SkillProb>? SkillsProbs { get; set; }
        public AttributeProbs? AttributesProbs { get; set; }
    }

    public class HeroClassDataModel {
        public string? Id { get; set; }
        public string? NamePath { get; set; }
        public string? Name { get; set; }
        public List<SkillProb>? SkillsProbs { get; set; }
        public AttributeProbs? AttributesProbs { get; set; }
    }
}
