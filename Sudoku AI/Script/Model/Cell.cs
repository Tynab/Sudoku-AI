using System;
using System.Collections.Generic;
using static Sudoku_AI.Script.Constant;
using static System.EventArgs;

namespace Sudoku_AI.Script.Model
{
    internal class Cell
    {
        #region Fields
        private string _value = string.Empty;
        private List<string> _availableValues = new(BASE_NUMS);
        #endregion

        #region Properties
        internal int X { get; set; } = 0;
        internal int Y { get; set; } = 0;
        internal string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged();
                if (string.IsNullOrWhiteSpace(_value))
                {
                    AvailableValues = new List<string>();
                }
            }
        }
        internal List<string> AvailableValues
        {
            get => _availableValues;
            set
            {
                _availableValues = value;
                if (_availableValues.Count == 1)
                {
                    Value = _availableValues[0];
                }
            }
        }
        #endregion
        #region Events
        internal event EventHandler ValueChanged;
        internal void OnValueChanged() => ValueChanged?.Invoke(this, Empty);
        #endregion
    }
}
