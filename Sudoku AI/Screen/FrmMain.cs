using Sudoku_AI.Script.Model;
using System;
using System.Windows.Forms;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Screen
{
    public partial class FrmMain : Form
    {
        #region Fields
        private readonly Cell[,] _arrCells = new Cell[MAX_W, MAX_H];
        private readonly Area[,] _arrAreas = new Area[WB, HB];
        #endregion

        #region Constructors
        public FrmMain()
        {
            InitializeComponent();
            InitItems();
        }
        #endregion

        #region Events
        // frm load
        private void FrmMain_Load(object sender, EventArgs e)
        {
            // fill cells
            for (var i = 0; i < MAX_W; i++)
            {
                for (var j = 0; j < MAX_H; j++)
                {
                    _arrCells[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        Value = _txtCells[i, j].String
                    };
                }
            }
            // fill areas
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    var area = new Area
                    {
                        X = i,
                        Y = j,
                        Cells = new Cell[WA, HA]
                    };
                    var m = 0;
                    for (var k = i * WA; k < (i + 1) * WA; k++)
                    {
                        var n = 0;
                        for (var l = j * WA; l < (j + 1) * HA; l++)
                        {
                            area.Cells[m, n] = _arrCells[k, l];
                            n++;
                        }
                        m++;
                    }
                    _arrAreas[i, j] = area;
                }
            }
        }

        // btn calculate click
        private void BtnCalc_Click(object sender, EventArgs e)
        {
            var board = new Board(_arrAreas);
            board.Prcs();
            for (var i = 0; i < WB; i++)
            {
                for (var j = 0; j < HB; j++)
                {
                    for (var k = 0; k < WA; k++)
                    {
                        for (var l = 0; l < HA; l++)
                        {
                            var cell = board.Areas[i, j].Cells[k, l];
                            _txtCells[cell.X, cell.Y].String = cell.Value;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
