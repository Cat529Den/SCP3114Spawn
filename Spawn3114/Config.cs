using System.ComponentModel;

namespace Spawn3114
{
    public class Config
    {
        [Description("啟用插件嗎? Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("生成的3114會替代1名人類玩家嗎? Overrides the spawn queue of human players.")]
        public bool Spawn3114OverridesHumanQueue = false;

        [Description("生成3114的機率 Chance to spawn 3114.")]
        public float Spawn3114Chance { get; set; } = 0.5f;

        [Description("生成3114的最少人數 Minimal players to spawn 3114.")]
        public int Spawn3114MinPlayers { get; set; } = 1;

    }
}