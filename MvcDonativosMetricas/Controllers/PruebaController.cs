using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDonativosMetricas.Controllers
{
    public class PruebaController : Controller
    {
        private readonly ILogger _logger;
        

        public PruebaController(ILogger logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            this._logger.LogInformation(DateTime.Now.ToShortTimeString()
    + " Entrando a Prueba/Index");
            return View();
        }

        public IActionResult Otro()
        {
            this._logger.LogInformation(DateTime.Now.ToShortTimeString()
    + " Entrando a Prueba/Otro");
            return View();
        }
    }
}
