using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Application
{
    public class TableCase
    {
        Case[,] grid;
        int sizeTable;

        public TableCase(int size)
        {
            grid = new Case[size, size];
            sizeTable = size;
        }

        public TableCase(TableCase tb)
        {
            sizeTable = tb.sizeTable;
            grid = new Case[sizeTable, sizeTable];
            for (int i = 0; i < tb.sizeTable; i++)
            {
                for (int j = 0; j < tb.sizeTable; j++)
                {
                    grid[i, j] = new Case(tb[i, j]);
                }
            }
        }

        public void reinit(TableCase tb)
        {
            for (int i = 0; i < sizeTable; i++)
            {
                for (int j = 0; j < sizeTable; j++)
                {
                    Case c = grid[i, j];
                    c.Valeur = tb[i, j].Valeur;
                    c.Line = tb[i, j].Line;
                    c.Square = tb[i, j].Square;
                    c.Hypotheses.Clear();
                    for (int k = 0; k < tb[i, j].Hypotheses.Count; k++)
                    {
                        c.Hypotheses.Add(tb[i, j].Hypotheses[k]);
                    }
                }
            }
        }

        public bool isPresent(int i, int j)
        {
            for (int k = 0; k < sizeTable; k++)
            {
                if (k == j) continue;
                if (grid[i, k].Valeur == grid[i, j].Valeur)
                    return true;
            }
            return false;
        }

        public bool isNotPresent(int i, char value)
        {
            for (int k = 0; k < sizeTable; k++)
            {
                if (value == grid[i, k].Valeur)
                    return false;
            }
            return true;
        }

        public bool isHypothesePresentInTableCase(int i, char value)
        {
            for (int j = 0; j < sizeTable; j++)
            {
                if (isHypothesePresent(i, j, value))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isHypothesePresent(int i, int j, char value)
        {
            for (int k = 0; k < grid[i, j].Hypotheses.Count; k++)
            {
                if (grid[i, j].Hypotheses[k] == value)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isHypothesePresent(int i, char value)
        {
            int nbPresent = 0;
            for (int j = 0; j < sizeTable; j++)
            {
                if (isHypothesePresent(i, j, value))
                    nbPresent++;
            }
            return nbPresent == 1 ? false : true;
        }

        public int countHypothese(int i, char value)
        {
            int cpt = 0;
            for (int j = 0; j < grid.Length; j++)
            {
                for (int k = 0; k < grid[i, j].Hypotheses.Count; k++)
                {
                    if (grid[i, j].Hypotheses[k] == value)
                    {
                        cpt++;
                    }
                }

            }
            return cpt;
        }

        public void clearHypothese(int i, char value)
        {
            for (int j = 0; j < sizeTable; j++)
            {
                if (isHypothesePresent(i, j, value))
                {
                    grid[i, j].Hypotheses.Remove(value);
                }
            }
            //grid[i, j].Hypotheses.Clear();
        }

        public Case this[int i, int j]
        {
            get { return grid[i, j]; }
            set { grid[i, j] = value; }
        }

        public bool isComplete()
        {
            for (int i = 0; i < sizeTable; i++)
            {
                for (int j = 0; j < sizeTable; j++)
                {
                    if (grid[i, j].Valeur == '.')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /*si c'est présent sur la même ligne*/
        public bool isHPML(int square, int line, char v)
        {
            for (int j = 0; j < sizeTable; j++)
            {
                if (isHypothesePresent(square, j, v) && grid[square, j].Line != line)
                {
                    return false;
                }
            }
            return true;
        }

        /* si c'est présent sur la meme colonne */
        public bool isHPMC(int square, int column, char v)
        {
            for (int j = 0; j < sizeTable; j++)
            {
                if (isHypothesePresent(square, j, v) && grid[square, j].Column != column)
                {
                    return false;
                }
            }
            return true;
        }

        public bool clearHypothese(int num, int square, char v)
        {
            bool action = false;
            for (int j = 0; j < sizeTable; j++)
            {
                if (isHypothesePresent(num, j, v) && grid[num, j].Square != square)
                {
                    grid[num, j].Hypotheses.Remove(v);
                    action = true;
                }
            }
            return action;
        }

        public bool candidatsIdentiques(int i, int j)
        {
            int nbOccurrence = 0;
            bool isEqual;
            bool action = false;
            List<Case> clearHypothese = new List<Case>();
            for (int k = j + 1; k < sizeTable; k++)
            {
                if (grid[i, j].Hypotheses.Count == grid[i, k].Hypotheses.Count)
                {
                    isEqual = true;
                    for (int l = 0; l < grid[i, j].Hypotheses.Count; l++)
                    {
                        if (!grid[i, k].Hypotheses.Contains(grid[i, j].Hypotheses[l]))
                        {
                            isEqual = false;
                            break;
                        }
                    }
                    if (isEqual)
                    {
                        nbOccurrence++;
                        clearHypothese.Add(grid[i, k]);
                    }
                }
            }
            if (nbOccurrence == grid[i, j].Hypotheses.Count)
            {
                foreach (Case c in clearHypothese)
                {
                    foreach (char v in grid[i, j].Hypotheses)
                    {
                        if (c.Hypotheses.Remove(v))
                            action = true;
                    }
                }
            }
            return action;
        }
    }
}
