using System.ComponentModel.DataAnnotations;

namespace GastosPersonales.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required]
        [StringLength(35)]
        public string? Nombre { get; set; }
        [Required]
        public string? TipoCategoria { get; set;}
    }
}
