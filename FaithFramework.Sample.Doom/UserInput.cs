using DoomNetFrameworkEngine;
using DoomNetFrameworkEngine.DoomEntity.Event;
using DoomNetFrameworkEngine.DoomEntity.Game;
using DoomNetFrameworkEngine.DoomEntity.World;
using DoomNetFrameworkEngine.UserInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FaithFramework.Sample.Doom;

public class UserInput : IUserInput
{
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    private static extern long GetKeyboardLayoutName(System.Text.StringBuilder pwszKLID);
    const int KL_NAMELENGTH = 9;

    private Config config;
    private Action<DoomEvent> postEventCallback;
    private readonly HashSet<ConsoleKey> pressedKeys = new();

    private bool isAzerty = false;
    public UserInput(Config config, Action<DoomEvent> postEvent)
    {
        this.config = config;
        postEventCallback = postEvent;

        StringBuilder name = new StringBuilder(KL_NAMELENGTH);
        GetKeyboardLayoutName(name);
        isAzerty = name.ToString() == "0000040C";
    }

    private static bool IsKeyDown(ConsoleKey key) => (GetAsyncKeyState((int)key) & 0x8000) != 0;

    public void PostEvent(DoomEvent e) { }

    public void BuildTicCmd(TicCmd cmd)
    {
        cmd.Clear();

        int speed = 1;

        if (IsKeyDown(!isAzerty ? ConsoleKey.W : ConsoleKey.Z))
            cmd.ForwardMove += (sbyte)PlayerBehavior.ForwardMove[speed];
        if (IsKeyDown(ConsoleKey.S))
            cmd.ForwardMove -= (sbyte)PlayerBehavior.ForwardMove[speed];
        if (IsKeyDown(!isAzerty ? ConsoleKey.A : ConsoleKey.Q))
            cmd.SideMove -= (sbyte)PlayerBehavior.SideMove[speed];
        if (IsKeyDown(ConsoleKey.D))
            cmd.SideMove += (sbyte)PlayerBehavior.SideMove[speed];

        if (IsKeyDown(ConsoleKey.LeftArrow))
            cmd.AngleTurn += (short)PlayerBehavior.AngleTurn[speed];
        if (IsKeyDown(ConsoleKey.RightArrow))
            cmd.AngleTurn -= (short)PlayerBehavior.AngleTurn[speed];

        if (IsKeyDown(ConsoleKey.Enter))
            cmd.Buttons |= TicCmdButtons.Attack;
        if (IsKeyDown(ConsoleKey.Spacebar))
            cmd.Buttons |= TicCmdButtons.Use;
        if (IsKeyDown(ConsoleKey.Escape))
            postEventCallback?.Invoke(new DoomEvent(EventType.KeyDown, DoomKey.Escape));

        for (int i = 1; i <= 7; i++)
        {
            if (IsKeyDown((ConsoleKey)((int)ConsoleKey.D1 + i - 1)))
            {
                cmd.Buttons |= TicCmdButtons.Change;
                cmd.Buttons |= (byte)((i - 1) << TicCmdButtons.WeaponShift);
                break;
            }
        }
    }

    private void SendMenuKey(ConsoleKey key, DoomKey mapped)
    {
        if (IsKeyDown(key))
        {
            if (!pressedKeys.Contains(key))
            {
                pressedKeys.Add(key);
                postEventCallback?.Invoke(new DoomEvent(EventType.KeyDown, mapped));
            }
        }
        else if (pressedKeys.Remove(key))
        {
            postEventCallback?.Invoke(new DoomEvent(EventType.KeyUp, mapped));
        }
    }

    public void Reset() { pressedKeys.Clear(); }
    public void GrabMouse() { }
    public void ReleaseMouse() { }
    public void Dispose() { }

    public int MaxMouseSensitivity => 15;
    public int MouseSensitivity { get => 5; set { } }

    public void PollMenuKeys()
    {
        SendMenuKey(ConsoleKey.UpArrow, DoomKey.Up);
        SendMenuKey(!isAzerty ? ConsoleKey.W : ConsoleKey.Z, DoomKey.Up);

        SendMenuKey(ConsoleKey.DownArrow, DoomKey.Down);
        SendMenuKey(ConsoleKey.S, DoomKey.Down);

        SendMenuKey(ConsoleKey.LeftArrow, DoomKey.Left);
        SendMenuKey(!isAzerty ? ConsoleKey.A : ConsoleKey.Q, DoomKey.Left);

        SendMenuKey(ConsoleKey.RightArrow, DoomKey.Right);
        SendMenuKey(ConsoleKey.D, DoomKey.Right);

        SendMenuKey(ConsoleKey.Enter, DoomKey.Enter);
        SendMenuKey(ConsoleKey.Spacebar, DoomKey.Enter);

        SendMenuKey(ConsoleKey.Escape, DoomKey.Escape);

        SendMenuKey(ConsoleKey.Y, DoomKey.Y);
        SendMenuKey(ConsoleKey.N, DoomKey.N);
    }
}
