using Homm5Parser;
using Homm5Parser.Entities;
using Newtonsoft.Json;

IReadOnlyList<string> _fileDirectories = new List<string>() {
    "data", "Maps", "UserMODs"
};

IReadOnlyList<string> _hommFilesExtensions = new List<string>() {
    ".pak", ".h5m", ".h5c", ".h5u"
};

string appPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", string.Empty);

Console.WriteLine("Укажите путь к папке героев: ");
string? hommPath = Console.ReadLine();
Paths.HommData = hommPath + "data\\";

//List<FileInfo> fileInfos = new List<FileInfo>();
//foreach (string directory in _fileDirectories) {
//    string dirPath = $"{hommPath}{directory}\\";
//    foreach (FileInfo fileInfo in new DirectoryInfo(dirPath).GetFiles()) {
//        if (_hommFilesExtensions.Contains(fileInfo.Extension)) {
//            fileInfos.Add(fileInfo);
//        }
//    }
//}
//Parser parser = new Parser();
//parser.Run(fileInfos);

List<SkillDataModel> skillData = JsonConvert.DeserializeObject<List<SkillDataModel>>(File.ReadAllText($"{Paths.HommData}skills.json"))!;
if (skillData is not null) {
    IEnumerable<string> baseSkills = from SkillDataModel skill in skillData
                                     where skill.BasicSkill == "HERO_SKILL_NONE" && skill.Id != "HERO_SKILL_NONE" && skill.Names.Count > 1
                                     select skill.Id;
    if (baseSkills is not null) {
        List<WheelSkillModel> wheelSkills = new List<WheelSkillModel>();
        foreach (string baseSkill in baseSkills) {
            SkillDataModel skillModel = skillData.Find(s => s.Id == baseSkill)!;
            if (skillModel is not null) {
                int skillCount = 0;
                int skillLevels = skillModel.Names.Count;
                string skillPrefix = baseSkill.Replace("HERO_SKILL_", string.Empty);
                for (int i = 0; i < skillLevels; i++) {
                    skillCount++;
                    wheelSkills.Add(new WheelSkillModel() {
                        Id = $"{skillPrefix}_0{skillCount}",
                        Name = skillModel.NamesPaths[i],
                        Desc = skillModel.DescsPaths[i],
                        Icon = skillModel.IconsPaths[i],
                    });
                }
                IEnumerable<SkillDataModel> dependedSkills = from skill in skillData
                                                             where skill.BasicSkill == baseSkill
                                                             select skill;
                if (dependedSkills is not null) {
                    foreach (SkillDataModel dependedSkill in dependedSkills) {
                        skillCount++;
                        string prefix = skillCount >= 10 ? string.Empty : "0";
                        wheelSkills.Add(new WheelSkillModel() {
                            Id = $"{skillPrefix}_{prefix}{skillCount}",
                            Name = dependedSkill.NamesPaths[0],
                            Desc = dependedSkill.DescsPaths[0],
                            Icon = dependedSkill.IconsPaths.Last()
                        });
                    }
                }
            }
        }
        Console.WriteLine(wheelSkills.Count);
        string s = JsonConvert.SerializeObject(wheelSkills, Formatting.Indented);
        File.WriteAllText($"{Paths.HommData}wheel_skills.json", s);
    }
}
