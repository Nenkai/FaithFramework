namespace FF16Framework.Services.GameApis.Magic.MagicFile;

/// <summary>
/// Known Magic IDs for various Eikons and abilities.
/// </summary>
public static class MagicIds
{
    public struct MagicEntry
    {
        public int Id;
        public string Name;
        public MagicEntry(int id, string name) { Id = id; Name = name; }
    }

    public static class Ifrit
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(524, "Will O' The Wykes") 
        };
    }

    public static class Phoenix
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(1, "Fire"),
            new(212, "Fire (unused fire? SFX missing)"),
            new(4, "Fira (base)"),
            new(1564, "Fira (upgraded)"),
            new(348, "Fira  (precision shot)"),
            new(218, "Fira (unused fire? SFX missing)"),
            new(353, "Fira (unused fire? SFX missing)"),
            new(1569, "Fira  (unused fire? SFX missing)"),
            new(912, "Heatwave (base - 1st slash)"),
            new(913, "Heatwave (base - 2nd slash counter)"),
            new(914, "Heatwave (upgrade - 1st slash)"),
            new(915, "Heatwave (upgrade - 2nd slash)"),
            new(916, "Heatwave (upgrade - 3rd slash counter)"),
            new(917, "Heatwave (upgrade - 4th slash counter)"),
            new(1145, "Flames of Rebirth (base - 1)"),
            new(1146, "Flames of Rebirth (upgraded - 1)"),
            new(1147, "Flames of Rebirth (upgraded - 2)"),
            new(1144, "Flames of Rebirth (base/upgraded - end hit)")
        };
    }

    public static class Garuda
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(2, "Aero"),
            new(5, "Aerora (base)"),
            new(1565, "Aerora (upgraded)"),
            new(349, "Aerora (precision shot)"),
            new(7, "Deadly Embrace (base)"),
            new(1321, "Deadly Embrace (upgraded)"),
            new(1997, "Deadly Embrace (duplicated?)"),
            new(1998, "Deadly Embrace (duplicated?)"),
            new(457, "Aerial Blast (base)"),
            new(480, "Aerial Blast (upgraded)")
        };
    }

    public static class Titan
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(3, "Stone"),
            new(6, "Stonera (base)"),
            new(1566, "Stonera (upgraded)"),
            new(350, "Stonera (precision shot)"),
            new(500, "Earthen Fury (base - End2End)"),
            new(995, "Earthen Fury (upgraded - End2End)"),
            new(1154, "Earthen Fury (1)"),
            new(1155, "Earthen Fury (2)"),
            new(1153, "Earthen Fury (3)"),
            new(1552, "Earthen Fury (small single pilar)"),
            new(1553, "Earthen Fury (medium single pilar)"),
            new(1554, "Earthen Fury (big single pilar)"),
            new(1555, "Earthen Fury (biggest single pilar)"),
        };
    }

    public static class Ramuh
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(210, "Thunder"),
            new(1567, "Thundara"),
            new(351, "Thundara (precision shot)"),
            new(216, "Thundara (duplicate?)"),
            new(670, "Blind Justice (uncharged - master?)"),
            new(669, "Blind Justice (uncharged - slave?)"),
            new(672, "Blind Justice (base - master?)"),
            new(671, "Blind Justice (base - slave?)"),
            new(674, "Blind Justice (upgraded - master?)"),
            new(673, "Blind Justice (upgraded - slave?)"),
            new(652, "Thunderstorm (base - End2End)"),
            new(646, "Thunderstorm (base - thunderbolts)"),
            new(804, "Thunderstorm (base - thunderbolts alt)"),
            new(805, "Thunderstorm (base - final thunderbolt)"),
            new(653, "Thunderstorm (upgraded - End2End)"),
            new(649, "Thunderstorm (upgraded - thunderbolts)"),
            new(806, "Thunderstorm (upgraded - thunderbolts alt)"),
            new(807, "Thunderstorm (upgraded - final thunderbolt)"),
            new(821, "Lightning Rod (base - object)"),
            new(819, "Lightning Rod (base - proc player hit)"),
            new(1102, "Lightning Rod (base - proc enemy hit)"),
            new(822, "Lightning Rod (upgraded - object)"),
            new(820, "Lightning Rod (upgraded - proc player hit)"),
            new(1103, "Lightning Rod (upgraded - proc enemy hit)"),
            new(1024, "Judgement Bolt (base - hit)"),
            new(1025, "Judgement Bolt (upgraded - 1st hit)"),
            new(1026, "Judgement Bolt (upgraded - 2nd hit)")
        };
    }

    public static class Bahamut
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(214, "Dia"),
            new(220, "Diara (base)"),
            new(1571, "Diara (upgraded)"),
            new(355, "Diara (precision shot)"),
            new(1083, "Megaflare (lvl 1)"),
            new(1084, "Megaflare (lvl 2)"),
            new(1085, "Megaflare (lvl 3)"),
            new(1086, "Megaflare (lvl 4)"),
            new(623, "Gigaflare (GAME CRASHES)"),
            new(631, "Impulse (1st projectile - base)"),
            new(632, "Impulse (2nd projectile - base)"),
            new(633, "Impulse (3rd projectile - base - not used?)"),
            new(634, "Impulse (4th projectile - base - not used?)"),
            new(702, "Impulse (projectile orbit enemy)"),
            new(815, "Impulse (1st projectile - upgraded)"),
            new(816, "Impulse (2nd projectile - upgraded)"),
            new(817, "Impulse (3rd projectile - upgraded)"),
            new(818, "Impulse (4th projectile - upgraded)"),
            new(452, "Satellite Dia (base)"),
            new(1108, "Satellite Dia (upgraded - L)"),
            new(1109, "Satellite Dia (upgraded - R)"),
            new(1558, "Satellite Magic Burst (base)"),
            new(1559, "Satellite Magic Burst (upgraded - R)"),
            new(1560, "Satellite Magic Burst (upgraded - L)"),
            new(1185, "Satellite Diara (base)"),
            new(1186, "Satellite Diara (upgraded - R)"),
            new(1187, "Satellite Diara (upgraded - L)"),
            new(1188, "Satellite Diara (base - precision shot)"),
            new(1189, "Satellite Diara (upgraded - R - precision shot)"),
            new(1190, "Satellite Diara (upgraded - L - precision shot)"),
            new(582, "Flare Breath (stream - base)"),
            new(583, "Flare Breath (stream - upgraded)"),
            new(1360, "Flare Breath (last hit)")
        };
    }

    public static class Shiva
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(213, "Blizzard"),
            new(219, "Blizzara (base)"),
            new(1570, "Blizzara (upgraded)"),
            new(354, "Blizzara (precision shot)"),
            new(992, "Cold Snap (base - frostbite)"),
            new(1999, "Cold Snap (upgraded - frostbite not used ingame?)"),
            new(624, "Ice Age (undershooted)"),
            new(627, "Ice Age (overshooted)"),
            new(1543, "Ice Age (timed)"),
            new(620, "Mesmerized (individual projectile)"),
            new(621, "Rime (base)"),
            new(1604, "Rime (base - recast)"),
            new(981, "Rime (upgraded)"),
            new(1605, "Rime (upgraded - recast)"),
            new(625, "Diamond Dust (1st hit)"),
            new(982, "Diamond Dust (2nd hit)")
        };
    }

    public static class Odin
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(211, "Dark"),
            new(217, "Darkra (base)"),
            new(1568, "Darkra (upgraded)"),
            new(352, "Darkra (precision shot)"),
        };
    }

    public static class Leviathan
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(215, "Water"),
            new(221, "Watera (base)"),
            new(1572, "Watera (upgraded)"),
            new(356, "Watera (precision shot)"),
            new(1574, "Tidal Torrent"),
            new(1575, "Charged Torrent"),
            new(1607, "Precision Torrent"),
            new(1576, "Tidal Stream"),
            new(1943, "Charged Stream"),
            new(1608, "Precision Stream"),
            new(1577, "Tidal Bomb"),
            new(1578, "Deluge (stream)"),
            new(1792, "Deluge (end)"),
            new(1579, "Cross Swell (base)"),
            new(1580, "Cross Swell (upgraded)"),
            new(1621, "Abyssal Tear (vent lvl 1)"),
            new(1650, "Abyssal Tear (vent lvl 2)"),
            new(1651, "Abyssal Tear (vent lvl 3)"),
            new(1652, "Abyssal Tear (vent lvl 4)"),
            new(838, "Tsunami (base)"),
            new(983, "Tsunami (upgraded - 1)"),
            new(1811, "Tsunami (upgraded - 2)")
        };
    }

    public static class Ultima
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(1561, "Ruin"),
            new(1562, "Ruinra (base)"),
            new(1573, "Ruinra (upgraded)"),
            new(1563, "Ruinra (precision shot)"),
            new(1697, "Purge (ascend mode - burning blade)"),
            new(1646, "Rampant Ruin (ascend mode - ruin)"),
            new(1647, "Rampant Ruinra (base - ascend mode - ruinra)"),
            new(1648, "Rampant Ruinra (upgrade - ascend mode - ruinra)"),
            new(1599, "Ruination (ascend mode - precision shot)"),
            new(1766, "Ruination (ascend mode - precision shot - duplicated?)"),
            new(1612, "Proselytize"),
            new(1721, "Dominion (base)"),
            new(1722, "Dominion (upgraded)"),
            new(1609, "Voice of God"),
            new(1723, "Ultimate Demise (casting)"),
            new(1610, "Ultimate Demise (AoE)"),
        };
    }

    public static class Unknown
    {
        public static readonly List<MagicEntry> All = new() 
        { 
            new(607, "Unknown (GAME CRASHES)"),
            new(1602, "Unknown (Ramuh related?)"),
            new(1603, "Unknown (Ramuh related?)")

        };
    }
}
