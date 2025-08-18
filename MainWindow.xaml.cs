using GestorDePlanillas.Model;
using GestorDePlanillas.Views.UserControls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GestorDePlanillas
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Ternero> terneros = new();
        private Importador importador = new Importador();

        public MainWindow()
        {
            InitializeComponent();
            //dgTerneros.ItemsSource = terneros;
            //tbImportador.ArchivoSeleccionado += TbImportador_ArchivoSeleccionado;   
        }
        /*
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
        }*/

        private void dgTerneros_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "fotoIzquierda" || e.PropertyName == "fotoDerecha")
            {
                e.Cancel = true; // oculta esas columnas
            }
        }

        private void StackPanelDashBoard_ImportarExcelClick(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanelDashBoard_CargarTerneroClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CargarTerneroView();
        }

        private void StackPanelDashBoard_ReportesClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

/*
 * <Grid Grid.Column="2" Margin="10">
            <Grid.RowDefinitions>
                <RowDefinition Height="100"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>

            <UserControls:PlaceHolderTextBox Width="800" x:Name="tbImportador" HorizontalAlignment="Left"/>

            <DataGrid x:Name="dgTerneros" Grid.Row="1" Width="1300" HorizontalAlignment="Left"
                      AutoGenerateColumns="True" IsReadOnly="True" Background="White" AutoGeneratingColumn="dgTerneros_AutoGeneratingColumn"
                      HorizontalGridLinesBrush="#B8CFCE" />

            <ui:SimpleStackPanel Grid.Row="1" HorizontalAlignment="Right" Width="300" Margin="35">
                <Button Style="{StaticResource StackButton}" Content="Visualizar Planilla"/>
                <Button Style="{StaticResource StackButton}" Content="Descargar Planillas"/>
                <Button Style="{StaticResource StackButton}" Content="Imprimir Planillas"/>
            </ui:SimpleStackPanel>
        </Grid>
*/