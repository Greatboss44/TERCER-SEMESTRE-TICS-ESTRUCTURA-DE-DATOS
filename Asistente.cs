using System;

namespace AuditorioCongreso
{
    /// <summary>
    /// Representa a una persona que llega al congreso y espera en la fila
    /// para ser registrada por una de las dos ventanillas.
    /// </summary>
    public class Asistente
    {
        public int Turno { get; private set; }          // Orden global de llegada
        public string Cedula { get; private set; }
        public string Nombre { get; private set; }
        public DateTime HoraLlegada { get; private set; }
        public int Ventanilla { get; private set; }     // Fila en la que espera (1 o 2)

        public Asistente(int turno, string cedula, string nombre, int ventanilla)
        {
            Turno = turno;
            Cedula = cedula;
            Nombre = nombre;
            Ventanilla = ventanilla;
            HoraLlegada = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("Turno {0,3} | {1,-10} | {2,-28} | Fila {3} | {4:HH:mm:ss}",
                Turno, Cedula, Nombre, Ventanilla, HoraLlegada);
        }
    }
}
