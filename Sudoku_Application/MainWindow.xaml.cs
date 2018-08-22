using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Sudoku_Application
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.viewModelSudoku;
        }

        // Fonction permettant de charger un fichier de grille sudoku.
        private void Button_loadFile(object sender, RoutedEventArgs e)
        {
            // Ouverture d'une fenêtre de selection de fichier.
            OpenFileDialog openGridFile = new OpenFileDialog();
            openGridFile.Filter = "Sudoku File (.sud)|*.sud";

            bool? openFile = openGridFile.ShowDialog();

            if (openFile == true)
            {
                // Netoyage de la liste de grille.
                App.viewModelSudoku.clearListBox();
                String path = openGridFile.FileName;
                // Remplissage de la liste de grille.
                App.viewModelSudoku.getGridFile(path);
                button_ResolveGrid.IsEnabled = true;
                button_checkGrid.IsEnabled = true;
            }
        }

        // Fonction permettant d'afficher la grille sudoku que l'on a selectionner dans la Liste.
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Netoyage de la grille.
            FrontGrid.Children.Clear();
            FrontGrid.ColumnDefinitions.Clear();
            FrontGrid.RowDefinitions.Clear();
            Grille grid = App.viewModelSudoku.gridSelect;

            // Construction de la grille selectionnée.
            for (int i = 0; i < grid.Size; i++)
            {
                FrontGrid.ColumnDefinitions.Add(new ColumnDefinition());
                FrontGrid.RowDefinitions.Add(new RowDefinition());
            }
            // Remplissage des cases de la grille.
            for (int i = 0; i < grid.Size; i++)
            {
                for (int j = 0; j < grid.Size; j++)
                {
                    FrameworkElement b = createGridCase(grid, i, j);
                    FrontGrid.Children.Add(b);
                }
            }
        }

        // Fonction permettant de construire la grille selon l'état des cases.
        private FrameworkElement createGridCase(Grille g, int i, int j)
        {
            FrameworkElement b;
            char c = g.grid[i, j].Valeur;
            if (c == '.')
            {
                b = new Button();
                ((Button)b).Background = Brushes.LightSkyBlue;
            }
            else
            {
                b = new Button();
                ((Button)b).Background = Brushes.LightSeaGreen;
                ((Button)b).Content = c;
            }
            Grid.SetRow(b, i);
            Grid.SetColumn(b, j);
            return b;
        }

        // Fonction qui lance la résolution automatique de la grille.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Grille grid = App.viewModelSudoku.gridSelect;
            App.viewModelSudoku.completeGrilles(grid);
            for (int i = 0; i < grid.Size; i++)
            {
                for (int j = 0; j < grid.Size; j++)
                {
                    FrameworkElement b = createGridCase(grid, i, j);
                    FrontGrid.Children.Add(b);
                }
            }
        }

        // Fonction que vérifie que la grille est valide ou non.
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Grille grid = App.viewModelSudoku.gridSelect;
            App.viewModelSudoku.valideGrilles(grid);
        }
    }
}
