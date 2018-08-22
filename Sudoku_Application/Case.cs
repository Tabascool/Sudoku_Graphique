using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Application
{
    public class Case
    {
        public char Valeur { get; set; }
        public List<char> Hypotheses { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int Square { get; set; }

        public Case()
        {
            Hypotheses = new List<char>();
        }

        public Case(char value, int line, int column, int square)
        {
            Valeur = value;
            this.Line = line;
            this.Column = column;
            this.Square = square;
            Hypotheses = new List<char>();
        }

        public Case(Case c)
        {
            Hypotheses = new List<char>();
            Valeur = c.Valeur;
            Line = c.Line;
            Column = c.Column;
            Square = c.Square;
            for (int i = 0; i < c.Hypotheses.Count; i++)
            {
                Hypotheses.Add(c.Hypotheses[i]);
            }
        }

        public void addHypothese(char h)
        {
            Hypotheses.Add(h);
        }

        static public bool operator ==(Case c1, Case c2)
        {
            return (c1.Line == c2.Line && c1.Column == c2.Column && c1.Square == c2.Square);
        }
        static public bool operator !=(Case c1, Case c2)
        {
            return !(c1 == c2);
        }

    }
}
