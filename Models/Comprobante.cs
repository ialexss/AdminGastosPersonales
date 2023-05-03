using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GastosPersonales.Models
{
    public class Comprobante
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un detalle para el comprobante")]
        [StringLength(250, ErrorMessage = "La longitud maxima es 250 caracteres")]
        public string Detalle { get; set; }
        [Required(ErrorMessage = "Seleccione una fecha")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Ingrese un costo")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Costo { get; set; }
        [Required(ErrorMessage = "Seleccione que tipo de comprobante es")]
        [StringLength(50, ErrorMessage = "La longitud maxima es 50 caracteres")]
        public string Tipo { get; set;}
        [Required]
        public bool Activo { get; set; }
        public byte[]? Imagen { get; set;}
        [Required]
        public int CategoriaId { get; set; }
        [Required]
        public string UserId { get; set; }
        public Categoria? Categoria { get; set; }
        public IdentityUser? User { get; set; }

    }
}
