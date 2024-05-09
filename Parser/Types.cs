//using DuelPresetsGenerator.Common.Objects;
//using DuelPresetsGenerator.Entities.Creature;
//using Microsoft.Data.Sqlite;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using System.Xml.Serialization;

//namespace DuelPresetsGenerator.Parsers {
//    public class DbID {
//        public string? XPointer { get; set; }
//    }

//    /// <summary>
//    /// Представляет экземпляр таблицы, записанной в types.xml
//    /// </summary>
//    public class Table {
//        public DbID? dbid { get; set; }
//        [XmlArrayItem("Item")]
//        public List<string>? EnumEntries { get; set; }
//    }

//    [XmlRoot("Base")]
//    public class H5Types {
//        [XmlArrayItem("Item")]
//        public List<Table>? Tables { get; set; }
//    }

//    public class DBObject<T> {
//        public T? ID { get; set; }
//        public FileRef? Obj;
//    }

//    public class Table_Creature_CreatureType {
//        [XmlArrayItem("Item")]
//        public List<DBObject<CreatureType>>? objects { get; set; }
//    }

//    public class TypesXMLParser {

//        private readonly string CreaturesTableXPointer = "/GameMechanics/RefTables/Creatures.xdb#xpointer(/Table_Creature_CreatureType)";
//        private readonly string ArtifactsTableXPointer = "/GameMechanics/RefTables/Artifacts.xdb#xpointer(/Table_DBArtifact_ArtifactEffect)";
//        private readonly string SpellsTableXPointer = "/GameMechanics/RefTables/UndividedSpells.xdb#xpointer(/Table_Spell_SpellID)";
//        private readonly string SkillsTableXPointer = "/GameMechanics/RefTables/Skills.xdb#xpointer(/Table_HeroSkill_SkillID)";

//        private readonly string DbPath = "D:\\Users\\pgn\\source\\DuelPresetsGenerator\\database.db";

//        public void Parse(string fileName) {
//            XmlSerializer ds = new XmlSerializer(typeof(H5Types));
//            XDocument doc = XDocument.Parse(File.ReadAllText(fileName));
//            H5Types h5types = (H5Types)ds.Deserialize(doc.CreateReader())!;
//            GenerateCreatureIDsDataBase(h5types);
//        }

//        private void GenerateCreatureIDsDataBase(H5Types types) {
//            Table creatureIDsTable = (from Table t in types.Tables!
//                                      where t.dbid!.XPointer == CreaturesTableXPointer
//                                      select t).First();
//            string commandString = "BEGIN TRANSACTION; CREATE TABLE CreatureIDs(Id INTEGER PRIMARY KEY UNIQUE NOT NULL, Type TEXT); ";
//            int id = 0;
//            foreach (string entry in creatureIDsTable.EnumEntries!) {
//                commandString += $"INSERT INTO CreatureIDs(Id, Type) VALUES({id}, \"{entry}\"); ";
//                id++;
//            }
//            commandString += "CREATE TABLE Creatures(Id INTEGER PRIMARY KEY UNIQUE NOT NULL, Grow INTEGER NOT NULL, Town TEXT NOT NULL, Tier int NOT NULL); ";

//            commandString += "COMMIT;";
//            using (SqliteConnection connection = new SqliteConnection($"Data Source={DbPath}")) {
//                connection.Open();
//                SqliteCommand cmd = new SqliteCommand(commandString, connection);
//                cmd.ExecuteNonQuery();
//            }


//        }
//    }
//}
