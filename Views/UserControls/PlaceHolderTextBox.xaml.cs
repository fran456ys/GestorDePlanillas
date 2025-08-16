using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestorDePlanillas.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para PlaceHolderTextBox.xaml
    /// </summary>
    public partial class PlaceHolderTextBox : UserControl
    {
        private string selectedFileName = string.Empty;
        public string SelectedFileName => selectedFileName;

        public event EventHandler<string> ArchivoSeleccionado;

        public PlaceHolderTextBox()
        {
            InitializeComponent();
            UpdatePlaceholder();
        }

        private void UpdatePlaceholder()
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                tbPlaceHolder.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceHolder.Visibility = Visibility.Hidden;
            }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholder();
        }

        private void btnArchivos_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "Archivos xlsx (*.xlsx)|*.xlsx|Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*"
            };

            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                selectedFileName = fileDialog.FileName;
                txtInput.Text = selectedFileName;

                ArchivoSeleccionado?.Invoke(this, selectedFileName);
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo.", "Información",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void txtInput_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Solo permitir teclas de borrado y navegación
            if (e.Key != Key.Delete &&
                e.Key != Key.Back &&
                e.Key != Key.Left &&
                e.Key != Key.Right &&
                e.Key != Key.Home &&
                e.Key != Key.End &&
                !(e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control) &&
                !(e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control))
            {
                e.Handled = true; // Bloquear escritura de nuevos caracteres
            }
        }

        private void txtInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
