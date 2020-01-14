using System;

namespace Game.Core
{
    [Flags]
    public enum Inputs
    {
        None = 0,
        Mouse = 1,
        Cursor = 2,
        Arrows = 4,
        Keyboard = 8
    }
}