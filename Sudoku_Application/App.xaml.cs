﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku_Application
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SudokuViewModel viewModelSudoku { get; set; }
        static App()
        {
            viewModelSudoku = new SudokuViewModel();
        }
    }
}
