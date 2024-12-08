using Microsoft.AspNetCore.Mvc;
using Fase1PED.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Fase1PED.Controllers
{
    [Authorize]
    public class ClusteringController : Controller
    {
        private readonly AppDbContext _context;

        public ClusteringController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string cluster = null)
        {
            // Obtener todos los usuarios desde la base de datos
            var usuarios = _context.Usuarios.ToList();

            // Crear una lista filtrada
            var usuariosFiltrados = new List<Usuario>();

            foreach (var usuario in usuarios)
            {
                // Calcular el Score
                decimal score = ClusterLogic.CalcularScoreAvanzado(
                    usuario.Salario,
                    usuario.SegundoIngreso,
                    usuario.TiempoAFP,
                    usuario.TiempoEmpresa
                );

                // Determinar el cluster
                string usuarioCluster = ClusterLogic.CalcularCluster(score);

                // Si no se aplica filtro, o si coincide con el cluster, agregar a la lista
                if (string.IsNullOrEmpty(cluster) || usuarioCluster == cluster)
                {
                    usuariosFiltrados.Add(usuario);
                }
            }

            // Pasar el cluster seleccionado a la vista
            ViewBag.ClusterSeleccionado = cluster;

            // Pasar los usuarios filtrados a la vista
            return View(usuariosFiltrados);
        }
    }
}