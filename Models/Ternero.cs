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

        public Ternero(string nombre, string rp, string rDeControl, string raza, string hba, string chapa, 
            string color, string fechaNac, string tambo, string padreRP, string padreHBA, string nombrePadre, string madreRP, 
            string madreHBA, string madreCAL, string nombreMadre, string fotoIzquierda, string fotoDerecha)
        {
            this.nombre = nombre;
            this.rp = rp;
            this.rDeControl = rDeControl;
            this.raza = raza;
            this.hba = hba;
            this.chapa = chapa;
            this.color = color;
            this.fechaNac = fechaNac;
            this.tambo = tambo;
            this.padreRP = padreRP;
            this.padreHBA = padreHBA;
            this.nombrePadre = nombrePadre;
            this.madreRP = madreRP;
            this.madreHBA = madreHBA;
            this.madreCAL = madreCAL;
            this.nombreMadre = nombreMadre;
            this.fotoIzquierda = fotoIzquierda;
            this.fotoDerecha = fotoDerecha;
        }


    }
}
