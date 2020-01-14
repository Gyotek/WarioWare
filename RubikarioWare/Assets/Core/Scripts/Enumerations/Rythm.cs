using System;

namespace Game.Core
{
    [Flags]
    public enum Rythm 
    {
        None = 0,
        Bpm96 = 1,
        Bpm120 = 2,
        Bpm160 = 4
    }
}
