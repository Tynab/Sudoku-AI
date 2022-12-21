using Sudoku_AI.Screen;
using System;
using static System.Windows.Forms.Application;

namespace Sudoku_AI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            EnableVisualStyles();
            SetCompatibleTextRenderingDefault(false);
            Run(new FrmMain());
        }
    }
}
