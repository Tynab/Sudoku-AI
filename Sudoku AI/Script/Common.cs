using Sudoku_AI.Script.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Sudoku_AI.Script.Constant;
using static System.Threading.Tasks.Task;
using static System.Threading.Tasks.TaskStatus;

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

        /// <summary>
        /// List board first success task.
        /// </summary>
        /// <param name="tasks">Task list.</param>
        /// <returns>Board completed.</returns>
        internal static async Task<Board> Scs1st(this IEnumerable<Task<Board>> tasks)
        {
            var taskList = new List<Task<Board>>(tasks);
            var cmpl1st = default(Task<Board>);
            while (taskList.Count > 0)
            {
                var cmplCur = await WhenAny(taskList);
                if (cmplCur.Status == RanToCompletion && cmplCur.Result != null)
                {
                    cmpl1st = cmplCur;
                    break;
                }
                else
                {
                    taskList.Remove(cmplCur);
                }
            }
            return (cmpl1st != default(Task<Board>)) ? cmpl1st.Result : default;
        }
    }
}
