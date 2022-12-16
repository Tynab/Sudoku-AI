using Sudoku_AI.Script.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sudoku_AI.Script.Constant;

namespace Sudoku_AI.Screen
{
    public partial class FrmMain : Form
    {
        private Cell[,] _arrCells = new Cell[MAX_W, MAX_H];
        private Area[,] _arrAreas = new Area[W, H];
        public FrmMain()
        {
            InitializeComponent();
            InitItems();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //
            for (var i = 0; i < MAX_W; i++)
            {
                for (var j = 0; j < MAX_H; j++)
                {
                    _arrCells[i, j] = new Cell()
                    {
                        X = i,
                        Y = j,
                        Value = _txtCells[i, j].String
                    };
                }
            }
            //
            for (var i = 0; i < W; i++)
            {
                for (var j = 0; j < H; j++)
                {
                    var area = new Area()
                    {
                        X = i,
                        Y = j,
                    };
                    for (var k = 0; k < W; k++)
                    {
                        for (var l = 0; l < H; l++)
                        {

                        }
                    }
                }
            }
        }
    }
}
