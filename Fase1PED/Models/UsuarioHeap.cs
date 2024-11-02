using System.Collections.Generic;

namespace Fase1PED.Models
{
    public class UsuarioHeap
    {
        // Almacenamos los Usuarios en nuestro HEAP
        private List<Usuario> heap = new List<Usuario>();

        public enum TipoHeap { Bueno, Malo, Incobrable }
        private TipoHeap tipoHeap;

        //Guardamos el tipo de Usuarios en el constructor UsuarioHeap
        public UsuarioHeap(TipoHeap tipo)
        {
            tipoHeap = tipo;
        }

        // Agregamos un nuevo usuario 
        public void Insertar(Usuario usuario)
        {
            heap.Add(usuario);
            Subir(heap.Count - 1); // Ajustar la posición del nuevo elemento
        }

        // Obtener el usuario con mayor prioridad
        public Usuario ObtenerMayorPrioridad()
        {
            if (heap.Count == 0) return null;
            return heap[0];
        }

        // Eliminar usuario de mayor prioridad
        public void EliminarMayorPrioridad()
        {
            if (heap.Count == 0) return;
            Intercambiar(0, heap.Count - 1);
            heap.RemoveAt(heap.Count - 1);
            Bajar(0);
        }

        // Ajustar el heap cuando se inserta un elemento
        private void Subir(int index)
        {
            while (index > 0)
            {
                int padreIndex = (index - 1) / 2;
                if (Comparar(heap[index], heap[padreIndex]) > 0)
                {
                    Intercambiar(index, padreIndex);
                    index = padreIndex;
                }
                else
                {
                    break;
                }
            }
        }

        // Ajustar el heap cuando se elimina o se modifica un elemento
        private void Bajar(int index)
        {
            int ultimoIndex = heap.Count - 1;
            while (true)
            {
                int hijoIzqIndex = 2 * index + 1;
                int hijoDerIndex = 2 * index + 2;
                int mayorIndex = index;

                if (hijoIzqIndex <= ultimoIndex && Comparar(heap[hijoIzqIndex], heap[mayorIndex]) > 0)
                {
                    mayorIndex = hijoIzqIndex;
                }

                if (hijoDerIndex <= ultimoIndex && Comparar(heap[hijoDerIndex], heap[mayorIndex]) > 0)
                {
                    mayorIndex = hijoDerIndex;
                }

                if (mayorIndex == index)
                {
                    break;
                }

                Intercambiar(index, mayorIndex);
                index = mayorIndex;
            }
        }

        // Método para intercambiar dos elementos en el heap
        private void Intercambiar(int index1, int index2)
        {
            Usuario temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }

        // Método para comparar usuarios según múltiples criterios
        private int Comparar(Usuario u1, Usuario u2)
        {
            // Utiliza la lógica correcta según el tipo de heap
            if (tipoHeap == TipoHeap.Bueno)
            {
                return CompararClientesBuenos(u1, u2);
            }
            else if (tipoHeap == TipoHeap.Malo)
            {
                return CompararClientesMalos(u1, u2);
            }
            else // TipoHeap.Incobrable
            {
                return CompararClientesIncobrables(u1, u2);
            }
        }

        // Método para comparar usuarios buenos (A y B)
        private int CompararClientesBuenos(Usuario u1, Usuario u2)
        {
            // 1. Prioridad por Pago Pendiente (los que no tienen pagos pendientes tienen más prioridad)
            if (!u1.PagoPendiente && u2.PagoPendiente)
                return 1; // u1 tiene más prioridad
            if (u1.PagoPendiente && !u2.PagoPendiente)
                return -1; // u2 tiene más prioridad

            // 2. Prioridad por Saldo (mayor es mejor)
            if (u1.Saldo > u2.Saldo)
                return 1; // u1 tiene más prioridad
            if (u1.Saldo < u2.Saldo)
                return -1; // u2 tiene más prioridad

            // 3. Prioridad por Categoría (A es mejor que B)
            return u1.CategoriaId.CompareTo(u2.CategoriaId);
        }

        // Método para comparar usuarios malos (C y D)
        private int CompararClientesMalos(Usuario u1, Usuario u2)
        {
            // 1. Prioridad por Pago Pendiente (los que tienen pagos pendientes tienen más prioridad)
            if (u1.PagoPendiente && !u2.PagoPendiente)
                return 1; // u1 tiene más prioridad
            if (!u1.PagoPendiente && u2.PagoPendiente)
                return -1; // u2 tiene más prioridad

            // 2. Prioridad por Saldo (mayor es peor)
            if (u1.Saldo > u2.Saldo)
                return -1; // u2 tiene más prioridad
            if (u1.Saldo < u2.Saldo)
                return 1; // u1 tiene más prioridad

            // 3. Prioridad por Categoría (peor categoría es mejor)
            return u2.CategoriaId.CompareTo(u1.CategoriaId);
        }

        // Método para comparar usuarios incobrables (E)
        private int CompararClientesIncobrables(Usuario u1, Usuario u2)
        {
            // Prioridad por Saldo (los que deben más tienen más prioridad)
            if (u1.Saldo > u2.Saldo)
                return 1; // u1 tiene más prioridad
            if (u1.Saldo < u2.Saldo)
                return -1; // u2 tiene más prioridad

            // Si el saldo es igual, podemos usar otro criterio, por ejemplo, el ID de usuario
            return -u1.CategoriaId.CompareTo(u2.CategoriaId);
        }
    }
}