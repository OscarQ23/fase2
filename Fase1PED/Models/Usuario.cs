using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fase1PED.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Telefono { get; set; } = string.Empty;

        [Required] // El ID de la categoría siempre será obligatorio
        public int CategoriaId { get; set; }

        [Required] // El ID de la tarjeta siempre será obligatorio
        public int TarjetaId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        [Required]
        public bool PagoPendiente { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        public decimal? SegundoIngreso { get; set; }

        [Required]
        public int TiempoAFP { get; set; }

        [Required]
        public int TiempoEmpresa { get; set; }

        // Estas propiedades de navegación no deben ser obligatorias para validar el modelo
        public Categorias? Categoria { get; set; }
        public Tarjeta? Tarjeta { get; set; }

        [NotMapped] // Campo calculado
        public decimal Score
        {
            get
            {
                return (Salario + (SegundoIngreso ?? 0) + (TiempoAFP * 0.1m) + (TiempoEmpresa * 0.1m)) / 100000m;
            }
        }
    }
}