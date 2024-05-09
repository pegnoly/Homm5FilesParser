using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Homm5Parser.Common {

    /// <summary>
    /// Типы городов
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TownType {
        TOWN_NO_TYPE,
        TOWN_HEAVEN,
        TOWN_INFERNO,
        TOWN_NECROMANCY,
        TOWN_ACADEMY,
        TOWN_DUNGEON,
        TOWN_PRESERVE,
        TOWN_FORTRESS,
        TOWN_STRONGHOLD
    }

    /// <summary>
    /// Типы игроков
    /// </summary>
    [Serializable]
    public enum PlayerID {
        PLAYER_NONE,
        PLAYER_1,
        PLAYER_2,
        PLAYER_3,
        PLAYER_4,
        PLAYER_5,
        PLAYER_6,
        PLAYER_7,
        PLAYER_8
    }
}
