using GastosPersonales.Data;
using GastosPersonales.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GastosPersonales.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int Dias { get; set; } = 30;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Saldo Actual
            decimal sumaIngresos = _context.Comprobante.Where(c => c.Tipo == "Ingreso").Sum(c => c.Costo);
            decimal sumaGastos = _context.Comprobante.Where(c => c.Tipo == "Egreso").Sum(c => c.Costo);
            decimal resultado = sumaIngresos - sumaGastos;

            ViewBag.Resultado = resultado;

            return View();
        }

        public IActionResult InformeIngresosFecha()
        {
            DateTime FechaInicio = DateTime.Now;
            FechaInicio = FechaInicio.AddDays(- Dias);

            List<VMComprobante> Lista = (from comprobante in _context.Comprobante
                                                               where (comprobante.Fecha.Date >= FechaInicio.Date && comprobante.Tipo == "Ingreso")
                                                               group comprobante by comprobante.Fecha.Date into grupo
                                                               select new VMComprobante
                                                               {
                                                                   fecha = grupo.Key.ToString("dd/MM/yyyy"),
                                                                   saldo = grupo.Sum(c => c.Costo),

                                                               }).ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult InformeGastosFecha()
        {
            DateTime FechaInicio = DateTime.Now;
            FechaInicio = FechaInicio.AddDays(- Dias);

            List<VMComprobante> Lista = (from comprobante in _context.Comprobante
                                         where (comprobante.Fecha.Date >= FechaInicio.Date && comprobante.Tipo == "Egreso")
                                         group comprobante by comprobante.Fecha.Date into grupo
                                         select new VMComprobante
                                         {
                                             fecha = grupo.Key.ToString("dd/MM/yyyy"),
                                             saldo = grupo.Sum(c => c.Costo),

                                         }).ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult InformeGategoria()
        {
            DateTime FechaInicio = DateTime.Now;
            FechaInicio = FechaInicio.AddDays(- Dias);

            List<VMComprobante> Lista = (from comprobante in _context.Comprobante
                                         where (comprobante.Fecha.Date >= FechaInicio.Date)
                                         group comprobante by comprobante.Categoria.Nombre into grupo
                                         select new VMComprobante
                                         {
                                             categoria = grupo.Key.ToString(),
                                             saldo = grupo.Sum(c => c.Costo),
                                             tipo = grupo.FirstOrDefault().Categoria.TipoCategoria, //Guardamos el tipo de la categoria
                                         }).ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult InformeSaldoMensual()
        {
            decimal sumaIngresos = _context.Comprobante.Where(c => c.Tipo == "Ingreso").Sum(c => c.Costo);
            decimal sumaGastos = _context.Comprobante.Where(c => c.Tipo == "Egreso").Sum(c => c.Costo);
            decimal resultado = sumaIngresos - sumaGastos;

            ViewBag.Resultado = resultado;

            return View();
        }
    }
}
