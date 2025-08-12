namespace GestorDePlanillas.Model
{
    internal class Ternero
    {
        public string nombre { get; set; }
        public string rp { get; set; }
        public string rDeControl { get; set; }
        public string raza { get; set; }
        public string hba { get; set; }
        public string chapa { get; set; }
        public string color { get; set; }
        public string fechaNac { get; set; } //hay que ver a la hora de importar el csv
        public string tambo { get; set; }
        public string padreRP { get; set; }
        public string padreHBA { get; set; }
        public string nombrePadre { get; set; }
        public string madreRP { get; set; }
        public string madreHBA { get; set; }
        public string madreCAL { get; set; }
        public string nombreMadre { get; set; }

        public string fotoIzquierda { get; set; }
        public string fotoDerecha { get; set; }
    }
}
