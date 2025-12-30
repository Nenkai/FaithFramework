using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Utils;

public unsafe struct StdMap<T> : IEnumerable<StdMapEntry<T>> where T : unmanaged 
{
    // this is also really just a node too
    public Node<T>* Root;       // header->parent
    public Node<T>* Leftmost;   // header->left
    public Node<T>* Rightmost;  // header->right

    public Node<T>* Begin() => Leftmost;
    public Node<T>* End() => (Node<T>*)Unsafe.AsPointer(ref this);

    public bool IsEmpty()
        => Leftmost == End();

    public unsafe uint Count()
    {
        uint count = 0;
        Node<T>* end = End();

        for (Node<T>* it = Begin(); it != end && it->IsNil == 0; it = Node<T>.Next(it))
            count++;

        return count;
    }

    public IEnumerator<StdMapEntry<T>> GetEnumerator()
        => new StdMapEnumerator<T>((StdMap<T>*)Unsafe.AsPointer(ref this));

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
};


public unsafe struct Node<T> where T : unmanaged
{
    public Node<T>* Left;     // [0]
    public Node<T>* Parent;   // [1]
    public Node<T>* Right;    // [2]
    public byte Color;    // +24 (red/black)
    public byte IsNil;    // +25 (sentinel flag)
    private ushort padding; // +26
    private uint padding2; // +28
    public uint Key;   // +32 (fileHash)
    public T* Value; // [5]

    public static unsafe Node<T>* Next(Node<T>* n)
    {
        if (n->Right->IsNil == 0)
        {
            n = n->Right;
            while (n->Left->IsNil == 0)
                n = n->Left;
            return n;
        }

        Node<T>* p = n->Parent;
        while (p->IsNil == 0 && n == p->Right)
        {
            n = p;
            p = p->Parent;
        }
        return p;
    }
};

// Adapters for IEnumerable/IEnumerator
public unsafe readonly struct StdMapEntry<T> where T : unmanaged
{
    public readonly uint Key;
    public readonly T* Value;

    public StdMapEntry(uint key, T* value)
    {
        Key = key;
        Value = value;
    }
}

public unsafe struct StdMapEnumerator<T> : IEnumerator<StdMapEntry<T>> where T : unmanaged
{
    private Node<T>* _current;
    private readonly Node<T>* _end;

    internal StdMapEnumerator(StdMap<T>* map)
    {
        _current = map->Begin();
        _end = map->End();
        Current = default;
    }

    public StdMapEntry<T> Current { get; private set; }
    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_current == _end || _current->IsNil != 0)
            return false;

        Current = new StdMapEntry<T>(_current->Key, _current->Value);
        _current = Node<T>.Next(_current);
        return true;
    }

    public void Reset()
        => throw new NotSupportedException();

    public void Dispose() { }
}
