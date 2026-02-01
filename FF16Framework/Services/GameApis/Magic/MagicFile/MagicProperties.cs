namespace FF16Framework.Services.GameApis.Magic.MagicFile;

/// <summary>
/// Enumeration of property value types used in magic properties.
/// </summary>
public enum MagicValueType
{
    Int,
    Float,
    Vec3Float,
    Vec3Int,
    Bool
}

/// <summary>
/// Information about a magic property including its name and type.
/// </summary>
public record MagicPropertyInfo(string Name, MagicValueType Type);

/// <summary>
/// Known magic property IDs with their names and types.
/// Properties define configuration values for operations.
/// </summary>
public static class MagicProperties
{
    // ==================== COMMONLY USED PROPERTY IDS ====================
    // Use these constants for cleaner, more readable code
    
    /// <summary>Projectile speed multiplier (float)</summary>
    public const int Speed = 8;
    
    /// <summary>Disables tracking/homing behavior when true (bool)</summary>
    public const int NoTracking = 13;
    
    /// <summary>Vertical angle offset in degrees (int)</summary>
    public const int VerticalAngleOffset = 22;
    
    /// <summary>Operation group ID to execute (int)</summary>
    public const int OperationGroupId = 26;
    
    /// <summary>VFX/Audio ID to play (int)</summary>
    public const int VfxAudioId = 27;
    
    /// <summary>Disappear after duration expires (bool)</summary>
    public const int DisappearAfterDuration = 30;
    
    /// <summary>VFX scale multiplier (float)</summary>
    public const int VfxScale = 31;
    
    /// <summary>Duration in seconds before expiration (float)</summary>
    public const int Duration = 35;
    
    /// <summary>Operation group to execute when projectile expires (int)</summary>
    public const int OnExpireOperationGroupId = 37;
    
    /// <summary>Attack parameter ID for damage calculation (int)</summary>
    public const int AttackParamId = 41;
    
    /// <summary>Hitbox/attachment size (float)</summary>
    public const int HitboxSize = 42;
    
    /// <summary>Location spawn type ID (int)</summary>
    public const int SpawnLocationType = 73;
    
    /// <summary>EID reference (int)</summary>
    public const int EidId = 81;
    
    /// <summary>Alternative VFX/Audio ID (int)</summary>
    public const int VfxAudioId2 = 89;
    
    /// <summary>Operation group to execute on target hit (int)</summary>
    public const int OnTargetHitOperationGroupId = 95;
    
    /// <summary>Third VFX/Audio ID slot (int)</summary>
    public const int VfxAudioId3 = 105;
    
    /// <summary>Magic ID to spawn (int)</summary>
    public const int SpawnMagicId = 147;
    
    /// <summary>Second magic ID to spawn (int)</summary>
    public const int SpawnMagicId2 = 148;
    
    /// <summary>Third magic ID to spawn (int)</summary>
    public const int SpawnMagicId3 = 149;
    
    /// <summary>Type of trajectory ID (int)</summary>
    public const int TrajectoryType = 187;
    
    /// <summary>Scaled duration - alternative to Duration (float)</summary>
    public const int DurationScaled = 1379;
    
    /// <summary>Layout instance ID (int)</summary>
    public const int LayoutInstanceId = 1458;
    
    /// <summary>Horizontal angle (float)</summary>
    public const int HorizontalAngle = 1999;
    
    /// <summary>Vertical angle (float)</summary>
    public const int VerticalAngle = 2000;
    
    /// <summary>Trajectory rotation variables (Vector3)</summary>
    public const int TrajectoryRotation = 2430;
    
    /// <summary>Trajectory curve intensity (float)</summary>
    public const int TrajectoryCurveStrength = 2593;
    
    /// <summary>Camera F-Curve ID (int)</summary>
    public const int CameraFCurveId = 3848;
    
    /// <summary>Command ID (int)</summary>
    public const int CommandId = 5274;
    
    /// <summary>Skill upgrade level (int)</summary>
    public const int SkillUpgradeLevel = 5276;
    
    // ==================== FULL DEFINITIONS DICTIONARY ====================
    
