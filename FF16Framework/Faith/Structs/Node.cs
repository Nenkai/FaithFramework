using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Structs;

public unsafe struct /*faith::*/Node // sizeof=0x70
{
    public nint vtable;
    public int field_8;
    public int field_C;
    public Matrix4x4 Matrix;
    public uint Flags;
    public Transform Transform;
};

public unsafe struct Transform // sizeof=0x1C
{
    public Vector3 Position;
    public Vector3 EulerRotation;
    public float Scale;
};

