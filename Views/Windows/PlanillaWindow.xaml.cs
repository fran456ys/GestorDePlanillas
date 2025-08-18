using System.Windows;
using System.Windows.Media.Imaging;

namespace GestorDePlanillas.Views.Windows
{
    public partial class PlanillaWindow : Window
    {
        public PlanillaWindow(BitmapSource planilla)
        {
            InitializeComponent();
            imgPlanilla.Source = planilla;
        }
    }
}
