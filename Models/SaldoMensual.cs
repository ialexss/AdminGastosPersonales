using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GastosPersonales.Models
{
    public class SaldoMensual
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Total no puede estar vacio")]
        [StringLength(30, ErrorMessage = "La longitud maxima es 30 caracteres")]
        public string Total { get; set; }
        [Required(ErrorMessage = "Ingrese un Mes")]
        public int Mes { get; set; }
        [Required(ErrorMessage = "Ingrese un Año")]
        public int Año { get; set; }
        [Required]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }
    }
}
