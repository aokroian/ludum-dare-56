using System;

namespace InputUtils
{
    [Flags]
    public enum PlayerInputFlags
    {
        None = 0,
        Fire = 1,
        Matchstick = 2,
        Move = 4,
        Look = 8,
        Escape = 16,
        Restart = 32,
        All = Fire | Matchstick | Move | Look | Escape | Restart, 
        NonGameplay = Escape | Restart
    }
}