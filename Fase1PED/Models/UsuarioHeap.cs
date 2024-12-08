using System.Collections.Generic;

namespace Fase1PED.Models
{
    public class UsuarioHeap
    {
        private List<Usuario> heap = new List<Usuario>();

        public enum TipoHeap { Bueno, Malo, Incobrable }
        private TipoHeap tipoHeap;

        public UsuarioHeap(TipoHeap tipo)
        {
            tipoHeap = tipo;
        }

        // Insertar un usuario al heap
        public void Insertar(Usuario usuario)
        {
            heap.Add(usuario);
            Subir(heap.Count - 1); // Ajusta la posición del nuevo elemento
        }

        // Obtener el usuario con mayor prioridad (manejar nulos explícitamente)
        public Usuario? ObtenerMayorPrioridad()
        {
            if (heap.Count == 0) return null;
            return heap[0];
        }

        // Eliminar el usuario con mayor prioridad
        public void EliminarMayorPrioridad()
        {
            if (heap.Count == 0) return;
            Intercambiar(0, heap.Count - 1);
            heap.RemoveAt(heap.Count - 1);
            Bajar(0);
        }

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

        private void Intercambiar(int index1, int index2)
        {
            Usuario temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }

        // Método principal para comparar usuarios basado en el tipo de heap
        private int Comparar(Usuario u1, Usuario u2)
        {
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

        // Comparación para clientes buenos (A y B)
        private int CompararClientesBuenos(Usuario u1, Usuario u2)
        {
            
            // 2. Prioridad por Saldo
            if (u1.Saldo > u2.Saldo)
                return 1;
            if (u1.Saldo < u2.Saldo)
                return -1;

            // 1. Prioridad por Pago Pendiente
            if (!u1.PagoPendiente && u2.PagoPendiente)
                return 1;
            if (u1.PagoPendiente && !u2.PagoPendiente)
                return -1;


            // 3. Prioridad por Categoría (manejar valores nulos)
            return CompararNullable(u1.CategoriaId, u2.CategoriaId);
        }

        // Comparación para clientes malos (C y D)
        private int CompararClientesMalos(Usuario u1, Usuario u2)
        {
            // 1. Prioridad por Saldo
            if (u1.Saldo > u2.Saldo)
                return 1;
            if (u1.Saldo < u2.Saldo)
                return -1;


            // 2. Prioridad por Pago Pendiente
            if (u1.PagoPendiente && !u2.PagoPendiente)
                return 1;
            if (!u1.PagoPendiente && u2.PagoPendiente)
                return -1;

            // 3. Prioridad por Categoría (manejar valores nulos)
            return CompararNullable(u1.CategoriaId, u2.CategoriaId);
        }

        // Comparación para clientes incobrables (E)
        private int CompararClientesIncobrables(Usuario u1, Usuario u2)
        {
            // Prioridad por Saldo
            if (u1.Saldo > u2.Saldo)
                return 1;
            if (u1.Saldo < u2.Saldo)
                return -1;

            // Si el saldo es igual, usar el ID como criterio adicional
            return u1.Id.CompareTo(u2.Id);
        }

        // Método auxiliar para comparar valores int? (nulos permitidos)
        private int CompararNullable(int? a, int? b)
        {
            if (a.HasValue && b.HasValue)
                return a.Value.CompareTo(b.Value); // Comparar valores si ambos no son nulos
            if (!a.HasValue && b.HasValue)
                return -1; // Si `a` es nulo, `b` tiene más prioridad
            if (a.HasValue && !b.HasValue)
                return 1; // Si `b` es nulo, `a` tiene más prioridad
            return 0; // Ambos son nulos, son iguales
        }
    }
}