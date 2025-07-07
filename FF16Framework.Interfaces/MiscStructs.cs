using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces;

public unsafe struct StdVector
{
    public void* Myfirst;
    public void* Mylast;
    public void* Myend;
}

// https://github.com/microsoft/STL/blob/881bcadeca4ae9240a132588d9ac983e7b24dbe0/stl/inc/list#L286
public unsafe struct StdListNode // _List_node
{
    public StdListNode* Next;
    public StdListNode* Previous;
    public uint Key;
    public uint Padding;
    public void* Data; // Starting from here is data. Type is templated, it could be anything else inline to this struct i.e a std::vector
}

// https://github.com/microsoft/STL/blob/881bcadeca4ae9240a132588d9ac983e7b24dbe0/stl/inc/list#L755
public unsafe struct StdList // std::list
{
    public StdListNode* Node;
    public void* Unk;
    public uint Size;
}

// https://github.com/microsoft/STL/blob/881bcadeca4ae9240a132588d9ac983e7b24dbe0/stl/inc/xhash#L1960
public unsafe struct StdUnorderedMap
{
    public ulong LoadFactor;
    public StdList List;
    public StdVector Vec;
    public ulong Mask;
    public ulong MaskIdx;
};
