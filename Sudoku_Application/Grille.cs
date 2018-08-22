using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku_Application
{
    public class Grille
    {
        public string Nom { get; set; }
        public string Date { get; set; }
        public string Symbole { get; set; }
        public int Size { get; set; }

        int sizeTable;
        int rc;
        public TableCase grid;
        TableCase lines;
        TableCase columns;
        TableCase squares;
        List<CopyGrid> copyGrid;
        int nbBruteForce = 0;

        public Grille()
        {
            grid = null;
            lines = null;
            columns = null;
            squares = null;
        }

        public void initializeTab(int size)
        {
            if (grid == null)
            {
                rc = (int)Math.Sqrt((double)size);
                sizeTable = size;
                Size = size;
                grid = new TableCase(size);
                lines = new TableCase(size);
                columns = new TableCase(size);
                squares = new TableCase(size);
                copyGrid = new List<CopyGrid>();
            }
        }

        public void setTable(int i, int j, Case newCase)
        {
            grid[i, j] = newCase;
            lines[i, j] = newCase;
            columns[j, i] = newCase;
            squares[numSquare(i, j), columSquare(i, j)] = newCase;
        }

        public void valideGrille()
        {
            String caption = "Validation de la grille";
            String message = null;

            
            for (int i = 0; i < sizeTable; i++)
                for (int j = 0; j < sizeTable; j++)
                {
                    if (grid[i, j].Valeur != '.')
                    {
                        if (lines.isPresent(i, j))
                        {
                            Console.WriteLine("erreur sur la ligne : " + i);
                            message = "erreur sur la ligne : " + i;
                            MessageBox.Show(message, caption);
                            return;
                        }
                        else if (columns.isPresent(j, i))
                        {
                            Console.WriteLine("erreur sur la colonne : " + j);
                            message = "erreur sur la colonne : " + j;
                            MessageBox.Show(message, caption);
                            return;
                        }
                        else if (squares.isPresent(numSquare(i, j), columSquare(i, j)))
                        {
                            Console.WriteLine("erreur sur le carré : " + numSquare(i, j));
                            message = "erreur sur le carré : " + numSquare(i, j);
                            MessageBox.Show(message, caption);
                            return;
                        }
                    }
                }

            Console.WriteLine("La grille " + Nom + " est valide \n");
            message = "La grille " + Nom + " est valide";
            MessageBox.Show(message, caption);
        }

        public void completeGrille()
        {
            addHypotheses();
            candidatUJT();
        }

        private void addHypotheses()
        {
            for (int i = 0; i < sizeTable; i++)
            {
                for (int j = 0; j < sizeTable; j++)
                {
                    if (grid[i, j].Valeur == '.')
                    {
                        for (int k = 0; k < sizeTable; k++)
                        {
                            char v = Symbole[k];
                            if (lines.isNotPresent(grid[i, j].Line, v) && columns.isNotPresent(grid[i, j].Column, v)
                                && squares.isNotPresent(grid[i, j].Square, v) && !grid.isHypothesePresent(i, j, v))
                            {
                                grid[i, j].addHypothese(v);
                            }
                        }
                    }
                }
            }
        }

        private void candidatUJT()
        {
            bool action = false;
            for (int i = 0; i < sizeTable; i++)
            {
                for (int j = 0; j < sizeTable; j++)
                {
                    Case c = grid[i, j];
                    if (c.Hypotheses.Count > 0)
                    {
                        if (seulCandidat(c) || candidatUnique(c) || jumeauxTriples(c))
                            action = true;
                    }
                }
            }
            if (action)
            {
                candidatUJT();
            }

            if (!grid.isComplete())
            {
                CandidatsI();
            }

            if (!grid.isComplete())
            {
                bruteForce();
            }
        }

        private bool seulCandidat(Case c)
        {
            if (c.Hypotheses.Count == 1)
            {
                c.Valeur = c.Hypotheses[0];
                reportValue(c);
                return true;
            }
            return false;
        }

        private bool candidatUnique(Case c)
        {
            bool action = false;
            foreach (char v in c.Hypotheses)
            {
                if (!lines.isHypothesePresent(c.Line, v) || !columns.isHypothesePresent(c.Column, v)
                    || !squares.isHypothesePresent(c.Square, v))
                {
                    c.Valeur = v;
                    reportValue(c);
                    action = true;
                    break;
                }
            }
            return action;
        }

        private bool jumeauxTriples(Case c)
        {
            bool action = false;
            foreach (char v in c.Hypotheses)
            {
                if (squares.isHPML(c.Square, c.Line, v))
                {
                    if (lines.clearHypothese(c.Line, c.Square, v))
                        action = true;
                }
                if (squares.isHPMC(c.Square, c.Column, v))
                {
                    if (columns.clearHypothese(c.Column, c.Square, v))
                        action = true;
                }
            }
            return action;
        }

        private void CandidatsI()
        {
            bool action = false;
            for (int i = 0; i < sizeTable; i++)
            {
                for (int j = 0; j < sizeTable; j++)
                {
                    if (grid[i, j].Hypotheses.Count < sizeTable)
                    {
                        if (lines.candidatsIdentiques(i, j) || columns.candidatsIdentiques(i, j) || squares.candidatsIdentiques(i, j))
                            action = true;
                    }
                }
            }
            if (action)
            {
                candidatUJT();
            }
        }

        private void bruteForce()
        {
            for (int i = 0; i < sizeTable; i++)
            {
                for (int j = 0; j < sizeTable; j++)
                {
                    Case c = grid[i, j];
                    if (c.Valeur == '.')
                    {
                        if (c.Hypotheses.Count > 0)
                        {
                            if (!caseIsPresentInCopy(c))
                            {
                                nbBruteForce++;
                                CopyGrid cg = new CopyGrid(new TableCase(grid), new Case(c), 0);
                                copyGrid.Add(cg);
                                c.Valeur = c.Hypotheses[0];
                                reportValue(c);
                                candidatUJT();
                                return;
                            }
                            else if (copyIsLast(c))
                            {
                                CopyGrid cg = copyGrid.Last();
                                if (cg.valueTest < c.Hypotheses.Count - 1)
                                {
                                    c.Valeur = c.Hypotheses[++cg.valueTest];
                                    reportValue(c);
                                    candidatUJT();
                                    return;
                                }
                                else
                                {
                                    copyGrid.Remove(cg);
                                    grid.reinit(copyGrid.Last().GridCopy);
                                    return;
                                }

                            }
                        }
                        else
                        {
                            if (nbBruteForce < 200)
                                grid.reinit(copyGrid.Last().GridCopy);
                            return;
                        }

                    }
                }
            }
        }

        private bool caseIsPresentInCopy(Case c)
        {
            foreach (CopyGrid cg in copyGrid)
            {
                if (c == cg.caseCopy)
                {
                    return true;
                }
            }
            return false;
        }

        private bool copyIsLast(Case c)
        {
            if (c == copyGrid.Last().caseCopy)
            {
                return true;
            }
            return false;
        }

        private void reportValue(Case c)
        {
            lines.clearHypothese(c.Line, c.Valeur);
            columns.clearHypothese(c.Column, c.Valeur);
            squares.clearHypothese(c.Square, c.Valeur);
            c.Hypotheses.Clear();
        }

        public int numSquare(int i, int j)
        {
            return ((i / rc) * rc) + (j / rc);
        }

        private int columSquare(int i, int j)
        {
            return ((i - ((i / rc) * rc)) * rc) + (j - ((j / rc) * rc));
        }

    }
}
