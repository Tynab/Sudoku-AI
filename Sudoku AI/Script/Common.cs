using Sudoku_AI.Script.Model;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script
{
    internal static class Common
    {
        /// <summary>
        /// Copy array area 2D.
        /// </summary>
        /// <param name="srcArr">Source array.</param>
        /// <returns>New array.</returns>
        internal static Area[,] Copy(this Area[,] srcArr)
        {
            var newArr = new Area[WB, HB];
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    newArr[i, j] = new Area
                    {
                        Cells = new Cell[WA, HA]
                    };
                    for (var k = 0; k < WA; k++)
                    {
                        for (var l = 0; l < HA; l++)
                        {
                            var srcCell = srcArr[i, j].Cells[k, l];
                            newArr[i, j].Cells[k, l] = new Cell
                            {
                                X = srcCell.X,
                                Y = srcCell.Y,
                                Value = srcCell.Value
                            };
                        }
                    }
                }
            }
            return newArr;
        }
    }
}
