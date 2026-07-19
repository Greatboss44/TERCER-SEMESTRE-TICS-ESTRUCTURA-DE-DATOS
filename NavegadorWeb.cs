using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NavegadorWeb
{
    /// <summary>
    /// Representa una página visitada dentro del navegador.
    /// Se modela como clase para poder añadir atributos adicionales
    /// (título, hora de visita) sin alterar el resto del programa.
    /// </summary>
    public class Pagina
    {
        public string Url { get; set; }
        public string Titulo { get; set; }
        public DateTime HoraVisita { get; set; }

        public Pagina(string url, string titulo)
        {
            Url = url;
            Titulo = titulo;
            HoraVisita = DateTime.Now;
        }

        // Representación legible usada por la reportería.
        public override string ToString()
        {
            return $"{Titulo,-28} | {Url,-32} | {HoraVisita:HH:mm:ss}";
        }
    }

    /// <summary>
    /// Historial del navegador. Implementa el comportamiento del botón
    /// "Retroceder" y "Adelante" mediante DOS pilas (estructura LIFO):
    ///   - pilaAtras:    páginas a las que se puede retroceder.
    ///   - pilaAdelante: páginas a las que se puede avanzar.
    /// La página en curso se mantiene aparte. Este es exactamente el
    /// modelo que usan los navegadores reales (Chrome, Firefox, Edge).
    /// </summary>
    public class HistorialNavegador
    {
        private readonly Stack<Pagina> pilaAtras = new Stack<Pagina>();
        private readonly Stack<Pagina> pilaAdelante = new Stack<Pagina>();
        private Pagina paginaActual;

        public Pagina PaginaActual => paginaActual;
        public bool PuedeRetroceder => pilaAtras.Count > 0;
        public bool PuedeAvanzar => pilaAdelante.Count > 0;

        /// <summary>
        /// Navega a una nueva página. La página actual se apila en "atrás"
        /// (LIFO: la última visitada será la primera en recuperarse) y se
        /// limpia la pila "adelante", igual que un navegador real.
        /// </summary>
        public void Navegar(string url, string titulo)
        {
            if (paginaActual != null)
                pilaAtras.Push(paginaActual);

            paginaActual = new Pagina(url, titulo);
            pilaAdelante.Clear();
            Console.WriteLine($"  -> Navegando a: {paginaActual.Titulo} ({paginaActual.Url})");
        }

        /// <summary>
        /// Botón RETROCEDER. Saca (Pop) la última página de la pila "atrás"
        /// y la vuelve la página actual; la página actual pasa a "adelante".
        /// </summary>
        public bool Retroceder()
        {
            if (!PuedeRetroceder)
            {
                Console.WriteLine("  [!] No hay páginas anteriores. La pila 'atrás' está vacía.");
                return false;
            }
            pilaAdelante.Push(paginaActual);
            paginaActual = pilaAtras.Pop();
            Console.WriteLine($"  <- Retrocediendo a: {paginaActual.Titulo} ({paginaActual.Url})");
            return true;
        }

        /// <summary>
        /// Botón ADELANTE. Operación inversa a Retroceder.
        /// </summary>
        public bool Avanzar()
        {
            if (!PuedeAvanzar)
            {
                Console.WriteLine("  [!] No hay páginas siguientes. La pila 'adelante' está vacía.");
                return false;
            }
            pilaAtras.Push(paginaActual);
            paginaActual = pilaAdelante.Pop();
            Console.WriteLine($"  -> Avanzando a: {paginaActual.Titulo} ({paginaActual.Url})");
            return true;
        }

        // ---------- REPORTERÍA ----------

        /// <summary>Muestra el estado completo de las tres partes del historial.</summary>
        public void MostrarEstado()
        {
            Console.WriteLine("\n========== REPORTE DE ESTADO DEL HISTORIAL ==========");
            Console.WriteLine($"Página actual : {(paginaActual != null ? paginaActual.Titulo : "(ninguna)")}");
            Console.WriteLine($"Puede retroceder: {PuedeRetroceder}   |   Puede avanzar: {PuedeAvanzar}");

            Console.WriteLine($"\n-- Pila ATRÁS (tope = más reciente) [{pilaAtras.Count} elementos] --");
            ImprimirPila(pilaAtras);

            Console.WriteLine($"\n-- Pila ADELANTE (tope = más reciente) [{pilaAdelante.Count} elementos] --");
            ImprimirPila(pilaAdelante);
            Console.WriteLine("=====================================================\n");
        }

        // Recorre una pila sin destruirla para efectos de consulta.
        private void ImprimirPila(Stack<Pagina> pila)
        {
            if (pila.Count == 0)
            {
                Console.WriteLine("   (vacía)");
                return;
            }
            int i = 1;
            foreach (Pagina p in pila) // Stack se recorre desde el tope hacia la base
                Console.WriteLine($"   {i++}. {p}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("############################################################");
            Console.WriteLine("#  SIMULADOR DEL BOTON RETROCEDER DE UN NAVEGADOR WEB       #");
            Console.WriteLine("#  Estructura de Datos - Pila (LIFO) en C# con POO         #");
            Console.WriteLine("############################################################\n");

            var historial = new HistorialNavegador();

            // --- Simulación de una sesión de navegación real ---
            Console.WriteLine(">> El usuario abre el navegador y visita varias páginas:");
            historial.Navegar("https://www.google.com", "Google");
            historial.Navegar("https://www.wikipedia.org", "Wikipedia");
            historial.Navegar("https://www.uea.edu.ec", "UEA - Inicio");
            historial.Navegar("https://aula.uea.edu.ec", "Aula Virtual");

            historial.MostrarEstado();

            Console.WriteLine(">> El usuario pulsa RETROCEDER dos veces:");
            historial.Retroceder();
            historial.Retroceder();

            historial.MostrarEstado();

            Console.WriteLine(">> El usuario pulsa ADELANTE una vez:");
            historial.Avanzar();

            Console.WriteLine("\n>> El usuario visita una NUEVA página (se borra la pila 'adelante'):");
            historial.Navegar("https://github.com", "GitHub");

            historial.MostrarEstado();

            Console.WriteLine(">> El usuario intenta retroceder hasta el inicio y más allá:");
            while (historial.Retroceder()) { }

            // --- Análisis del tiempo de ejecución de las operaciones de la pila ---
            MedirTiempos();

            Console.WriteLine("Fin de la simulación.");
        }

        /// <summary>
        /// Demuestra empíricamente que Push/Pop de la pila son O(1):
        /// el tiempo no depende de la cantidad de elementos ya almacenados.
        /// </summary>
        static void MedirTiempos()
        {
            Console.WriteLine("\n========== ANÁLISIS DE TIEMPO DE EJECUCIÓN ==========");
            var pila = new Stack<Pagina>();
            var sw = new Stopwatch();

            int[] tamanos = { 1000, 10000, 100000, 1000000 };
            foreach (int n in tamanos)
            {
                pila.Clear();
                sw.Restart();
                for (int i = 0; i < n; i++)
                    pila.Push(new Pagina($"https://sitio{i}.com", $"Sitio {i}"));
                sw.Stop();
                double push = sw.Elapsed.TotalMilliseconds;

                sw.Restart();
                while (pila.Count > 0) pila.Pop();
                sw.Stop();
                double pop = sw.Elapsed.TotalMilliseconds;

                Console.WriteLine($"n = {n,8} | Push total: {push,8:F2} ms | Pop total: {pop,8:F2} ms");
            }
            Console.WriteLine("Cada Push/Pop individual es O(1) (tiempo constante).");
            Console.WriteLine("=====================================================\n");
        }
    }
}
