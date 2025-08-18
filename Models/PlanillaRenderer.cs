using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GestorDePlanillas.Model
{
    public enum TipoCampo
    {
        Texto,
        Imagen
    }

    public class CampoConfig
    {
        public string NombrePropiedad { get; set; }
        public TipoCampo Tipo { get; set; } = TipoCampo.Texto;

        // PARA TEXTO
        public Point Posicion { get; set; }
        public double TamañoFuente { get; set; } = 12;
        public string Fuente { get; set; } = "Arial";
        public Brush Color { get; set; } = Brushes.Black;
        public string Formato { get; set; }

        //PARA IMAGEN
        public Rect RectanguloImagen { get; set; }
        public bool MantenerAspecto { get; set; } = true;
        public bool RecortarImagen { get; set; } = false;
    }

    internal class PlanillaRenderer
    {
        private readonly Dictionary<string, CampoConfig> _configuracionCampos;
        private readonly Typeface _tipoFuenteDefault;
        private readonly double _dpi;

        public PlanillaRenderer()
        {
            _configuracionCampos = InicializarConfiguracionCampos();
            _tipoFuenteDefault = new Typeface("Arial");
            var dummyVisual = new DrawingVisual();
            _dpi = VisualTreeHelper.GetDpi(dummyVisual).PixelsPerDip;
        }

        private Dictionary<string, CampoConfig> InicializarConfiguracionCampos()
        {
            return new Dictionary<string, CampoConfig>
            {
                // Basándome en tu imagen, ajusta estas coordenadas:
                ["Nombre"] = new CampoConfig { NombrePropiedad = "nombre", Posicion = new Point(415, 88), TamañoFuente = 52 },
                ["RP"] = new CampoConfig { NombrePropiedad = "rp", Posicion = new Point(1606, 88), TamañoFuente = 52 },
                ["RDeControl"] = new CampoConfig { NombrePropiedad = "rDeControl", Posicion = new Point(2063, 87), TamañoFuente = 52 },
                ["Raza"] = new CampoConfig { NombrePropiedad = "raza", Posicion = new Point(405, 168), TamañoFuente = 52 },
                ["HBA"] = new CampoConfig { NombrePropiedad = "hba", Posicion = new Point(1657, 166), TamañoFuente = 52 },
                ["Chapa"] = new CampoConfig { NombrePropiedad = "chapa", Posicion = new Point(2070, 164), TamañoFuente = 52 },
                ["Color"] = new CampoConfig { NombrePropiedad = "color", Posicion = new Point(374, 250), TamañoFuente = 52 },
                ["FechaNacimiento"] = new CampoConfig { NombrePropiedad = "fechaNac", Posicion = new Point(1584, 250), TamañoFuente = 52, Formato = "MMM-yy" },
                ["Tambo"] = new CampoConfig { NombrePropiedad = "tambo", Posicion = new Point(2100, 248), TamañoFuente = 52 },

                // Datos del padre
                ["PadreRP"] = new CampoConfig { NombrePropiedad = "padreRP", Posicion = new Point(424, 1168), TamañoFuente = 52 },
                ["PadreHBA"] = new CampoConfig { NombrePropiedad = "padreHBA", Posicion = new Point(781, 1168), TamañoFuente = 52 },
                ["PadreNombre"] = new CampoConfig { NombrePropiedad = "nombrePadre", Posicion = new Point(412, 1250), TamañoFuente = 52 },

                // Datos de la madre
                ["MadreRP"] = new CampoConfig { NombrePropiedad = "madreRP", Posicion = new Point(1491, 1169), TamañoFuente = 52 },
                ["MadreHBA"] = new CampoConfig { NombrePropiedad = "madreHBA", Posicion = new Point(1854, 1169), TamañoFuente = 52 },
                ["MadreCAL"] = new CampoConfig { NombrePropiedad = "madreCAL", Posicion = new Point(2271, 1169), TamañoFuente = 52 },
                ["MadreNombre"] = new CampoConfig { NombrePropiedad = "nombreMadre", Posicion = new Point(1460, 1253), TamañoFuente = 52 },

                ["FotoIzquierda"] = new CampoConfig
                {
                    NombrePropiedad = "fotoIzquierda",
                    Tipo = TipoCampo.Imagen,
                    RectanguloImagen = new Rect(430, 478, 663, 466), // x, y, ancho, alto
                    MantenerAspecto = true,
                    RecortarImagen = true
                },
                ["FotoDerecha"] = new CampoConfig
                {
                    NombrePropiedad = "fotoDerecha",
                    Tipo = TipoCampo.Imagen,
                    RectanguloImagen = new Rect(1509, 478, 663, 466),
                    MantenerAspecto = true,
                    RecortarImagen = true
                }
            };
        }

        public BitmapSource GenerarPlanilla(Ternero ternero, string rutaPlantilla)
        {
            // Cargar plantilla
            var plantilla = new BitmapImage(new Uri(rutaPlantilla, UriKind.RelativeOrAbsolute));

            // Crear superficie de dibujo
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                // Dibujar imagen base
                dc.DrawImage(plantilla, new Rect(0, 0, plantilla.PixelWidth, plantilla.PixelHeight));

                // Renderizar todos los campos automáticamente
                RenderizarCampos(dc, ternero);
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

        private void RenderizarCampos(DrawingContext dc, Ternero ternero)
        {
            var tipo = typeof(Ternero);
            var propiedades = tipo.GetProperties();

            foreach (var config in _configuracionCampos.Values)
            {
                var propiedad = tipo.GetProperty(config.NombrePropiedad, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propiedad != null)
                {
                    var valor = propiedad.GetValue(ternero);

                    if (config.Tipo == TipoCampo.Texto)
                    {

                        var textoMostrar = FormatearValor(valor, config.Formato);

                        if (!string.IsNullOrEmpty(textoMostrar))
                        {
                            DibujarTexto(dc, textoMostrar, config);
                        }
                    }
                    else{
                        DibujarImagen(dc, valor, config);
                    }
                }
            }
        }

        private void DibujarImagen(DrawingContext dc, object rutaImagen, CampoConfig config)
        {
            if (rutaImagen == null)
            {
                DibujarPlaceholderImagen(dc, config.RectanguloImagen, "Sin ruta de imagen valida");
                return;
            }

            string ruta = rutaImagen.ToString();
            if (string.IsNullOrEmpty(ruta))
            {
                DibujarPlaceholderImagen(dc, config.RectanguloImagen, "Ruta vacía");
                return;
            }

            if (!File.Exists(ruta))
            {
                DibujarPlaceholderImagen(dc, config.RectanguloImagen, $"Archivo no encontrado:\n{Path.GetFileName(ruta)}");
                return;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(ruta, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                if (config.RecortarImagen)
                {
                    // Modo recorte: ajusta la imagen exactamente al rectángulo
                    dc.DrawImage(bitmap, config.RectanguloImagen);
                }
                else if (config.MantenerAspecto)
                {
                    // Modo mantener aspecto: calcula el rectángulo para mantener proporciones
                    var rectAjustado = CalcularRectanguloConAspecto(bitmap, config.RectanguloImagen);
                    dc.DrawImage(bitmap, rectAjustado);
                }
                else
                {
                    // Modo estiramiento: usa el rectángulo completo sin mantener aspecto
                    dc.DrawImage(bitmap, config.RectanguloImagen);
                }
            }
            catch (Exception ex)
            {
                DibujarPlaceholderImagen(dc, config.RectanguloImagen, $"Error cargando imagen:\n{ex.Message}");
            }
        }

        private void DibujarPlaceholderImagen(DrawingContext dc, Rect rectangulo, string mensaje = "Sin imagen")
        {
            // Fondo gris claro
            dc.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Gray, 2), rectangulo);

            // Texto centrado
            var texto = new FormattedText(
                mensaje,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                _tipoFuenteDefault,
                24, // Tamaño más grande para que se vea
                Brushes.DarkRed, // Color rojo para errores
                _dpi
            );

            var posicionTexto = new Point(
                rectangulo.X + (rectangulo.Width - texto.Width) / 2,
                rectangulo.Y + (rectangulo.Height - texto.Height) / 2
            );

            dc.DrawText(texto, posicionTexto);
        }

        private Rect CalcularRectanguloConAspecto(BitmapSource bitmap, Rect rectanguloDestino)
        {
            double aspectoImagen = (double)bitmap.PixelWidth / bitmap.PixelHeight;
            double aspectoDestino = rectanguloDestino.Width / rectanguloDestino.Height;

            double nuevoAncho, nuevoAlto;

            if (aspectoImagen > aspectoDestino)
            {
                nuevoAncho = rectanguloDestino.Width;
                nuevoAlto = rectanguloDestino.Width / aspectoImagen;
            }
            else
            {
                nuevoAlto = rectanguloDestino.Height;
                nuevoAncho = rectanguloDestino.Height * aspectoImagen;
            }

            double x = rectanguloDestino.X + (rectanguloDestino.Width - nuevoAncho) / 2;
            double y = rectanguloDestino.Y + (rectanguloDestino.Height - nuevoAlto) / 2;

            return new Rect(x, y, nuevoAncho, nuevoAlto);
        }


        private void DibujarTexto(DrawingContext dc, string texto, CampoConfig config)
        {
            var tipoFuente = new Typeface(config.Fuente);

            var textoFormateado = new FormattedText(
                texto,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                tipoFuente,
                config.TamañoFuente,
                config.Color,
                _dpi
            );

            dc.DrawText(textoFormateado, config.Posicion);
        }

        private string FormatearValor(object valor, string formato)
        {
            if (valor == null) return string.Empty;

            // Formateo especial para fechas
            if (valor is DateTime fecha && !string.IsNullOrEmpty(formato))
            {
                return fecha.ToString(formato, CultureInfo.InvariantCulture);
            }

            return valor.ToString();
        }

        public List<BitmapSource> GenerarPlanillasLote(List<Ternero> terneros, string rutaPlantilla)
        {
            var resultados = new List<BitmapSource>();

            // Cargar plantilla una sola vez
            var plantilla = new BitmapImage(new Uri(rutaPlantilla, UriKind.RelativeOrAbsolute));

            foreach (var ternero in terneros)
            {
                var bmp = GenerarPlanillaConPlantillaCargada(ternero, plantilla);
                resultados.Add(bmp);
            }

            return resultados;
        }
        private BitmapSource GenerarPlanillaConPlantillaCargada(Ternero ternero, BitmapImage plantilla)
        {
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawImage(plantilla, new Rect(0, 0, plantilla.PixelWidth, plantilla.PixelHeight));
                RenderizarCampos(dc, ternero);
            }

            var bmp = new RenderTargetBitmap(
                plantilla.PixelWidth,
                plantilla.PixelHeight,
                96, 96,
                PixelFormats.Pbgra32
            );
            bmp.Render(visual);

            return bmp;
        }

        public void ActualizarPosicionCampo(string nombreCampo, Point nuevaPosicion)
        {
            if (_configuracionCampos.ContainsKey(nombreCampo))
            {
                _configuracionCampos[nombreCampo].Posicion = nuevaPosicion;
            }
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

        public void GuardarPlanillasLote(List<Ternero> terneros, string rutaPlantilla, string carpetaSalida)
        {
            var planillas = GenerarPlanillasLote(terneros, rutaPlantilla);

            for (int i = 0; i < planillas.Count; i++)
            {
                var nombreArchivo = $"{terneros[i].nombre}_{terneros[i].rp}.png";
                var rutaCompleta = Path.Combine(carpetaSalida, nombreArchivo);
                GuardarComoPng(planillas[i], rutaCompleta);
            }
        }
    }
}
