using System.ComponentModel.DataAnnotations;

namespace Fase1PED.Models
{
    public class Categorias
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty; // Evita valores nulos

        [Required]
        public string Descripcion { get; set; } = string.Empty; // Evita valores nulos
    }
}