using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Application
{
    class CopyGrid
    {
        public TableCase GridCopy { get; set; }
        public Case caseCopy { get; set; }
        public int valueTest { get; set; }

        public CopyGrid(TableCase tb, Case c, int test)
        {
            GridCopy = tb;
            caseCopy = c;
            valueTest = test;
        }
    }
}
