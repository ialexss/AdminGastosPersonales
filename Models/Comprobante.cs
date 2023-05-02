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
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }
        [Required]
        public decimal Costo { get; set; }
        public string Tipo { get; set;}
        public bool Activo { get; set; }
        public byte[]? Imagen { get; set;}
        public int CategoriaId { get; set; }
        public string UserId { get; set; }
        public Categoria? Categoria { get; set; }
        public IdentityUser? User { get; set; }

    }
}
