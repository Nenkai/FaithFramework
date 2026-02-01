namespace FF16Framework.Services.GameApis.Magic.MagicFile;

/// <summary>
/// Known magic operation types with their names.
/// Operations define behaviors like trajectory, VFX, damage, etc.
/// </summary>
public static class MagicOperations
{
    // ==================== COMMONLY USED OPERATION TYPE IDS ====================
    // Use these constants for cleaner, more readable code
    
    /// <summary>Linear trajectory with slight homing</summary>
    public const int LinearHomingTrajectory = 1;
    
    /// <summary>Play VFX effect</summary>
    public const int PlayVfx = 25;
    
    /// <summary>Duration/timing operation</summary>
    public const int Duration = 35;
    
    /// <summary>Apply attack/damage to target</summary>
    public const int ApplyAttack = 40;
    
    /// <summary>Initialize the magic entity</summary>
    public const int Initialize = 51;
    
    /// <summary>Alternative VFX operation</summary>
    public const int PlayVfx2 = 87;
    
    /// <summary>On-hit callback operation</summary>
    public const int OnTargetHit = 94;
    
    /// <summary>Parabolic trajectory (like grenades)</summary>
    public const int ParabolaTrajectory = 2493;
    
    // ==================== ADDRESS-BASED NAMES (for debugging) ====================
    
    public static Dictionary<long, string> GetDefaultNames() => new()
    {
        { 0x7FF6C7A69EA0, "Operation_35 (Duration)" },
        { 0x7FF6C7A695D8, "Operation_25" },
        { 0x7FF6C7A69958, "Operation_94" },
        { 0x7FF6C7A69798, "Operation_87" },
        { 0x7FF6C7A684E8, "Operation_1841" },
        { 0x7FF6C7A69F80, "Operation_101" },
        { 0x7FF6C7A6A060, "Operation_183" },
        { 0x7FF6C7A69B18, "Operation_139" },
        { 0x7FF6C7A6A988, "Operation_4448" },
        { 0x7FF6C7A6A230, "Operation_50" },
        { 0x7FF6C7A6A310, "Operation_108" },
        { 0x7FF6C7A68960, "Operation_1587" },
        { 0x7FF6C7A67FA0, "Operation_2855" },
        { 0x7FF6C79D9860, "Operation_3790" },
        { 0x7FF6C79D9780, "Operation_3771" },
        { 0x7FF6C7A68080, "Operation_3847" },
        { 0x7FF6C7A68320, "Operation_39" },
        { 0x7FF6C79D8178, "Operation_6460" },
        { 0x7FF6C7A67C18, "Operation_4553" },
        { 0x7FF6C7A67DD8, "Operation_4446" },
    };

    // ==================== NAME LOOKUP ====================

    public static string GetName(int opType) => opType switch
    {
        1 => "Operation_1_LinearSlightlyHomingTrajectory",
        25 => "Operation_25_VFX",
        35 => "Operation_35",
        39 => "Operation_39",
        40 => "Operation_40_ApplyAttackMaybe",
        50 => "Operation_50",
        51 => "Operation_51_Initialize",
        86 => "Operation_86",
        87 => "Operation_87_VFX2",
        92 => "Operation_92",
        94 => "Operation_94_OnTargetHit",
        100 => "Operation_100",
        101 => "Operation_101",
        103 => "Operation_103",
        108 => "Operation_108",
        109 => "Operation_109",
        111 => "Operation_111",
        116 => "Operation_116",
        121 => "Operation_121",
        128 => "Operation_128",
        133 => "Operation_133",
        135 => "Operation_135",
        137 => "Operation_137",
        138 => "Operation_138",
        139 => "Operation_139",
        141 => "Operation_141",
        144 => "Operation_144",
        151 => "Operation_151",
        153 => "Operation_153",
        157 => "Operation_157",
        169 => "Operation_169",
        171 => "Operation_171",
        182 => "Operation_182",
        183 => "Operation_183",
        186 => "Operation_186",
        189 => "Operation_189",
        788 => "Operation_788",
        798 => "Operation_798",
        990 => "Operation_990",
        1470 => "Operation_1470",
        1562 => "Operation_1562",
        1587 => "Operation_1587",
        1623 => "Operation_1623",
        1685 => "Operation_1685",
        1825 => "Operation_1825",
        1841 => "Operation_1841",
        1842 => "Operation_1842",
        2325 => "Operation_2325",
        2366 => "Operation_2366",
        2493 => "Operation_2493_ParabolaTrajectory",
        2592 => "Operation_2592",
        2598 => "Operation_2598",
        2855 => "Operation_2855",
        3070 => "Operation_3070",
        3176 => "Operation_3176",
        3270 => "Operation_3270",
        3294 => "Operation_3294",
        3391 => "Operation_3391",
        3433 => "Operation_3433",
        3436 => "Operation_3436",
        3515 => "Operation_3515",
        3586 => "Operation_3586",
        3558 => "Operation_3558",
        3643 => "Operation_3643",
        3660 => "Operation_3660",
        3771 => "Operation_3771",
        3781 => "Operation_3781",
        3790 => "Operation_3790",
        3847 => "Operation_3847",
        3877 => "Operation_3877",
        3910 => "Operation_3910",
        3940 => "Operation_3940",
        3941 => "Operation_3941",
        4015 => "Operation_4015",
        4106 => "Operation_4106",
        4274 => "Operation_4274",
        4344 => "Operation_4344",
        4446 => "Operation_4446",
        4448 => "Operation_4448",
        4516 => "Operation_4516",
        4553 => "Operation_4553",
        4784 => "Operation_4784",
        4828 => "Operation_4828",
        4998 => "Operation_4998",
        5027 => "Operation_5027",
        5035 => "Operation_5035",
        5059 => "Operation_5059",
        5068 => "Operation_5068",
        5133 => "Operation_5133",
        5224 => "Operation_5224",
        5231 => "Operation_5231",
        5321 => "Operation_5321",
        5476 => "Operation_5476",
        5643 => "Operation_5643",
        5722 => "Operation_5722",
        5964 => "Operation_5964",
        6024 => "Operation_6024",
        6118 => "Operation_6118",
        6264 => "Operation_6264",
        6453 => "Operation_6453",
        6460 => "Operation_6460",
        6473 => "Operation_6473",
        6846 => "Operation_6846",
        6931 => "Operation_6931",
        6942 => "Operation_6942",
        7038 => "Operation_7038",
        7187 => "Operation_7187",
        7292 => "Operation_7292",
        7378 => "Operation_7378",
        7453 => "Operation_7453",
        7481 => "Operation_7481",
        7584 => "Operation_7584",
        7732 => "Operation_7732",
        7771 => "Operation_7771",
        7801 => "Operation_7801",
        7808 => "Operation_7808",
        _ => $"Operation_{opType}"
    };
}
