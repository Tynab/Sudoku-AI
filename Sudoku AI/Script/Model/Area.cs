using System;
using System.Collections.Generic;
using System.Linq;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Script.Model
{
    internal class Area
    {
        #region Fields
        private bool _flag = false;
        #endregion

        #region Properties
        internal List<string> ExistValues;
        internal Cell[,] Cells { get; set; }
        internal int X { get; set; } = 0;
        internal int Y { get; set; } = 0;
        internal bool IsBreak { get; set; } = false;
        internal bool IsCompleted { get; set; } = true;
        #endregion

        #region Constructors
        internal Area(Cell[,] cells)
        {
            Cells = cells;
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    Cells[i, j].ValueChanged += Cell_ValueChanged;
                }
            }
        }
        #endregion

        #region Events
        // Cell value changed
        private void Cell_ValueChanged(object sender, EventArgs e) => _flag = true;
        #endregion

        #region Methods
        /// <summary>
        /// Process.
        /// </summary>
        internal void Prcs()
        {
            _flag = false;
            ChkBrk();
            ChkCplt();
            if (!IsBreak && !IsCompleted)
            {
                LastFreeCell();
                ObviousPrs();
                Obvious3b();
                H1b();
            }
            Reboot();
        }

        // Get list cells
        private IEnumerable<Cell> GetListCells()
        {
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    yield return Cells[i, j];
                }
            }
        }

        // Break check
        private void ChkBrk()
        {
            ExistValues = new List<string>();
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    var val = Cells[i, j].Value;
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (ExistValues.Contains(val))
                        {
                            IsBreak = true;
                            return;
                        }
                        ExistValues.Add(val);
                    }
                }
            }
        }

        // Completed check
        private void ChkCplt()
        {
            if (!IsBreak)
            {
                for (var i = 0; i < WA; i++)
                {
                    for (var j = 0; j < HA; j++)
                    {
                        if (string.IsNullOrWhiteSpace(Cells[i, j].Value))
                        {
                            IsCompleted = false;
                            return;
                        }
                    }
                }
            }
            else
            {
                IsCompleted = false;
            }
        }

        // Last free cell
        private void LastFreeCell() => ExistValues.ForEach(x =>
                                                {
                                                    for (var i = 0; i < MAX_W; i++)
                                                    {
                                                        for (var j = 0; j < MAX_H; j++)
                                                        {
                                                            Cells[i, j].AvailableValues.Remove(x);
                                                        }
                                                    }
                                                });

        // Obvious pairs
        private void ObviousPrs() => GetListCells().Where(x => x.AvailableValues.Count == 2).Select(x => x.AvailableValues).GroupBy(y => y).Where(g => g.Count() > 1).SelectMany(g => g.Distinct()).SelectMany(y => y).ToList().ForEach(z =>
                                                                                {
                                                                                    for (var i = 0; i < WA; i++)
                                                                                    {
                                                                                        for (var j = 0; j < HA; j++)
                                                                                        {
                                                                                            var availVas = Cells[i, j].AvailableValues;
                                                                                            if (availVas.Count > 2 && availVas.Contains(z))
                                                                                            {
                                                                                                availVas.Remove(z);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                });

        // Obvious triples
        private void Obvious3b()
        {
            var prs = GetListCells().Where(x => x.AvailableValues.Count == 2).Select(x => x.AvailableValues).SelectMany(y => y).Distinct().OrderBy(int.Parse).ToList();
            var obvious3d = new List<List<string>>();
            var cnt = prs.Count;
            // find obvious pairs
            for (var i = 0; i < cnt; i++)
            {
                for (var j = i; j < cnt; j++)
                {
                    prs[i].ForEach(x =>
                    {
                        if (prs[j].Contains(x))
                        {
                            prs[j].ForEach(y =>
                            {
                                if (y != x)
                                {
                                    for (var k = j; k < cnt; k++)
                                    {
                                        if (prs[k].Contains(y))
                                        {
                                            prs[k].ForEach(z =>
                                            {
                                                if (z == x)
                                                {
                                                    obvious3d.Add(prs[i]);
                                                    obvious3d.Add(prs[j]);
                                                    obvious3d.Add(prs[k]);
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    });
                }
            }
            // reboot
            obvious3d.ForEach(x =>
            {
                for (var i = 0; i < WA; i++)
                {
                    for (var j = 0; j < HA; j++)
                    {
                        var availVas = Cells[i, j].AvailableValues;
                        if (availVas.Count > 2 && x.All(y => availVas.Contains(y)))
                        {
                            x.ForEach(y => availVas.Remove(y));
                        }
                    }
                }
            });
        }

        // Hidden singles
        private void H1b() => GetListCells().Select(x => x.AvailableValues).Select(y => y).GroupBy(y => y).Where(g => g.Count() == 1).SelectMany(g => g.First()).ToList().ForEach(z =>
                                       {
                                           for (var i = 0; i < WA; i++)
                                           {
                                               for (var j = 0; j < HA; j++)
                                               {
                                                   var availVas = Cells[i, j].AvailableValues;
                                                   if (availVas.Contains(z))
                                                   {
                                                       Cells[i, j].Value = z;
                                                   }
                                               }
                                           }
                                       });

        // Hidden pairs
        private void HPrs()
        {
            var cells = GetListCells().ToList();
            var listCpls = new List<List<(string, string)>>();
            for (var i = 0; i < WA; i++)
            {
                for (var j = 0; j < HA; j++)
                {
                    var cpls = new List<(string, string)>();
                    var availVas = Cells[i, j].AvailableValues;
                    var availVasCnt = availVas.Count;
                    for (var k = 0; k < availVasCnt; k++)
                    {
                        for (var l = k; l < availVasCnt; l++)
                        {
                            cpls.Add((availVas[k], availVas[l]));
                        }
                    }
                    listCpls.Add(cpls);
                }
            }
            listCpls.ForEach(x =>
            {
                x.GroupBy
            });
        }

        // Update Cell
        private void Reboot()
        {
            if (_flag)
            {
                Prcs();
            }
        }
        #endregion
    }
}
