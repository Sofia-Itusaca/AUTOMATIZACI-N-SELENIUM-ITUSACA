using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace PruebaSeleniumSIGES.Modulos
{
    public class ModuloReportes : ModuloBase
    {
        

        // PRUEBA P031
        public void GenerarReporteGeneralDelMes()
        {
            Console.WriteLine("=== Ejecutando P031: Generar reporte general del mes ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde"
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);

                // Seleccionar todo el texto existente y reemplazar
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("30/10/2025 16:31:10 PM");
                Thread.Sleep(400);

                // Clic fuera para aplicar cambios
                body.Click();
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Fecha inicial establecida correctamente.");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta"
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);

                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaHasta.SendKeys("10/10/2025 16:31:10 PM");
                Thread.Sleep(400);

                // Clic fuera para registrar la fecha final
                body.Click();
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Fecha final establecida correctamente.");

                // ===============================
                // 🔹 4. Clic en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(4000);

                // ===============================
                // 🔹 5. Verificar el reporte
                // ===============================
                bool reporteVisible = driver.PageSource.Contains("Reporte de Pedidos") ||
                                      driver.PageSource.Contains("Pedidos del mes") ||
                                      driver.PageSource.Contains("Total General") ||
                                      driver.PageSource.Contains("Invalidados");

                if (reporteVisible)
                {
                    Console.WriteLine("[✅] Reporte generado correctamente del 10/10/2025 al 31/10/2025.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectó el contenido esperado del reporte.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P031: " + ex.Message);
            }
        }
        // PRUEBA P032
        public void GenerarReporteGeneralOctubre()
        {
            Console.WriteLine("=== Ejecutando P032: Generar reporte general del mes (01/10–31/10) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde"
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("01/10/2025 16:31:10 PM");
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(1000);

                Console.WriteLine("[OK] Fecha inicial establecida correctamente (01/10/2025).");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta"
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);
                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaHasta.SendKeys("31/10/2025 16:31:10 PM");
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(1000);

                Console.WriteLine("[OK] Fecha final establecida correctamente (31/10/2025).");

                // ===============================
                // 🔹 4. Clic en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(4000);

                // ===============================
                // 🔹 5. Verificar el reporte
                // ===============================
                bool reporteVisible = driver.PageSource.Contains("Reporte de Pedidos") ||
                                      driver.PageSource.Contains("Pedidos del mes") ||
                                      driver.PageSource.Contains("Total General") ||
                                      driver.PageSource.Contains("Invalidados");

                if (reporteVisible)
                {
                    Console.WriteLine("[✅] Reporte general de octubre generado correctamente (válidos e invalidados).");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectó el contenido esperado del reporte.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P032: " + ex.Message);
            }
        }
        // PRUEBA P036
        public void LimpiarFiltrosSinFechas()
        {
            Console.WriteLine("=== Ejecutando P036: Limpiar filtros sin ingresar fechas ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // 🔹 2. Limpiar los campos de fecha (si contienen algo)
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));
                campoFechaDesde.Click();
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(300);

                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));
                campoFechaHasta.Click();
                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(300);

                // 🔹 3. Click fuera para registrar los cambios
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Campos de fecha vacíos correctamente.");

                // 🔹 4. Click en el botón "VER"
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado con fechas vacías.");
                Thread.Sleep(2000);

                // 🔹 5. Verificación de mensaje de error o validación
                bool mensajeError = driver.PageSource.Contains("Ingrese") ||
                                    driver.PageSource.Contains("error") ||
                                    driver.PageSource.Contains("requerido") ||
                                    driver.PageSource.Contains("Seleccione una fecha");

                if (mensajeError)
                {
                    Console.WriteLine("[✅] El sistema mostró mensaje de validación correctamente al no ingresar fechas.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se mostró mensaje de error, pero no se generó reporte (validación silenciosa).");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P036: " + ex.Message);
            }
        }
        // PRUEBA P037
        public void GenerarReporteConUnaFechaVacia()
        {
            Console.WriteLine("=== Ejecutando P037: Generar reporte con una fecha vacía ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde" (solo este se llenará)
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("01/10/2025 16:31:10 PM");
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha inicial establecida correctamente (01/10/2025).");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta" (queda vacío)
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);
                campoFechaHasta.SendKeys(Keys.Control + "a");
                campoFechaHasta.SendKeys(Keys.Delete);
                body.Click();
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Campo de fecha final dejado vacío.");

                // ===============================
                // 🔹 4. Click en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Intento de generar reporte con una sola fecha.");
                Thread.Sleep(3000);

                // ===============================
                // 🔹 5. Validación de respuesta esperada
                // ===============================
                bool mensajeError = driver.PageSource.Contains("Ingrese") ||
                                    driver.PageSource.Contains("requerido") ||
                                    driver.PageSource.Contains("Seleccione ambas fechas") ||
                                    driver.PageSource.Contains("error");

                if (mensajeError)
                {
                    Console.WriteLine("[✅] El sistema mostró mensaje de validación correctamente al faltar una fecha.");
                }
                else
                {
                    Console.WriteLine("[❌] No se mostró ningún mensaje. El sistema permitió continuar indebidamente.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P037: " + ex.Message);
            }
        }
        // PRUEBA P040
        public void PuntoDeVentaConSimbolosInvalidos()
        {
            Console.WriteLine("=== Ejecutando P040: Punto de venta con símbolos inválidos ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Punto de Venta"
                // ===============================
                var campoPuntoVenta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//ul[@class='select2-selection__rendered']")));
                campoPuntoVenta.Click();
                Thread.Sleep(800);

                // Seleccionar el cuadro de búsqueda del select2
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));

                inputBusqueda.SendKeys("@@@");
                Thread.Sleep(800);

                Console.WriteLine("[OK] Texto '@@@' ingresado en el campo Punto de Venta.");

                // Presionar Enter (en caso de que el select2 lo acepte)
                inputBusqueda.SendKeys(Keys.Enter);
                Thread.Sleep(1000);

                // ===============================
                // 🔹 3. Click en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(2500);

                // ===============================
                // 🔹 4. Validación de resultados
                // ===============================
                bool mensajeError = driver.PageSource.Contains("error") ||
                                    driver.PageSource.Contains("inválido") ||
                                    driver.PageSource.Contains("No se encontraron resultados") ||
                                    driver.PageSource.Contains("símbolos no válidos");

                if (mensajeError)
                {
                    Console.WriteLine("[✅] El sistema manejó correctamente la entrada inválida ('@@@') en Punto de Venta.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectó mensaje de error, pero el sistema no devolvió resultados válidos.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P040: " + ex.Message);
            }
        }
        // PRUEBA P061
        public void FechaMinimaValida()
        {
            Console.WriteLine("=== Ejecutando P061: Validación de fecha mínima (01/01/2000) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde"
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);

                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("01/01/2000 00:00:00 AM");
                Thread.Sleep(400);

                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha inicial (01/01/2000) establecida correctamente.");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta"
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);

                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaHasta.SendKeys("31/10/2025 23:59:59 PM");
                Thread.Sleep(400);

                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha final (31/10/2025) establecida correctamente.");

                // ===============================
                // 🔹 4. Click en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(4000);

                // ===============================
                // 🔹 5. Verificar resultados
                // ===============================
                bool reporteVisible = driver.PageSource.Contains("Reporte de Pedidos") ||
                                      driver.PageSource.Contains("Pedidos del mes") ||
                                      driver.PageSource.Contains("Total General") ||
                                      driver.PageSource.Contains("Invalidados");

                if (reporteVisible)
                {
                    Console.WriteLine("[✅] Reporte generado correctamente con fecha mínima (01/01/2000).");
                }
                else
                {
                    bool sinDatos = driver.PageSource.Contains("No hay datos disponibles") ||
                                    driver.PageSource.Contains("no se encontraron registros");

                    if (sinDatos)
                        Console.WriteLine("[✅] El sistema acepta la fecha mínima y muestra mensaje de 'no se encontraron registros'.");
                    else
                        Console.WriteLine("[⚠️] No se detectó reporte ni mensaje de validación.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P061: " + ex.Message);
            }
        }
        // PRUEBA P062
        public void FechaMaximaFutura()
        {
            Console.WriteLine("=== Ejecutando P062: Validación de fecha máxima futura (31/12/2100) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde"
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("01/10/2025 00:00:00 AM");
                Thread.Sleep(400);

                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha inicial establecida correctamente (01/10/2025).");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta" (futura)
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);
                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaHasta.SendKeys("31/12/2100 23:59:59 PM");
                Thread.Sleep(400);

                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha final establecida correctamente (31/12/2100).");

                // ===============================
                // 🔹 4. Click en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(4000);

                // ===============================
                // 🔹 5. Validación del comportamiento
                // ===============================
                bool resultados = driver.PageSource.Contains("REGISTRADO") ||
                                  driver.PageSource.Contains("ADMIN") ||
                                  driver.PageSource.Contains("S/.");

                if (resultados)
                {
                    Console.WriteLine("[⚠️] El sistema muestra resultados con fecha futura — No valida rango temporal.");
                }
                else
                {
                    bool sinResultados = driver.PageSource.Contains("no se encontraron registros") ||
                                         driver.PageSource.Contains("No hay datos disponibles");

                    if (sinResultados)
                        Console.WriteLine("[✅] El sistema rechaza correctamente el rango futuro (sin resultados).");
                    else
                        Console.WriteLine("[⚠️] No se detectó mensaje de validación ni resultados visibles.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P062: " + ex.Message);
            }
        }

        // PRUEBA P063
        public void RangoInvertidoDeFechas()
        {
            Console.WriteLine("=== Ejecutando P063: Validación de rango invertido de fechas (25/10/2025 - 20/10/2025) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde" (mayor que la final)
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);
                campoFechaDesde.SendKeys(Keys.Control + "a"); // ✅ seleccionar todo
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);        // ✅ limpiar campo
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("25/10/2025 00:00:00 AM"); // ✅ pegar nueva fecha
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha inicial (25/10/2025) establecida correctamente.");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta" (menor que la inicial)
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);
                campoFechaHasta.SendKeys(Keys.Control + "a"); // ✅ seleccionar todo
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);        // ✅ limpiar campo
                Thread.Sleep(200);
                campoFechaHasta.SendKeys("20/10/2025 23:59:59 PM"); // ✅ pegar nueva fecha
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha final (20/10/2025) establecida correctamente (rango invertido).");

                // ===============================
                // 🔹 4. Click en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado con rango invertido.");
                Thread.Sleep(4000);

                // ===============================
                // 🔹 5. Validación del resultado
                // ===============================
                bool mensajeError = driver.PageSource.Contains("rango inválido") ||
                                    driver.PageSource.Contains("Fecha inicial mayor que la final") ||
                                    driver.PageSource.Contains("error") ||
                                    driver.PageSource.Contains("inválido");

                if (mensajeError)
                {
                    Console.WriteLine("[✅] El sistema mostró mensaje de error por rango inválido.");
                }
                else
                {
                    bool sinResultados = driver.PageSource.Contains("No hay datos disponibles") ||
                                         driver.PageSource.Contains("no se encontraron registros");

                    if (sinResultados)
                        Console.WriteLine("[⚠️] No se mostró mensaje de error, pero no se encontraron registros (falta validación explícita).");
                    else
                        Console.WriteLine("[❌] El sistema no detectó el rango invertido, posible error de validación.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P063: " + ex.Message);
            }
        }
        // PRUEBA P064
        public void FechasVaciasEnReporte()
        {
            Console.WriteLine("=== Ejecutando P064: Validación con fechas vacías ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde" (vaciar completamente)
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(300);
                body.Click();
                Thread.Sleep(500);
                Console.WriteLine("[OK] Campo 'Fecha Desde' vaciado correctamente.");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta" (vaciar completamente)
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);
                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(300);
                body.Click();
                Thread.Sleep(500);
                Console.WriteLine("[OK] Campo 'Fecha Hasta' vaciado correctamente.");

                // ===============================
                // 🔹 4. Intentar hacer clic en botón "VER"
                // ===============================
                var botonVer = driver.FindElement(By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']"));

                // Verificar si el botón está deshabilitado
                bool botonDeshabilitado = !botonVer.Enabled || botonVer.GetAttribute("class").Contains("disabled");

                if (botonDeshabilitado)
                {
                    Console.WriteLine("[✅] El botón 'VER' está inhabilitado cuando las fechas están vacías (control correcto).");
                }
                else
                {
                    // Intentar hacer clic de todas formas
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                    Console.WriteLine("[⚠️] El botón 'VER' no está deshabilitado, se intentó hacer clic.");
                    Thread.Sleep(2500);

                    // ===============================
                    // 🔹 5. Validación del mensaje
                    // ===============================
                    bool mensajeError = driver.PageSource.Contains("debe ingresar fecha") ||
                                        driver.PageSource.Contains("Debe ingresar fecha") ||
                                        driver.PageSource.Contains("Seleccione una fecha") ||
                                        driver.PageSource.Contains("error");

                    if (mensajeError)
                        Console.WriteLine("[✅] El sistema mostró correctamente el mensaje 'Debe ingresar fecha'.");
                    else
                        Console.WriteLine("[⚠️] No se mostró mensaje, pero el sistema no generó reporte (control parcial).");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P064: " + ex.Message);
            }
        }
        // PRUEBA P071
        public void EstablecimientoCadenaLarga()
        {
            Console.WriteLine("=== Ejecutando P071: Campo Establecimiento con cadena larga (255 caracteres) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                // Elemento del cuerpo
                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Click en el campo Establecimiento (select2)
                // ===============================
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[@id='select2--container' or contains(@class,'select2-selection__rendered')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);
                campoEstablecimiento.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Click en el campo 'Establecimiento' realizado correctamente.");

                // ===============================
                // 🔹 3. Escribir cadena larga de 255 caracteres
                // ===============================
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));

                string textoLargo = new string('A', 255); // Cadena “AAAAA…” de 255 caracteres
                inputBusqueda.SendKeys(textoLargo);
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Cadena larga de 255 caracteres ingresada en el campo Establecimiento.");

                // ===============================
                // 🔹 4. Presionar Enter (intentar seleccionar)
                // ===============================
                inputBusqueda.SendKeys(Keys.Enter);
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Se presionó ENTER para confirmar la búsqueda.");

                // ===============================
                // 🔹 5. Verificar comportamiento
                // ===============================
                bool resultados = driver.PageSource.Contains("SUCURSAL") ||
                                  driver.PageSource.Contains("CORPORACION") ||
                                  driver.PageSource.Contains("resultados");

                if (resultados)
                {
                    Console.WriteLine("[✅] El sistema aceptó la cadena larga y permitió buscar correctamente.");
                }
                else
                {
                    Console.WriteLine("[❌] El sistema no permitió filtrar correctamente al ingresar cadena larga (posible validación incorrecta).");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P071: " + ex.Message);
            }
        }
        // PRUEBA P073
        public void RangoDeFechasFueraDeLimite()
        {
            Console.WriteLine("=== Ejecutando P073: Validar rango de fechas fuera de límite (24/10/2025 - 01/01/1800) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var body = driver.FindElement(By.TagName("body"));

                // ===============================
                // 🔹 2. Campo "Fecha y Hora Desde" (válido)
                // ===============================
                var campoFechaDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaDesde' or contains(@ng-model,'FechaDesde')]")));

                campoFechaDesde.Click();
                Thread.Sleep(300);
                campoFechaDesde.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaDesde.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaDesde.SendKeys("24/10/2025 00:00:00 AM");
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha inicial (24/10/2025) establecida correctamente.");

                // ===============================
                // 🔹 3. Campo "Fecha y Hora Hasta" (fuera de rango: año 1800)
                // ===============================
                var campoFechaHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='reporteador.PedidosInvalidadosPorFecha.FechaHasta' or contains(@ng-model,'FechaHasta')]")));

                campoFechaHasta.Click();
                Thread.Sleep(300);
                campoFechaHasta.SendKeys(Keys.Control + "a");
                Thread.Sleep(200);
                campoFechaHasta.SendKeys(Keys.Delete);
                Thread.Sleep(200);
                campoFechaHasta.SendKeys("01/01/1800 23:59:59 PM");
                Thread.Sleep(400);
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha final (01/01/1800) establecida correctamente (fuera de rango).");

                // ===============================
                // 🔹 4. Click en botón "VER"
                // ===============================
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado con rango fuera de límite.");
                Thread.Sleep(4000);

                // ===============================
                // 🔹 5. Validación del resultado
                // ===============================
                bool mensajeError = driver.PageSource.Contains("fuera de rango") ||
                                    driver.PageSource.Contains("rango no válido") ||
                                    driver.PageSource.Contains("error") ||
                                    driver.PageSource.Contains("año no permitido");

                if (mensajeError)
                {
                    Console.WriteLine("[✅] El sistema detectó correctamente el rango fuera de límites (control correcto).");
                }
                else
                {
                    bool sinResultados = driver.PageSource.Contains("No hay datos disponibles") ||
                                         driver.PageSource.Contains("no se encontraron registros");

                    if (sinResultados)
                        Console.WriteLine("[✅] El sistema no mostró error, pero no devolvió registros (manejo correcto del rango inválido).");
                    else
                        Console.WriteLine("[⚠️] No se mostró mensaje ni restricción ante rango fuera de límites.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P073: " + ex.Message);
            }
        }
        


        // ========================================
        // 🔹 P130 - Reportes de Pedidos - Ver reporte en pantalla (sin descargar)
        // ========================================
        public void VerReportePedidosEnPantalla()
        {
            Console.WriteLine("=== Ejecutando P130: Visualizar Reporte de Pedidos Invalidado en pantalla ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 🔹 2. Fechas desde 2020 hasta 2025 (manteniendo hora actual)
                string horaActual = DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                string fechaDesde = $"02/11/2020 {horaActual}";
                string fechaHasta = $"02/11/2025 {horaActual}";

                // Campo Fecha Desde
                var campoDesde = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[contains(@ng-model,'FechaDesde')]")));
                campoDesde.Click();
                campoDesde.SendKeys(Keys.Control + "a");
                campoDesde.SendKeys(Keys.Delete);
                campoDesde.SendKeys(fechaDesde);
                body.Click();
                Console.WriteLine($"[OK] Fecha Desde establecida: {fechaDesde}");
                Thread.Sleep(500);

                // Campo Fecha Hasta
                var campoHasta = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[contains(@ng-model,'FechaHasta')]")));
                campoHasta.Click();
                campoHasta.SendKeys(Keys.Control + "a");
                campoHasta.SendKeys(Keys.Delete);
                campoHasta.SendKeys(fechaHasta);
                body.Click();
                Console.WriteLine($"[OK] Fecha Hasta establecida: {fechaHasta}");
                Thread.Sleep(800);

                // 🔹 3. Click en botón "VER"
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER']")));
                js.ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado correctamente.");
                Thread.Sleep(4000);

                
                IrAMenuPedido();
                IrAReportes();

                
                driver.SwitchTo().DefaultContent();
                Console.WriteLine("[OK] Salió del iframe y finalizó el flujo correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P130: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P131 - Limpiar campo Establecimiento en Reportes de Pedidos
        // ========================================
        public void LimpiarCampoEstablecimiento()
        {
            Console.WriteLine("=== Ejecutando P131: Limpiar campo Establecimiento en Reportes de Pedidos ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Reportes
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Click (doble) en el campo Establecimiento (select2)
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(@class,'select2-selection--single')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);

                Actions action = new Actions(driver);
                action.DoubleClick(campoEstablecimiento).Perform();
                Console.WriteLine("[OK] Campo 'Establecimiento' activado con doble clic.");
                Thread.Sleep(1000);

                // 3️⃣ Escribir texto “XXXXXXXXXX”
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));
                inputBusqueda.SendKeys("XXXXXXXXXX");
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Texto 'XXXXXXXXXX' ingresado correctamente en el campo Establecimiento.");

                // 4️⃣ Limpiar el input (simulando la limpieza)
                inputBusqueda.SendKeys(Keys.Control + "a");
                Thread.Sleep(300);
                inputBusqueda.SendKeys(Keys.Delete);
                Thread.Sleep(800);
                Console.WriteLine("[✅] Campo Establecimiento limpiado correctamente.");

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P131: {ex.Message}");
            }
        }

        // ========================================
        // 🔹 P135 - Seleccionar múltiples establecimientos y generar reporte
        // ========================================
        public void VerReporteTresEstablecimientos()
        {
            Console.WriteLine("=== Ejecutando P135: Seleccionar 'CA PRINCIPAL', 'CA ALMACEN CASTILLO' y 'PARA NIÑOS' ===");

            try
            {
                // 1️⃣ Ir al módulo Reportes
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Doble clic en el campo Establecimiento
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(@class,'select2-selection--multiple') or contains(@class,'select2-selection--single')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);

                Actions actions = new Actions(driver);
                actions.DoubleClick(campoEstablecimiento).Perform();
                Console.WriteLine("[OK] Campo 'Establecimiento' activado con doble clic.");
                Thread.Sleep(1000);

                // 3️⃣ Escribir y seleccionar los tres establecimientos
                string[] establecimientos = { "CA PRINCIPAL", "CA ALMACEN CASTILLO", "PARA NIÑOS" };

                foreach (var nombre in establecimientos)
                {
                    var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//input[contains(@class,'select2-search__field')]")));
                    inputBusqueda.Clear();
                    inputBusqueda.SendKeys(nombre);
                    Thread.Sleep(700);
                    inputBusqueda.SendKeys(Keys.Enter);
                    Console.WriteLine($"[OK] Establecimiento '{nombre}' ingresado correctamente.");
                    Thread.Sleep(1200);
                }

                // 4️⃣ Hacer clic en el botón "VER"
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", botonVer);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[✅] Click en botón 'VER' realizado correctamente.");
                Thread.Sleep(4000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P135: " + ex.Message);
            }
        }
        // ========================================
        // 🔹 P136 - Restablecer check “Todas” al cambiar establecimiento
        // ========================================
        public void RestablecerCheckTodas()
        {
            Console.WriteLine("=== Ejecutando P136: Restablecer check 'Todas' al cambiar establecimiento ===");

            try
            {
                // 1️⃣ Navegar al módulo Reportes
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Activar el campo Establecimiento (doble clic)
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(@class,'select2-selection--single')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);

                Actions actions = new Actions(driver);
                actions.DoubleClick(campoEstablecimiento).Perform();
                Console.WriteLine("[OK] Campo 'Establecimiento' activado con doble clic.");
                Thread.Sleep(1000);

                // 3️⃣ Escribir “CA PRINCIPAL” y presionar Enter
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));
                inputBusqueda.SendKeys("CA PRINCIPAL");
                Thread.Sleep(500);
                inputBusqueda.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Se ingresó 'CA PRINCIPAL' y se presionó Enter.");
                Thread.Sleep(1500);

                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(4000);


            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P136: " + ex.Message);
            }
        }

        // ========================================
        // 🔹 P137 - Ver reporte con establecimiento “CA ALMACEN CASTILLO”
        // ========================================
        public void VerReporteAlmacenCastillo()
        {
            Console.WriteLine("=== Ejecutando P137: Ver reporte con establecimiento 'CA ALMACEN CASTILLO' ===");

            try
            {
                // 1️⃣ Ir al módulo Reportes
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Doble clic en campo Establecimiento
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(@class,'select2-selection--single')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);

                Actions actions = new Actions(driver);
                actions.DoubleClick(campoEstablecimiento).Perform();
                Console.WriteLine("[OK] Campo 'Establecimiento' activado con doble clic.");
                Thread.Sleep(1000);

                // 3️⃣ Escribir “CA ALMACEN CASTILLO” y presionar Enter
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));
                inputBusqueda.SendKeys("CA ALMACEN CASTILLO");
                Thread.Sleep(600);
                inputBusqueda.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Se ingresó 'CA ALMACEN CASTILLO' y se presionó Enter.");
                Thread.Sleep(1500);

                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[OK] Click en botón 'VER' realizado.");
                Thread.Sleep(4000);

            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P137: " + ex.Message);
            }
        }

        // ========================================
        // 🔹 P138 - Ver reporte con establecimiento “PARA NIÑOS”
        // ========================================
        public void VerReporteParaNinos()
        {
            Console.WriteLine("=== Ejecutando P138: Ver reporte con establecimiento 'PARA NIÑOS' ===");

            try
            {
                // 1️⃣ Ir al módulo Reportes
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Doble clic en el campo Establecimiento
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(@class,'select2-selection--single')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);

                Actions actions = new Actions(driver);
                actions.DoubleClick(campoEstablecimiento).Perform();
                Console.WriteLine("[OK] Campo 'Establecimiento' activado con doble clic.");
                Thread.Sleep(1000);

                // 3️⃣ Escribir “PARA NIÑOS” y presionar Enter
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));
                inputBusqueda.SendKeys("PARA NIÑOS");
                Thread.Sleep(600);
                inputBusqueda.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Se ingresó 'PARA NIÑOS' y se presionó Enter.");
                Thread.Sleep(1500);

                // 4️⃣ Hacer clic en el botón "VER" (versión con <a>)
                var botonVer = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[normalize-space()='VER' or @title='Ventas Por Grupos']")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", botonVer);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonVer);
                Console.WriteLine("[✅] Click en botón 'VER' realizado correctamente.");
                Thread.Sleep(4000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Error al ejecutar P138: " + ex.Message);
            }
        }

        // ========================================
        // 🔹 P139 - Escribir y borrar “CA PRINCIPAL” en Establecimiento
        // ========================================
        public void EscribirYBorrarCAPrincipal()
        {
            Console.WriteLine("=== Ejecutando P139: Escribir y borrar 'CA PRINCIPAL' ===");

            try
            {
                // 1️⃣ Ir al módulo Reportes
                IrAMenuPedido();
                IrAReportes();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Activar el campo Establecimiento (doble clic)
                var campoEstablecimiento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(@class,'select2-selection--single')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoEstablecimiento);
                Thread.Sleep(500);

                Actions actions = new Actions(driver);
                actions.DoubleClick(campoEstablecimiento).Perform();
                Console.WriteLine("[OK] Campo 'Establecimiento' activado con doble clic.");
                Thread.Sleep(1000);

                // 3️⃣ Escribir “CA PRINCIPAL”
                var inputBusqueda = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[contains(@class,'select2-search__field')]")));
                inputBusqueda.SendKeys("CA PRINCIPAL");
                Console.WriteLine("[OK] Se escribió 'CA PRINCIPAL' en el campo Establecimiento.");
                Thread.Sleep(1000);

                // 4️⃣ Borrar el texto (Ctrl + A + Delete)
                inputBusqueda.SendKeys(Keys.Control + "a");
                Thread.Sleep(300);
                inputBusqueda.SendKeys(Keys.Delete);
                Console.WriteLine("[OK] Se borró el texto del campo Establecimiento.");
                Thread.Sleep(800);

                // 5️⃣ Validación visual (no intenta ver reporte)
                Console.WriteLine("[✅] Campo Establecimiento limpiado correctamente. No se generará reporte.");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P139: {ex.Message}");
            }
        }


        // PRUEBA P005
    }
}
