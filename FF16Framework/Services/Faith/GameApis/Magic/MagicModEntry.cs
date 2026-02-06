using System;

namespace FF16Framework.Services.Faith.GameApis.Magic;

/// <summary>
/// Internal structure representing a magic modification to be applied at runtime.
/// Used by the MagicProcessor to override or inject properties into .magic files.
/// </summary>
public class MagicModEntry
{
    public bool Enabled { get; set; } = true;
    public bool IsInjection { get; set; } = false; // If true, injects even if property doesn't exist
    public bool IsOperationOnly { get; set; } = false; // If true, only registers the operation
    public int TargetMagicId { get; set; } = -1;   // -1 for all, or specific Magic ID
    public int InjectAfterOp { get; set; } = -1;   // Inject after this Op ID
    public bool DisableOp { get; set; } = false;   // Block the original operation
    public int OpType { get; set; } = 51;
    public int PropertyId { get; set; } = 8;
    public bool UseFloat { get; set; } = true;
    public float FloatValue { get; set; } = 0.0f;
    public int IntValue { get; set; } = 0;

    public bool UseVec3 { get; set; } = false;
    public float Vec3X { get; set; } = 0.0f;
    public float Vec3Y { get; set; } = 0.0f;
    public float Vec3Z { get; set; } = 0.0f;

    public int Occurrence { get; set; } = -1; // -1 for all, 0 for first, etc.
    public int TargetOperationGroupId { get; set; } = -1; // -1 for all, or specific Group ID

    public MagicModEntry Clone()
    {
        return new MagicModEntry
        {
            Enabled = this.Enabled,
            IsInjection = this.IsInjection,
            IsOperationOnly = this.IsOperationOnly,
            TargetMagicId = this.TargetMagicId,
            InjectAfterOp = this.InjectAfterOp,
            DisableOp = this.DisableOp,
            OpType = this.OpType,
            PropertyId = this.PropertyId,
            UseFloat = this.UseFloat,
            FloatValue = this.FloatValue,
            IntValue = this.IntValue,
            UseVec3 = this.UseVec3,
            Vec3X = this.Vec3X,
            Vec3Y = this.Vec3Y,
            Vec3Z = this.Vec3Z,
            Occurrence = this.Occurrence,
            TargetOperationGroupId = this.TargetOperationGroupId
        };
    }
}
