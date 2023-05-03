using System.ComponentModel.DataAnnotations;

namespace GastosPersonales.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre para la categoria")]
        [StringLength(35, ErrorMessage = "La longitud maxima es 35 caracteres")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string? TipoCategoria { get; set;}
    }
}
