using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fase1PED.Models
{
    public class Categorias
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Descripcion { get; set; } // Campo opcional

        public ICollection<Usuario>? Usuarios { get; set; } // Relación con Usuarios
    }
}