using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestorDePlanillas.Model
{
    internal class Importador
    {
        public List<Ternero> importarCsv(string filePath)
        {
            var terneros = new List<Ternero>();

            var lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1) return terneros;

            foreach (var line in lines.Skip(1))
            {
                var valores = line.Split(';');

                if (valores.Length >= 17)
                {
                    terneros.Add(new Ternero
                    (
                        valores[0],//  nombre 
                        valores[1],//  rp 
                        valores[2],//  rDeControl 
                        valores[3],//  raza 
                        valores[4],//  hba 
                        valores[5],//  chapa 
                        valores[6],//  color 
                        valores[7],//  fechaNac 
                        valores[8],//  tambo 
                        valores[9],//  padreRP 
                        valores[10],// padreHBA 
                        valores[11],// nombrePadre 
                        valores[12],// madreRP 
                        valores[13],// madreHBA 
                        valores[14],// madreCAL 
                        valores[15],// nombreMadre 
                        valores[16],// fotoIzquierda 
                        valores[17] // fotoDerecha 
                    ));
                }
            }
            return terneros;
        }
    }
}
