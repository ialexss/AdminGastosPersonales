using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GastosPersonales.Models
{
    public class SaldoMensual
    {
        public int Id { get; set; }
        public string Total { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }
    }
}
