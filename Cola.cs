using System;
using System.Collections;
using System.Collections.Generic;

namespace AuditorioCongreso
{
    /// <summary>
    /// Nodo simple para la cola enlazada.
    /// </summary>
    public class Nodo<T>
    {
        public T Valor { get; set; }
        public Nodo<T> Siguiente { get; set; }

        public Nodo(T valor)
        {
            Valor = valor;
            Siguiente = null;
        }
    }

    /// <summary>
    /// Implementación propia del TAD Cola (FIFO) mediante nodos enlazados.
    /// Encolar y Desencolar operan en tiempo constante O(1) porque se
    /// mantienen referencias directas al frente y al final de la estructura.
    /// </summary>
    public class Cola<T> : IEnumerable<T>
    {
        private Nodo<T> frente;   // Primer elemento (el próximo en salir)
        private Nodo<T> final;    // Último elemento (el más reciente en entrar)

        public int Cantidad { get; private set; }

        public bool EstaVacia
        {
            get { return Cantidad == 0; }
        }

        /// <summary>Agrega un elemento al final de la cola. O(1)</summary>
        public void Encolar(T elemento)
        {
            Nodo<T> nuevo = new Nodo<T>(elemento);
            if (EstaVacia)
                frente = nuevo;
            else
                final.Siguiente = nuevo;
            final = nuevo;
            Cantidad++;
        }

        /// <summary>Retira y devuelve el elemento del frente. O(1)</summary>
        public T Desencolar()
        {
            if (EstaVacia)
                throw new InvalidOperationException("La cola está vacía.");
            T valor = frente.Valor;
            frente = frente.Siguiente;
            if (frente == null)
                final = null;
            Cantidad--;
            return valor;
        }

        /// <summary>Consulta el elemento del frente sin retirarlo. O(1)</summary>
        public T VerFrente()
        {
            if (EstaVacia)
                throw new InvalidOperationException("La cola está vacía.");
            return frente.Valor;
        }

        /// <summary>Permite recorrer la cola para la reportería sin modificarla.</summary>
        public IEnumerator<T> GetEnumerator()
        {
            Nodo<T> actual = frente;
            while (actual != null)
            {
                yield return actual.Valor;
                actual = actual.Siguiente;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
