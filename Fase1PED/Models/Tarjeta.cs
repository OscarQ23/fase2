using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fase1PED.Models
{
    public class Tarjeta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreTarjeta { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        public decimal? LimiteCredito { get; set; } // Opcional

        public ICollection<Usuario>? Usuarios { get; set; } // Relación con Usuarios
    }
}