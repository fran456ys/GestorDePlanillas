using GestorDePlanillas.Model;
using GestorDePlanillas.Views.UserControls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GestorDePlanillas
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        


        private void StackPanelDashBoard_ImportarExcelClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ImportarExcelView();
        }

        private void StackPanelDashBoard_CargarTerneroClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CargarTerneroView();
        }

        private void StackPanelDashBoard_ReportesClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReportesView();
        }
    }
}
