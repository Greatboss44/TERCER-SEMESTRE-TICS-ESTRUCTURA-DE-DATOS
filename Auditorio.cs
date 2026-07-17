using System;
using System.Collections.Generic;

namespace AuditorioCongreso
{
    /// <summary>
    /// Administra los 100 asientos del auditorio y su asignación secuencial,
    /// de modo que el orden de los asientos refleje el orden de llegada.
    /// </summary>
    public class Auditorio
    {
        public const int CAPACIDAD = 100;

        private Asiento[] asientos;
        private int proximoAsiento;   // Índice del siguiente asiento libre

        public Auditorio()
        {
            asientos = new Asiento[CAPACIDAD];
            for (int i = 0; i < CAPACIDAD; i++)
                asientos[i] = new Asiento(i + 1);
            proximoAsiento = 0;
        }

        public int AsientosOcupados
        {
            get { return proximoAsiento; }
        }

        public int AsientosDisponibles
        {
            get { return CAPACIDAD - proximoAsiento; }
        }

        public bool EstaLleno
        {
            get { return proximoAsiento >= CAPACIDAD; }
        }

        /// <summary>
        /// Asigna el siguiente asiento libre (en orden 1..100) al asistente.
        /// Devuelve el asiento asignado o null si el auditorio está lleno.
        /// </summary>
        public Asiento AsignarSiguiente(Asistente asistente)
        {
            if (EstaLleno)
                return null;
            Asiento asiento = asientos[proximoAsiento];
            asiento.Asignar(asistente);
            proximoAsiento++;
            return asiento;
        }

        /// <summary>Devuelve los asientos ya ocupados para la reportería.</summary>
        public IEnumerable<Asiento> ObtenerOcupados()
        {
            for (int i = 0; i < proximoAsiento; i++)
                yield return asientos[i];
        }

        /// <summary>Busca a un asistente ya ubicado por su número de cédula.</summary>
        public Asiento BuscarPorCedula(string cedula)
        {
            for (int i = 0; i < proximoAsiento; i++)
                if (asientos[i].Ocupante.Cedula == cedula)
                    return asientos[i];
            return null;
        }
    }
}
