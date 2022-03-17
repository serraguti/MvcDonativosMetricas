using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcDonativosMetricas.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDonativosMetricas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TelemetryClient telemetryClient;

        public HomeController(ILogger<HomeController> logger
            , TelemetryClient telemetryClient)
        {
            _logger = logger;
            this.telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            this._logger.LogInformation(DateTime.Now.ToShortTimeString() 
                + " Entrando a GET/Index");
            return View();
        }

        [HttpPost]
        public IActionResult Index(string nombre, int donativo)
        {
            this._logger.LogInformation(DateTime.Now.ToShortTimeString()
                + " Entrando a POST/Index");
            ViewData["MENSAJE"] = "Su donativo de " + donativo
                + " ha sido aceptado.  Muchas gracias Sr/Sra " + nombre;
            this.telemetryClient.TrackEvent("DonativosRequest");
            //PODEMOS CREAR ALGUN ELEMENTO PERSONALIZADO EN LA TELEMETRIA
            //CON EVENTOS, POR EJEMPLO, NOSOTROS VAMOS A CREAR UNA METRICA
            //PARA VISUALIZAR EN EL PORTAL DE AZURE LA SUMA DE DONATIVOS
            MetricTelemetry metricDonativos = new MetricTelemetry();
            metricDonativos.Name = "Donativos";
            metricDonativos.Sum = donativo;
            this.telemetryClient.TrackMetric(metricDonativos);
            //TAMBIEN TENEMOS LA POSIBILIDAD ALMACENAR TRAZAS PARA 
            //REALIZAR SEGUIMIENTOS DE ACTIVIDAD
            string mensaje = nombre + " " + donativo + "€";
            //POR DEFECTO, LAS TRAZAS SON DE INFORMACION, PERO 
            //PUEDO CAMBIAR TAMBIEN SU NIVEL DE SEVERIDAD
            SeverityLevel level;
            if (donativo < 5)
            {
                level = SeverityLevel.Warning;
            }
            else if (donativo < 2)
            {
                level = SeverityLevel.Critical;
            }
            else if (donativo == 0)
            {
                level = SeverityLevel.Error;
            }
            else
            {
                level = SeverityLevel.Information;
            }
            TraceTelemetry trace = new TraceTelemetry(mensaje, level);
            this.telemetryClient.TrackTrace(trace);
            return View();
        }

        public IActionResult Privacy()
        {
            this._logger.LogInformation(DateTime.Now.ToShortTimeString()
                + " Entrando a GET/Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
