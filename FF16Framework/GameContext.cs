using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework;

public class GameContext
{
    public FaithGameType GameType { get; private set; }

    public void SetGameType(FaithGameType gameType) => GameType = gameType;
}

public enum FaithGameType
{
    FFXVI,
    FFT,
}
