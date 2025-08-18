using System.Windows;
using System.Windows.Controls;

namespace GestorDePlanillas.Views.UserControls
{
    public partial class StackPanelDashBoard : UserControl
    {
        public event RoutedEventHandler ImportarExcelClick;
        public event RoutedEventHandler CargarTerneroClick;
        public event RoutedEventHandler ReportesClick;

        public StackPanelDashBoard()
        {
            InitializeComponent();
        }

        private void btnCargarTernero_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CargarTerneroClick?.Invoke(this, e);
        }

        private void btnImportarExcel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ImportarExcelClick?.Invoke(this, e);
        }

        private void btnReportes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReportesClick?.Invoke(this, e);
        }
    }
}
