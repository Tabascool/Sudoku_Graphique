using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku_Application
{
    public class SudokuViewModel
    {
        public string nameApp { get; set; }

        public ObservableCollection<Grille> GridList { get; set; }

        String _path;

        System.IO.StreamReader file;

        int gridCount = 0;

        public Grille gridSelect { get; set; }

        public SudokuViewModel()
        {
            nameApp = "Application Sudoku";
            GridList = new ObservableCollection<Grille>();
            _path = null;
        }

        // Fonction qui récupère les données du fichier sélectionné et affiche chaque grille dans la liste.
        public void getGridFile(String path)
        {
            _path = path;
            if (readFile())
            {
                Grille g;
                Case c;
                string line;
                int tailleGrille;
                //Lire ligne par ligne
                while ((line = file.ReadLine()) != null)
                {
                    gridCount++;
                    g = new Grille();
                    g.Nom = file.ReadLine();
                    g.Date = file.ReadLine();
                    g.Symbole = file.ReadLine();
                    tailleGrille = g.Symbole.Length;
                    g.initializeTab(tailleGrille);
                    for (int i = 0; i < tailleGrille; i++)
                    {
                        string values = file.ReadLine();
                        for (int j = 0; j < values.Length; j++)
                        {
                            c = new Case(values[j], i, j, g.numSquare(i, j));
                            g.setTable(i, j, c);
                        }

                    }

                    //ajouter la grille créée à partir du fichier
                    GridList.Add(g);
                }
                //fermer le fichier
                file.Close();
            }
        }

        public void valideGrilles(Grille grid)
        {
                grid.valideGrille();
        }

        public void completeGrilles(Grille grid)
        {
                grid.completeGrille();
        }

        // Fonction qui vérifie si le fichier sélectionné est bon.
        private bool readFile()
        {
            try
            {
                //Lire le fichier
                file = new System.IO.StreamReader(_path);
                return true;
            }
            catch (Exception e)
            {
                String caption = "Erreur fichier";
                String message = "Une erreur est survenu lors de la lecture du fichier: ";
                Console.WriteLine(message);
                Console.WriteLine(e.Message);
                MessageBox.Show(message, caption);
                return false;
            }
        }

        // Fonction qui vide la liste.
        internal void clearListBox()
        {
            if (GridList.Count > 0)
            {
                for (int i = 0; i <= GridList.Count; i++)
                {
                    if ((i % 2 == 0 && i > 0) || i == 1)
                        i--;
                    GridList.RemoveAt(i);
                }
            }
        }
    }
}
