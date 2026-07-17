namespace AuditorioCongreso
{
    /// <summary>
    /// Representa un asiento físico del auditorio.
    /// </summary>
    public class Asiento
    {
        public int Numero { get; private set; }
        public bool Ocupado { get; private set; }
        public Asistente Ocupante { get; private set; }

        public Asiento(int numero)
        {
            Numero = numero;
            Ocupado = false;
            Ocupante = null;
        }

        /// <summary>Asigna el asiento a un asistente registrado.</summary>
        public void Asignar(Asistente asistente)
        {
            Ocupante = asistente;
            Ocupado = true;
        }
    }
}
