using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fase1PED.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        public int CategoriaId { get; set; } // Clave foránea que referencia a Categorias

        [ForeignKey("CategoriaId")]
        public Categorias? Categoria { get; set; } // Marca como nullable para evitar problemas de relación

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; } = 0.0M;

        [Required]
        public bool PagoPendiente { get; set; } = false;
    }
}