using Sudoku_AI.Script.Model;
using System;
using System.Linq;
using System.Windows.Forms;
using YANF.Script;
using static Sudoku_AI.Script.Constant;
using static System.Drawing.Color;
using static YANF.Script.YANEvent;

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
            // txt evnt
            foreach (var txt in this.GetAllObjs(typeof(TextBox)).Cast<TextBox>())
            {
                txt.KeyPress += TxtNumeric_Keypress;
            }
            // pnl Bar evnt
            pnlBar.MouseDown += MoveFrmMod_MouseDown;
            pnlBar.MouseMove += MoveFrm_MouseMove;
            pnlBar.MouseUp += MoveFrm_MouseUp;
            // lbl Title evnt
            lblTit.MouseDown += MoveFrmMod_MouseDown;
            lblTit.MouseMove += MoveFrm_MouseMove;
            lblTit.MouseUp += MoveFrm_MouseUp;
            // for demo
            for(var i = 0; i < MAX_W; i++)
            {
                for(var j = 0; j < MAX_H; j++)
                {
                    if (!string.IsNullOrWhiteSpace(BOARD_DEMO[i, j]))
                    {
                        _txtCells[i, j].String = BOARD_DEMO[i, j];
                    }
                }
            }
        }
        #endregion

        #region Events
        // frm load
        private void FrmMain_Load(object sender, EventArgs e)
        {
            FillCells();
            FillAreas();
        }

        // frm shown
        private void FrmMain_Shown(object sender, EventArgs e) => this.FadeIn();

        // frm closing
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) => this.FadeOut();

        // btn calculate click
        private void BtnCalc_Click(object sender, EventArgs e)
        {
            // sound
            SND_NEXT.Play();
            // main
            pnlBar.Select();
            FillCells();
            FillAreas();
            BlkBoardCtrl(true);
            BoardCtrlRsltMode(true);
            // process
            var board = new Board(_arrAreas);
            board.Prcs();
            FillBoardCtrl(board);
        }

        // btn Reset click
        private void BtnRst_Click(object sender, EventArgs e)
        {
            // sound
            SND_BACK.Play();
            // main
            BlkBoardCtrl(false);
            BoardCtrlRsltMode(false);
        }

        // btn Close click
        private void BtnCl_Click(object sender, EventArgs e)
        {
            // action
            Close();
            // sound
            SND_NEXT.PlaySync();
        }
        #endregion

        #region Methods
        // Fill cells
        private void FillCells()
        {
            for (var i = 0; i < MAX_W; i++)
            {
                for (var j = 0; j < MAX_H; j++)
                {
                    var val = _txtCells[i, j].String;
                    _arrCells[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        Value = val
                    };
                }
            }
        }

        // Fill areas
        private void FillAreas()
        {
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

        // Fill board ctrl
        private void FillBoardCtrl(Board board)
        {
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

        // Block board control
        private void BlkBoardCtrl(bool isBlk)
        {
            foreach (var txt in this.GetAllObjs(typeof(TextBox)).Cast<TextBox>())
            {
                txt.ReadOnly = isBlk;
            }
        }

        // Board ctrl result mode
        private void BoardCtrlRsltMode(bool isRsltMode)
        {
            for (var i = 0; i < MAX_W; i++)
            {
                for (var j = 0; j < MAX_H; j++)
                {
                    if (isRsltMode)
                    {
                        if (string.IsNullOrWhiteSpace(_txtCells[i, j].String))
                        {
                            _txtCells[i, j].ForeColor = Blue;
                        }
                    }
                    else
                    {
                        _txtCells[i, j].String = string.Empty;
                        _txtCells[i, j].ForeColor = DimGray;
                    }
                }
            }
        }
        #endregion
    }
}
