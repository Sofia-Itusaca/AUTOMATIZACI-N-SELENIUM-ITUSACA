using NUnit.Framework;
using PruebaSeleniumSIGES.Modulos;

namespace PruebaSeleniumSIGES.Tests
{
    [TestFixture]
    public class PruebasReportes
    {
        private ModuloReportes reportes;

        [SetUp]
        public void Setup()
        {
            reportes = new ModuloReportes();
            reportes.Inicializar(headless: false);
            reportes.Login();
        }
        [Test]
        public void P031_GenerarReporteGeneralDelMes()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P031 - Generar reporte general del mes");

                reportes.GenerarReporteGeneralDelMes();

                Reporte.Log(reportes.Driver, "✅ Prueba P031 completada con éxito (Generar reporte general del mes).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P031: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P032_GenerarReporteGeneralOctubre()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P032 - Generar reporte general del mes (01/10–31/10)");

                reportes.GenerarReporteGeneralOctubre();

                Reporte.Log(reportes.Driver, "✅ Prueba P032 completada con éxito (Generar reporte de octubre).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P032: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P036_LimpiarFiltrosSinFechas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P036 - Limpiar filtros sin ingresar fechas");

                reportes.LimpiarFiltrosSinFechas();

                Reporte.Log(reportes.Driver, "✅ Prueba P036 completada con éxito (Fechas vacías → Solicita ingreso o muestra error).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P036: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P037_GenerarReporteConUnaFechaVacia()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P037 - Generar reporte con una fecha vacía");

                reportes.GenerarReporteConUnaFechaVacia();

                Reporte.Log(reportes.Driver, "✅ Prueba P037 completada con éxito (Validación por fecha faltante).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P037: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P040_PuntoDeVentaConSimbolosInvalidos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P040 - Punto de venta con símbolos inválidos");

                reportes.PuntoDeVentaConSimbolosInvalidos();

                Reporte.Log(reportes.Driver, "✅ Prueba P040 completada con éxito (Validación de símbolos en Punto de Venta).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P040: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P061_FechaMinimaValida()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P061 - Validar fecha mínima (01/01/2000)");

                reportes.FechaMinimaValida();

                Reporte.Log(reportes.Driver, "✅ Prueba P061 completada con éxito (Fecha mínima aceptada y resultados válidos).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P061: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P062_FechaMaximaFutura()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P062 - Validar fecha máxima futura (31/12/2100)");

                reportes.FechaMaximaFutura();

                Reporte.Log(reportes.Driver, "⚠️ Prueba P062 completada — El sistema no valida rango futuro correctamente.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P062: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P063_RangoInvertidoDeFechas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P063 - Validar rango invertido de fechas (25/10/2025 - 20/10/2025)");

                reportes.RangoInvertidoDeFechas();

                Reporte.Log(reportes.Driver, "⚠️ Prueba P063 completada — Falta validación de error para rango invertido.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P063: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P064_FechasVaciasEnReporte()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P064 - Validar campos de fecha vacíos en reporte de pedidos");

                reportes.FechasVaciasEnReporte();

                Reporte.Log(reportes.Driver, "✅ Prueba P064 completada — Control correcto: botón 'VER' inhabilitado o mensaje mostrado.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P064: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P071_EstablecimientoCadenaLarga()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P071 - Validar campo Establecimiento con cadena larga (255 caracteres)");

                reportes.EstablecimientoCadenaLarga();

                Reporte.Log(reportes.Driver, "⚠️ Prueba P071 completada — El campo no permite filtrar por cliente al ingresar cadena larga.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P071: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P073_RangoDeFechasFueraDeLimite()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P073 - Validar rango de fechas fuera de límite (24/10/2025 - 01/01/1800)");

                reportes.RangoDeFechasFueraDeLimite();

                Reporte.Log(reportes.Driver, "✅ Prueba P073 completada — Control correcto: el sistema maneja rangos de fechas inválidos adecuadamente.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P073: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }



        [Test]
        public void P130_VerReportePedidosEnPantalla()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P130 - Reportes de Pedidos: Visualización directa del reporte en pantalla");

                reportes.VerReportePedidosEnPantalla();

                Reporte.Log(reportes.Driver, "✅ Prueba P130 completada — El reporte se visualizó correctamente en pantalla.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P130: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P131_LimpiarCampoEstablecimiento()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P131 - Limpiar campo Establecimiento en Reportes de Pedidos");

                reportes.LimpiarCampoEstablecimiento();

                Reporte.Log(reportes.Driver, "✅ Prueba P131 completada — El campo 'Establecimiento' fue limpiado correctamente después de ingresar texto.");
                
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P131: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P135_VerReporteTresEstablecimientos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P135 - Ver reporte con múltiples establecimientos (CA PRINCIPAL, CA ALMACEN CASTILLO, PARA NIÑOS)");

                reportes.VerReporteTresEstablecimientos();

                Reporte.Log(reportes.Driver, "✅ Prueba P135 completada — Se generó correctamente el reporte con los tres establecimientos seleccionados.");
               
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P135: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P136_RestablecerCheckTodas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P136 - Restablecer check 'Todas' al cambiar establecimiento");

                reportes.RestablecerCheckTodas();

                Reporte.Log(reportes.Driver, "✅ Prueba P136 completada — Al seleccionar 'CA PRINCIPAL', se restableció correctamente el check 'Todas'.");
                
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P136: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P137_VerReporteAlmacenCastillo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P137 - Ver reporte con establecimiento 'CA ALMACEN CASTILLO'");

                reportes.VerReporteAlmacenCastillo();

                Reporte.Log(reportes.Driver, "✅ Prueba P137 completada — Se generó correctamente el reporte del establecimiento 'CA ALMACEN CASTILLO'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P137: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P138_VerReporteParaNinos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P138 - Ver reporte con establecimiento 'PARA NIÑOS'");

                reportes.VerReporteParaNinos();

                Reporte.Log(reportes.Driver, "✅ Prueba P138 completada — Se generó correctamente el reporte del establecimiento 'PARA NIÑOS'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P138: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P139_EscribirYBorrarCAPrincipal()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P139 - Escribir y borrar 'CA PRINCIPAL' en Establecimiento");

                reportes.EscribirYBorrarCAPrincipal();

                Reporte.Log(reportes.Driver, "✅ Prueba P139 completada — Se escribió y eliminó 'CA PRINCIPAL' correctamente sin mostrar reporte.");
                
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(reportes.Driver, $"❌ Error en P139: {ex.Message}");
                throw;
            }
            finally
            {
                reportes.EsperarCargaYFinalizar();
            }
        }


        [TearDown]
        public void TearDown()
        {
            reportes.Finalizar();
        }

    }
}
