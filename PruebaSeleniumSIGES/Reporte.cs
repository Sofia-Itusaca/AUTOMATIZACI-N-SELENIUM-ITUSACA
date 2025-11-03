using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using System;

namespace PruebaSeleniumSIGES
{
    public static class Reporte
    {
        private static ExtentReports extent;
        private static ExtentTest test;

        public static void IniciarReporte()
        {
            // Usamos SparkReporter (nuevo en ExtentReports 5.x)
            var spark = new ExtentSparkReporter("Reporte_PruebasSIGES.html");

            extent = new ExtentReports();
            extent.AttachReporter(spark);
        }

        public static void CrearTest(string nombre)
        {
            test = extent.CreateTest(nombre);
        }

        public static void Log(IWebDriver driver, string mensaje)
        {
            try
            {
                // Guarda la captura en una carpeta local
                string folderPath = "Screenshots";
                System.IO.Directory.CreateDirectory(folderPath);

                string fileName = $"{folderPath}/captura_{DateTime.Now:HHmmss}.png";
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(fileName);

                // Agrega la imagen al reporte
                test.Info(mensaje).AddScreenCaptureFromPath(fileName);

                Console.WriteLine("[CAPTURA] " + mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR CAPTURA] " + ex.Message);
            }
        }


        public static void FinalizarReporte()
        {
            extent.Flush();

            // 🔹 Abre automáticamente el reporte al finalizar
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "Reporte_PruebasSIGES.html",
                UseShellExecute = true
            });
        }
    }
}
