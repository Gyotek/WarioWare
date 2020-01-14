using System;

namespace Game.Core
{
    [Flags]
    public enum Rivals
    {
        None = 0,
        Melo = 1,
        Theo = 2,
        Jesse = 4,
        Fabala = 8,
        Vlad = 16,
        Emmanalyst = 32,
        Dode = 64,
        Hortensia = 128,
        Arsene = 256,
        MatMax = 512
    }
}