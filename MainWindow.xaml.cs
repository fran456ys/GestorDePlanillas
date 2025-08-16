using GestorDePlanillas.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestorDePlanillas
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Ternero> terneros = new();
        private Importador importador = new Importador();

        public MainWindow()
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
    }
}