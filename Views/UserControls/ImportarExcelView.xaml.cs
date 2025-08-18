using GestorDePlanillas.Model;
using GestorDePlanillas.Views.Windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GestorDePlanillas.Views.UserControls
{
    public partial class ImportarExcelView : UserControl
    {
        private ObservableCollection<Ternero> terneros = new();
        private Importador importador = new Importador();
        public ImportarExcelView()
        {
            InitializeComponent();
            dgTerneros.ItemsSource = terneros;
            tbImportador.ArchivoSeleccionado += TbImportador_ArchivoSeleccionado;
        }
        private void TbImportador_ArchivoSeleccionado(object sender, string filePath)
        {
            try
            {
                var terneros = this.importador.importarCsv(filePath);
                dgTerneros.ItemsSource = terneros;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al importar archivo: {ex.Message}");
            }
        }

        private void dgTerneros_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "fotoIzquierda" || e.PropertyName == "fotoDerecha")
            {
                e.Cancel = true; // oculta esas columnas
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (dgTerneros.SelectedItem is Ternero seleccionado)
            {
                PlanillaRenderer renderer = new PlanillaRenderer();
                var planillaGenerada = renderer.GenerarPlanilla(
                    seleccionado,
                    "Assets/planillaVacia.jpg"
                );

                // Abrir nueva ventana con la planilla
                var ventanaPlanilla = new PlanillaWindow(planillaGenerada);
                ventanaPlanilla.ShowDialog(); // o ShowDialog() si querés modal
            }
            else
            {
                MessageBox.Show("Seleccione un ternero primero.");
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            var menu = new ContextMenu();

            var imprimirSeleccionado = new MenuItem() { Header = "Imprimir ternero seleccionado" };
            menu.Items.Add(imprimirSeleccionado);

            var imprimirTodos = new MenuItem() { Header = "Imprimir todos los terneros" };
            menu.Items.Add(imprimirTodos);

            menu.PlacementTarget = sender as Button;
            menu.IsOpen = true;
        }
    }
}