    public static readonly Dictionary<int, MagicPropertyInfo> Definitions = new()
    {
        { 2, new("Global Property?", MagicValueType.Int) },
        { 3, new("Value", MagicValueType.Float) },
        { 4, new("Value", MagicValueType.Float) },
        { 5, new("Value", MagicValueType.Float) },
        { 6, new("Value", MagicValueType.Float) },
        { 7, new("Value", MagicValueType.Int) },
        { 8, new("Speed", MagicValueType.Float) },
        { 9, new("Value", MagicValueType.Float) },
        { 10, new("Chance", MagicValueType.Float) },
        { 11, new("Value", MagicValueType.Float) },
        { 12, new("Value", MagicValueType.Float) },
        { 13, new("NoTrackingTarget", MagicValueType.Bool) },
        { 14, new("Value", MagicValueType.Float) },
        { 16, new("BoolValue", MagicValueType.Bool) },
        { 17, new("Value", MagicValueType.Float) },
        { 18, new("BoolValue", MagicValueType.Bool) },
        { 19, new("BoolValue", MagicValueType.Bool) },
        { 22, new("Vertical Angle Degrees offset", MagicValueType.Float) },
        { 24, new("UnkType", MagicValueType.Int) },
        { 26, new("OperationGroupId", MagicValueType.Int) },
        { 27, new("VFXAudioId", MagicValueType.Int) },
        { 28, new("Value", MagicValueType.Int) },
        { 29, new("Value", MagicValueType.Int) },
        { 30, new("Disappear after duration?", MagicValueType.Bool) },
        { 31, new("VFXScale", MagicValueType.Float) },
        { 32, new("Value", MagicValueType.Float) },
        { 35, new("ProjectileDuration (s)", MagicValueType.Float) },
        { 36, new("ProjectileRandDurationMinMax", MagicValueType.Vec3Float) },
        { 37, new("OnNoImpactOperationGroupId", MagicValueType.Int) },
        { 38, new("BoolValue", MagicValueType.Bool) },
        { 39, new("OperationGroupId", MagicValueType.Int) },
        { 41, new("AttackParamId", MagicValueType.Int) },
        { 42, new("Hitbox/Attachment Size", MagicValueType.Float) },
        { 43, new("Rate (Speed)", MagicValueType.Float) },
        { 44, new("Target", MagicValueType.Float) },
        { 45, new("Type", MagicValueType.Int) },
        { 46, new("Value", MagicValueType.Float) },
        { 47, new("Value", MagicValueType.Float) },
        { 48, new("Value", MagicValueType.Float) },
        { 49, new("Value", MagicValueType.Float) },
        { 52, new("Value", MagicValueType.Vec3Float) },
        { 53, new("Value", MagicValueType.Vec3Float) },
        { 54, new("Value", MagicValueType.Vec3Float) },
        { 55, new("Value", MagicValueType.Vec3Float) },
        { 56, new("Value", MagicValueType.Vec3Float) },
        { 57, new("Value", MagicValueType.Vec3Float) },
        { 58, new("Value", MagicValueType.Vec3Float) },
        { 59, new("Value", MagicValueType.Vec3Float) },
        { 60, new("Value", MagicValueType.Vec3Float) },
        { 61, new("Value", MagicValueType.Vec3Float) },
        { 62, new("Value", MagicValueType.Vec3Float) },
        { 63, new("Value", MagicValueType.Vec3Float) },
        { 64, new("Value", MagicValueType.Vec3Float) },
        { 65, new("Value", MagicValueType.Int) },
        { 66, new("Value", MagicValueType.Float) },
        { 68, new("Value", MagicValueType.Int) },
        { 69, new("Value", MagicValueType.Int) },
        { 70, new("Value", MagicValueType.Float) },
        { 71, new("Value", MagicValueType.Float) },
        { 72, new("Value", MagicValueType.Vec3Float) },
        { 73, new("Location spawn type ID", MagicValueType.Int) },
        { 74, new("Value", MagicValueType.Float) },
        { 75, new("Value", MagicValueType.Int) },
        { 76, new("Value", MagicValueType.Int) },
        { 77, new("Value", MagicValueType.Float) },
        { 78, new("Value", MagicValueType.Int) },
        { 79, new("Value", MagicValueType.Int) },
        { 80, new("Value", MagicValueType.Int) },
        { 81, new("EidId", MagicValueType.Int) },
        { 89, new("VfxAudioId", MagicValueType.Int) },
        { 90, new("Use property 91?", MagicValueType.Bool) },
        { 91, new("Value (MotionLayerSetId?)", MagicValueType.Int) },
        { 93, new("Value", MagicValueType.Float) },
        { 95, new("OnTargetHitOperationGroupIdCallback", MagicValueType.Int) },
        { 96, new("OperationGroupId", MagicValueType.Int) },
        { 97, new("Value", MagicValueType.Int) },
        { 98, new("Value", MagicValueType.Int) },
        { 99, new("Value", MagicValueType.Int) },
        { 102, new("OperationGroupId", MagicValueType.Int) },
        { 105, new("VFXAudioId", MagicValueType.Int) },
        { 109, new("Value", MagicValueType.Int) },
        { 110, new("OperationGroupId", MagicValueType.Int) },
        { 114, new("BoolValue", MagicValueType.Bool) },
        { 117, new("Value", MagicValueType.Float) },
        { 129, new("Value", MagicValueType.Float) },
        { 134, new("Value", MagicValueType.Int) },
        { 136, new("Value", MagicValueType.Int) },
        { 147, new("MagicId", MagicValueType.Int) },
        { 148, new("MagicId", MagicValueType.Int) },
        { 149, new("MagicId", MagicValueType.Int) },
        { 150, new("Value", MagicValueType.Int) },
        { 187, new("Type of trayectory ID", MagicValueType.Int) },
        { 997, new("Value", MagicValueType.Float) },
        { 1025, new("Value", MagicValueType.Int) },
        { 1026, new("Value", MagicValueType.Int) },
        { 1123, new("BoolValue", MagicValueType.Bool) },
        { 1231, new("Value", MagicValueType.Int) },
        { 1232, new("Value", MagicValueType.Int) },
        { 1379, new("ProjectileDuration (Scaled)", MagicValueType.Float) },
        { 1432, new("Value", MagicValueType.Int) },
        { 1458, new("LayoutInstanceId", MagicValueType.Int) },
        { 1484, new("Value", MagicValueType.Float) },
        { 1490, new("Value", MagicValueType.Int) },
        { 1503, new("OperationGroupId", MagicValueType.Int) },
        { 1686, new("Value", MagicValueType.Float) },
        { 1688, new("Value", MagicValueType.Float) },
        { 1791, new("Value", MagicValueType.Int) },
        { 1793, new("Value", MagicValueType.Float) },
        { 1807, new("Value", MagicValueType.Vec3Float) },
        { 1833, new("OperationGroupId", MagicValueType.Int) },
        { 1957, new("BoolValue", MagicValueType.Bool) },
        { 1994, new("Value", MagicValueType.Int) },
        { 1997, new("Value", MagicValueType.Vec3Float) },
        { 1999, new("Angle", MagicValueType.Float) },
        { 2000, new("Angle", MagicValueType.Float) },
        { 2060, new("Value", MagicValueType.Float) },
        { 2061, new("Value", MagicValueType.Int) },
        { 2062, new("Value", MagicValueType.Int) },
        { 2063, new("Value", MagicValueType.Int) },
        { 2064, new("Value", MagicValueType.Int) },
        { 2065, new("Value", MagicValueType.Int) },
        { 2066, new("Value", MagicValueType.Int) },
        { 2067, new("Value", MagicValueType.Int) },
        { 2068, new("Value", MagicValueType.Int) },
        { 2069, new("Value", MagicValueType.Int) },
        { 2211, new("Value", MagicValueType.Float) },
        { 2227, new("BoolValue", MagicValueType.Bool) },
        { 2259, new("Value", MagicValueType.Int) },
        { 2260, new("Value", MagicValueType.Int) },
        { 2319, new("Value", MagicValueType.Int) },
        { 2351, new("Value", MagicValueType.Float) },
        { 2359, new("BoolValue", MagicValueType.Bool) },
        { 2396, new("LayoutInstanceId", MagicValueType.Int) },
        { 2413, new("Value", MagicValueType.Int) },
        { 2414, new("Value", MagicValueType.Int) },
        { 2430, new("Trajectory variables (Rotation)", MagicValueType.Vec3Float) },
        { 2528, new("Value", MagicValueType.Int) },
        { 2529, new("Value", MagicValueType.Int) },
        { 2593, new("Trayectory intensity curve strength", MagicValueType.Float) },
        { 2575, new("Value", MagicValueType.Int) },
        { 2604, new("Value", MagicValueType.Int) },
        { 2672, new("Value", MagicValueType.Int) },
        { 2798, new("Value", MagicValueType.Int) },
        { 2856, new("BoolValue", MagicValueType.Bool) },
        { 2857, new("Value", MagicValueType.Float) },
        { 2906, new("Value", MagicValueType.Int) },
        { 3078, new("Value", MagicValueType.Int) },
        { 3079, new("Value", MagicValueType.Int) },
        { 3171, new("Value", MagicValueType.Float) },
        { 3172, new("Value", MagicValueType.Float) },
        { 3362, new("Value", MagicValueType.Float) },
        { 3363, new("Value", MagicValueType.Int) },
        { 3434, new("Value", MagicValueType.Int) },
        { 3440, new("OperationGroupId", MagicValueType.Int) },
        { 3441, new("OperationGroupId", MagicValueType.Int) },
        { 3497, new("Value", MagicValueType.Vec3Float) },
        { 3498, new("Value", MagicValueType.Vec3Float) },
        { 3606, new("BoolValue", MagicValueType.Bool) },
        { 3608, new("OperationGroupId", MagicValueType.Int) },
        { 3609, new("OperationGroupId", MagicValueType.Int) },
        { 3658, new("Value", MagicValueType.Int) },
        { 3659, new("Value", MagicValueType.Int) },
        { 3691, new("OperationGroupId", MagicValueType.Int) },
        { 3692, new("OperationGroupId", MagicValueType.Int) },
        { 3693, new("OperationGroupId", MagicValueType.Int) },
        { 3721, new("BoolValue", MagicValueType.Bool) },
        { 3722, new("Value", MagicValueType.Int) },
        { 3785, new("Value", MagicValueType.Int) },
        { 3791, new("Value", MagicValueType.Int) },
        { 3802, new("Value", MagicValueType.Int) },
        { 3848, new("CameraFCurveId", MagicValueType.Int) },
        { 3850, new("CameraFCurveId", MagicValueType.Int) },
        { 3856, new("CameraFCurveId", MagicValueType.Int) },
        { 3938, new("LayoutInstanceId", MagicValueType.Int) },
        { 3961, new("Value", MagicValueType.Int) },
        { 3962, new("Value", MagicValueType.Int) },
        { 3681, new("Value", MagicValueType.Int) },
        { 3872, new("Value", MagicValueType.Int) },
        { 3906, new("Value", MagicValueType.Int) },
        { 3907, new("Value", MagicValueType.Float) },
        { 3970, new("Value", MagicValueType.Int) },
        { 4002, new("EidId", MagicValueType.Int) },
        { 4007, new("Value", MagicValueType.Float) },
        { 4008, new("Time", MagicValueType.Float) },
        { 4010, new("Value", MagicValueType.Float) },
        { 4011, new("Value", MagicValueType.Int) },
        { 4014, new("Value", MagicValueType.Vec3Float) },
        { 4101, new("Value", MagicValueType.Float) },
        { 4102, new("Value", MagicValueType.Float) },
        { 4107, new("Value", MagicValueType.Int) },
        { 4131, new("Value", MagicValueType.Int) },
        { 4132, new("Value", MagicValueType.Int) },
        { 4243, new("BoolValue", MagicValueType.Bool) },
        { 4278, new("Value", MagicValueType.Int) },
        { 4279, new("Value", MagicValueType.Int) },
        { 4327, new("Value", MagicValueType.Int) },
        { 4343, new("Value", MagicValueType.Int) },
        { 4387, new("BoolValue", MagicValueType.Bool) },
        { 4402, new("OperationGroupId", MagicValueType.Int) },
        { 4427, new("Value", MagicValueType.Int) },
        { 4436, new("Value", MagicValueType.Int) },
        { 4447, new("Value", MagicValueType.Int) },
        { 4509, new("Value", MagicValueType.Int) },
        { 4774, new("OperationGroupId", MagicValueType.Int) },
        { 4785, new("BoolValue", MagicValueType.Bool) },
        { 4829, new("Value", MagicValueType.Int) },
        { 4845, new("Value", MagicValueType.Int) },
        { 4846, new("Value", MagicValueType.Int) },
        { 4847, new("Rate", MagicValueType.Int) },
        { 4848, new("Target", MagicValueType.Int) },
        { 5101, new("Value", MagicValueType.Int) },
        { 5125, new("Value", MagicValueType.Int) },
        { 5274, new("CommandId", MagicValueType.Int) },
        { 5275, new("CommandId", MagicValueType.Int) },
        { 5276, new("SkillUpgradeLevel", MagicValueType.Int) },
        { 5312, new("Value", MagicValueType.Int) },
        { 5644, new("Value", MagicValueType.Vec3Float) },
        { 5671, new("Value", MagicValueType.Int) },
        { 5685, new("Value", MagicValueType.Int) },
        { 5838, new("Value", MagicValueType.Float) },
        { 5959, new("Color", MagicValueType.Vec3Float) },
        { 5989, new("Value", MagicValueType.Int) },
        { 6056, new("BoolValue", MagicValueType.Bool) },
        { 6107, new("OperationGroupId", MagicValueType.Int) },
        { 6179, new("Value", MagicValueType.Int) },
        { 6287, new("Value", MagicValueType.Float) },
        { 6299, new("Value", MagicValueType.Int) },
        { 6339, new("Value", MagicValueType.Int) },
        { 6340, new("Value", MagicValueType.Int) },
        { 6376, new("Value", MagicValueType.Int) },
        { 6383, new("BoolValue", MagicValueType.Bool) },
        { 6454, new("Value", MagicValueType.Int) },
        { 6459, new("Value", MagicValueType.Int) },
        { 6471, new("Value", MagicValueType.Int) },
        { 6474, new("OperationGroupId", MagicValueType.Int) },
        { 6475, new("OperationGroupId", MagicValueType.Int) },
        { 6515, new("OperationGroupId", MagicValueType.Int) },
        { 6516, new("OperationGroupId", MagicValueType.Int) },
        { 6596, new("BoolValue", MagicValueType.Bool) },
        { 6600, new("Value", MagicValueType.Float) },
        { 6775, new("Chance", MagicValueType.Float) },
        { 6776, new("BoolValue", MagicValueType.Bool) },
        { 6800, new("Value", MagicValueType.Int) },
        { 6801, new("Value", MagicValueType.Int) },
        { 6822, new("Value", MagicValueType.Int) },
        { 6825, new("BoolValue", MagicValueType.Bool) },
        { 6826, new("BoolValue", MagicValueType.Bool) },
        { 6831, new("Value", MagicValueType.Float) },
        { 6913, new("Value", MagicValueType.Int) },
        { 6975, new("Value", MagicValueType.Int) },
        { 6988, new("BoolValue", MagicValueType.Bool) },
        { 7028, new("Value", MagicValueType.Int) },
        { 7037, new("Value", MagicValueType.Float) },
        { 7121, new("Value", MagicValueType.Int) },
        { 7428, new("Value", MagicValueType.Int) },
        { 7429, new("Value", MagicValueType.Int) },
        { 7430, new("Value", MagicValueType.Int) },
        { 7585, new("Value", MagicValueType.Int) },
        { 7586, new("Value", MagicValueType.Int) },
        { 7587, new("Value", MagicValueType.Int) },
        { 7588, new("Value", MagicValueType.Int) },
        { 7791, new("Value", MagicValueType.Int) },
        { 7799, new("BoolValue", MagicValueType.Bool) },
        { 7807, new("Value", MagicValueType.Int) },
        { 7809, new("Value", MagicValueType.Int) },
        { 7811, new("BoolValue", MagicValueType.Bool) },
        { 7812, new("Value", MagicValueType.Int) },
        { 7813, new("BoolValue", MagicValueType.Bool) },
        { 3145, new("Value", MagicValueType.Float) },
        { 7691, new("Trajectory Rotation Degrees", MagicValueType.Vec3Float) },
        { 7734, new("BoolValue", MagicValueType.Bool) },
        { 7735, new("BoolValue", MagicValueType.Bool) },
    };
}
