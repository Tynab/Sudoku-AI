using System.Collections.Generic;
using System.Media;
using static Sudoku_AI.Properties.Resources;

namespace Sudoku_AI.Script
{
    internal static class Constant
    {
        // sound
        internal static readonly SoundPlayer SND_BACK = new(sBack);
        internal static readonly SoundPlayer SND_NEXT = new(sNext);
        internal static readonly SoundPlayer SND_HOV = new(sHover);
        internal static readonly SoundPlayer SND_PRS = new(sPress);
        internal static readonly SoundPlayer SND_CHG = new(sChange);

        // other
        internal const int WB = 3;
        internal const int HB = 3;
        internal const int WA = 3;
        internal const int HA = 3;
        internal const int MAX_W = 9;
        internal const int MAX_H = 9;
        internal const int MAX_NUM = 9;
        internal static readonly List<string> BASE_NUMS = new List<string>()
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"
        };
        internal static readonly string[,] BOARD_DEMO = new string[MAX_W, MAX_H]
        {
            { " ", " ", " ", "3", " ", " ", "9", " ", " " },
            { " ", "2", " ", " ", " ", "1", " ", " ", "7" },
            { " ", " ", " ", " ", " ", " ", "1", "5", " " },
            { "1", "5", "7", " ", " ", " ", "4", " ", " " },
            { " ", "9", " ", " ", "4", " ", " ", " ", " " },
            { " ", " ", " ", "1", " ", "6", " ", "3", " " },
            { " ", "3", " ", "2", " ", "7", " ", " ", " " },
            { "5", " ", "9", " ", " ", " ", "8", " ", " " },
            { "6", " ", " ", " ", " ", "4", "5", " ", " " }
        };
    }
}
