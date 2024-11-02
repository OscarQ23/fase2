using Microsoft.AspNetCore.Mvc;
using Fase1PED.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fase1PED.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        //Contexto de la base para realizar operaciones
        public UsuarioController(AppDbContext context)
        {
            _context = context;
            CargarUsuariosDesdeBaseDeDatos();
        }

        // Cargar los usuarios desde la base de datos y llenar el heap
        private void CargarUsuariosDesdeBaseDeDatos()
        { 
            var usuariosDB = _context.Usuarios.Include(u => u.Categoria).ToList();
        }

        //Ejecutar la vista
        public IActionResult Index()
        {
            return View();
        }

        //agregar usuario
        public IActionResult AgregarUsuario()
        {
            ViewBag.Categorias = _context.Categorias.ToList();
            return View();
        }


        //Meth creado para poder hacer el envio a la base de datos
        [HttpPost]
        public IActionResult AgregarUsuario(Usuario usuario)
        {
            // Verifica si el ModelState es válido
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error de ModelState: " + error.ErrorMessage);
                }

                // Recarga las categorías en caso de error
                ViewBag.Categorias = _context.Categorias.ToList();
                return View(usuario);
            }

            try
            {
                // Hacemos el respectivo insert a la base de datos
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción durante la inserción: " + ex.Message);
                ModelState.AddModelError(string.Empty, "Ocurrió un error al agregar el usuario: " + ex.Message);
            }

            //Recargamos la vista par poder ver los respectivos cambios
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(usuario);
        }

        // METH para los clientes de tipo A Y B
        public IActionResult ClientesBuenosOrganizados()
        {
            //Obtenemos la categoria de los clientes con ID 1 y 2 , los cuales como descripcion son A Y B
            var clientesBuenos = _context.Usuarios
                .Where(u => u.CategoriaId == 1 || u.CategoriaId == 2) 
                .ToList();

            // Crear el heap para clientes buenos
            UsuarioHeap heapBuenos = new UsuarioHeap(UsuarioHeap.TipoHeap.Bueno);

            //Colocamos los clientes en el heap
            foreach (var cliente in clientesBuenos)
            {
                heapBuenos.Insertar(cliente);
            }

            //Listado para organizar a los clientes con la logica que creamos en el heap 
            List<Usuario> clientesOrganizados = new List<Usuario>();
            while (heapBuenos.ObtenerMayorPrioridad() != null)
            {
                var clienteConPrioridad = heapBuenos.ObtenerMayorPrioridad();
                clientesOrganizados.Add(clienteConPrioridad);
                heapBuenos.EliminarMayorPrioridad();
            }

            return View(clientesOrganizados);
        }

        // METH para los clientes de tipo C Y D
        public IActionResult ClientesMalosOrganizados()
        { 
            var clientesMalos = _context.Usuarios
                .Where(u => u.CategoriaId == 3 || u.CategoriaId == 4 ) 
                .ToList();

            // Crear el heap para clientes malos
            UsuarioHeap heapMalos = new UsuarioHeap(UsuarioHeap.TipoHeap.Malo);

            //insertamos los clientes malos al heap
            foreach (var cliente in clientesMalos)
            {
                heapMalos.Insertar(cliente);
            }

            // Extraemos y hacemos el listado segun la prioridad del heap
            List<Usuario> clientesOrganizados = new List<Usuario>();
            while (heapMalos.ObtenerMayorPrioridad() != null)
            {
                var clienteConPrioridad = heapMalos.ObtenerMayorPrioridad();
                clientesOrganizados.Add(clienteConPrioridad);
                heapMalos.EliminarMayorPrioridad();
            }
            return View(clientesOrganizados);
        }

        /// Meth para los clientes de tipo E - Incobrables
        public IActionResult ClientesIncobrablesOrganizados()
        {
            // Obtener usuarios con categoría E (incobrables)
            var clientesIncobrables = _context.Usuarios
                .Where(u => u.CategoriaId == 5)
                .Include(u => u.Categoria) // Incluimos la categoría si la usas en la vista
                .ToList();

            // Crear el heap para clientes incobrables
            UsuarioHeap heapIncobrables = new UsuarioHeap(UsuarioHeap.TipoHeap.Incobrable);

            // Insertar los clientes incobrables en el heap
            foreach (var cliente in clientesIncobrables)
            {
                heapIncobrables.Insertar(cliente);
            }

            // Extraer los clientes del heap en orden de prioridad
            List<Usuario> clientesOrganizados = new List<Usuario>();
            while (heapIncobrables.ObtenerMayorPrioridad() != null)
            {
                var clienteConPrioridad = heapIncobrables.ObtenerMayorPrioridad();
                clientesOrganizados.Add(clienteConPrioridad);
                heapIncobrables.EliminarMayorPrioridad();
            }

            // Pasar la lista organizada a la vista
            return View(clientesOrganizados);
        }
    }
}