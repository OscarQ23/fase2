using Microsoft.AspNetCore.Mvc;
using Fase1PED.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Fase1PED.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Vista principal
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Categoria) // Cargar relación con Categoría
                .Include(u => u.Tarjeta)   // Cargar relación con Tarjeta (opcional)
                .ToList();
            return View(usuarios);
        }

        public IActionResult AgregarUsuario()
        {
            CargarViewBags();
            return View();
        }

        // Método para procesar el formulario de agregar usuario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarUsuario(Usuario usuario)
        {
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Errores de validación:");
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Campo: {state.Key}, Errores: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                CargarViewBags();
                return View(usuario);
            }

            try
            {
                // Insertar usuario en la base de datos
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                Console.WriteLine("Usuario agregado exitosamente.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar usuario: {ex.Message}");
                Console.WriteLine($"Detalles: {ex.StackTrace}");
                ModelState.AddModelError("", "Ocurrió un error al guardar los datos.");
            }

            CargarViewBags();
            return View(usuario);
        }


        // Vista para clientes de tipo A y B
        public IActionResult ClientesBuenosOrganizados()
        {
           
                var clientesBuenos = _context.Usuarios
                    .Include(u => u.Categoria) // Cargar las categorías relacionadas
                    .Include(u => u.Tarjeta)   // Cargar las tarjetas relacionadas
                    .Where(u => u.CategoriaId == 1 || u.CategoriaId == 2)
                    .ToList();
          
            UsuarioHeap heapBuenos = new UsuarioHeap(UsuarioHeap.TipoHeap.Bueno);

            foreach (var cliente in clientesBuenos)
            {
                heapBuenos.Insertar(cliente);
            }

            var clientesOrganizados = ExtraerUsuariosDelHeap(heapBuenos);
            return View(clientesOrganizados);
        }

        // Vista para clientes de tipo C y D
        public IActionResult ClientesMalosOrganizados()
        {
            var clientesMalos = _context.Usuarios
                  .Include(u => u.Categoria) // Cargar las categorías relacionadas
                  .Include(u => u.Tarjeta)   // Cargar las tarjetas relacionadas
                  .Where(u => u.CategoriaId == 3 || u.CategoriaId == 4)
                  .ToList();

            UsuarioHeap heapMalos = new UsuarioHeap(UsuarioHeap.TipoHeap.Malo);

            foreach (var cliente in clientesMalos)
            {
                heapMalos.Insertar(cliente);
            }

            var clientesOrganizados = ExtraerUsuariosDelHeap(heapMalos);
            return View(clientesOrganizados);
        }

        // Vista para clientes de tipo E - Incobrables
        public IActionResult ClientesIncobrablesOrganizados()
        {
            var clientesIncobrables = _context.Usuarios
                
                .Where(u => u.CategoriaId == 5)
                .Include(u => u.Categoria)
                .Include(u => u.Tarjeta)
                .ToList();

            UsuarioHeap heapIncobrables = new UsuarioHeap(UsuarioHeap.TipoHeap.Incobrable);

            foreach (var cliente in clientesIncobrables)
            {
                heapIncobrables.Insertar(cliente);
            }

            var clientesOrganizados = ExtraerUsuariosDelHeap(heapIncobrables);
            return View(clientesOrganizados);
        }

        // Método para cargar las categorías y tarjetas en ViewBag
        private void CargarViewBags()
        {
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Tarjetas = _context.Tarjetas.ToList();
        }

        // Método auxiliar para extraer usuarios del heap en orden de prioridad
        private List<Usuario> ExtraerUsuariosDelHeap(UsuarioHeap heap)
        {
            var usuariosOrganizados = new List<Usuario>();
            while (heap.ObtenerMayorPrioridad() != null)
            {
                var usuario = heap.ObtenerMayorPrioridad();
                usuariosOrganizados.Add(usuario);
                heap.EliminarMayorPrioridad();
            }
            return usuariosOrganizados;
        }
    }
}