using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public enum BotLevel
    {
        Easy,
        Medium,
        Hard,
        Extreme,
        Deadly
    }

    public enum Map
    {
        Outpost

    }

    public class SettingsHolder
    {
        public static BotLevel botLevel = BotLevel.Medium;
        public static Map map = Map.Outpost;
    }
}
