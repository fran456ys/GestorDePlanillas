using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GestorDePlanillas.Model
{
    internal class PlanillaRenderer
    {
        public BitmapSource GenerarPlanilla(Ternero t, string rutaPlantilla)
        {
            // Cargar plantilla
            var plantilla = new BitmapImage(new Uri(rutaPlantilla, UriKind.RelativeOrAbsolute));

            // Crear superficie de dibujo
            var visual = new DrawingVisual();

            // Obtener DPI (pixelsPerDip)
            var dpi = VisualTreeHelper.GetDpi(visual).PixelsPerDip;

            using (var dc = visual.RenderOpen())
            {
                // Dibujar imagen base
                dc.DrawImage(plantilla, new Rect(0, 0, plantilla.PixelWidth, plantilla.PixelHeight));

                // Configuración de texto
                var tipo = new Typeface("Arial");
                var brush = Brushes.Black;

                // Ejemplo: Nombre
                dc.DrawText(
                    new FormattedText(
                        t.nombre,
                        CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        tipo,
                        20,
                        brush,
                        dpi // PixelsPerDip
                    ),
                    new Point(100, 150) // posición
                );

                
            }

            // Renderizar a BitmapSource
            var bmp = new RenderTargetBitmap(
                plantilla.PixelWidth,
                plantilla.PixelHeight,
                96, 96,
                PixelFormats.Pbgra32
            );
            bmp.Render(visual);

            return bmp;
        }

        public void GuardarComoPng(BitmapSource bmp, string rutaSalida)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (var fs = new FileStream(rutaSalida, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }
    }
}
