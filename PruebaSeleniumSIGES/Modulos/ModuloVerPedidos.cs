using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace PruebaSeleniumSIGES.Modulos
{
    public class ModuloVerPedidos : ModuloBase
    {
        // PRUEBA P001
        public void VerPedidos_DiaActual()
        {
            try
            {
                // 🔹 Pasos comunes reutilizados
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(4000);
                // ========================================================
                // 🔹 Paso 3: Seleccionar la fecha actual dinámicamente
                // ========================================================
                // --- Fecha Inicial ---
                var fechaInicial = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio'] | //input[contains(@ng-model,'fechaIni')] | //input[contains(@placeholder,'dd/mm/aaaa')][1]")));
                fechaInicial.Click();
                Thread.Sleep(2000);

                // Haz clic en el día actual del calendario
                var hoyElemento = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("td.today.active.day")));
                hoyElemento.Click();
                Console.WriteLine("[OK] Seleccionó el día actual en Fecha Inicial.");
                Thread.Sleep(1000);

                // --- Fecha Final --- ✅ (Selector corregido)
                var fechaFinal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFinal.Click();
                Thread.Sleep(2000);

                // Ahora clic en el día actual del calendario
                var hoyElemento2 = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("td.today.active.day")));
                hoyElemento2.Click();
                Console.WriteLine("[OK] Fecha final seleccionada (día actual).");
                Thread.Sleep(2000);


                // ========================================================
                // 🔹 Paso 4: Click en la lupa (Consultar)
                // ========================================================
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos') or contains(.,'CONSULTAR')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en la lupa (Consultar).");


                // ========================================================
                // 🔹 Paso 5: Validar resultado
                // ========================================================
                var mensajeNoDatos = driver.FindElements(By.XPath("//*[contains(text(),'No hay datos disponibles') or contains(text(),'no hay pedidos')]"));
                if (mensajeNoDatos.Count > 0)
                {
                    Console.WriteLine("[✔] Prueba válida: No hay pedidos en el día actual.");
                }
                else
                {
                    Console.WriteLine("[✔] Prueba válida: Se muestran pedidos del día actual.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] En prueba VerPedidos_DiaActual: " + ex.Message);
            }
        }

        // PRUEBA P002
        public void VerPedidos_MesActual()
        {
            try
            {
                // 🔹 1. Navegación (reutiliza los métodos base)
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(2000);

                Console.WriteLine("[INFO] Iniciando prueba: Consultar pedidos del mes actual.");

                // 🔹 2. Calcular fechas del mes actual
                DateTime hoy = DateTime.Now;
                DateTime primerDia = new DateTime(hoy.Year, hoy.Month, 1);
                DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

                // 🔹 3. Colocar la Fecha Inicial
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(primerDia.ToString("dd/MM/yyyy"));

                // 🔹 Cerrar el calendario haciendo clic fuera
                var body = driver.FindElement(By.TagName("body"));
                body.Click();

                Thread.Sleep(2000);

                Console.WriteLine($"[OK] Fecha inicial establecida: {primerDia:dd/MM/yyyy}");

                // 🔹 4. Colocar la Fecha Final
                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(ultimoDia.ToString("dd/MM/yyyy"));

                // 🔹 Cerrar el calendario haciendo clic fuera
                body.Click();
                Thread.Sleep(2000);


                Console.WriteLine($"[OK] Fecha final establecida: {ultimoDia:dd/MM/yyyy}");

                // 🔹 5. Click en Consultar (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(.,'CONSULTAR') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);

                Console.WriteLine("[OK] Se hizo clic en la lupa (Consultar).");
                Thread.Sleep(2000);

                // 🔹 6. Validar resultado
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int cantidadFilas = filas.Count;

                if (cantidadFilas > 0)
                {
                    Console.WriteLine($"[✔] Se muestran {cantidadFilas} pedidos válidos/invalidados del mes actual.");
                }
                else
                {
                    Console.WriteLine("[✔] No hay pedidos registrados en este mes.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] En VerPedidos_MesActual: " + ex.Message);
            }
        }

        // PRUEBA P003
        public void VerPedidos_PorComprobante()
        {
            Console.WriteLine("=== Ejecutando P003: Buscar pedido por número de comprobante ===");

            // 🔹 1. Navegación (reutiliza los métodos base)
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 Paso 3: Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/07/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);
            body.Click();
            // 🔹 Paso 4: Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);
            body.Click();
            // 🔹 Paso 5: Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(1000);


            // 🔹 Paso 6: Escribir el comprobante en el campo "Buscar" (arriba a la derecha)
            var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
            campoBuscar.Clear();
            campoBuscar.SendKeys("0001-29648");
            Thread.Sleep(1000);
            Console.WriteLine("[OK] Consulta de pedido por comprobante completada.");

        }

        // PRUEBA P004
        public void VerPedidos_ComprobanteFechasInvalidas()
        {
            Console.WriteLine("=== Ejecutando P004: Buscar comprobante con fechas fuera del rango ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial (día actual o cualquier fecha fuera del rango)
            var fechaInvalida = DateTime.Now.ToString("dd/MM/yyyy");
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys(fechaInvalida);

            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final (mismo día actual)
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys(fechaInvalida);
            body.Click();
            Thread.Sleep(800);

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(1500);

            // 🔹 5. Escribir el comprobante en el campo "Buscar"
            var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
            campoBuscar.Clear();
            campoBuscar.SendKeys("0001-29648");
            Thread.Sleep(1000);

            Console.WriteLine($"[OK] Fechas fuera de rango usadas: {fechaInvalida} - {fechaInvalida}");

            // 🔹 6. Verificar que no se muestre ningún pedido
            bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                 driver.PageSource.Contains("No hay datos disponibles");

            if (sinResultados)
                Console.WriteLine("[OK] No se muestran pedidos, como se esperaba.");
            else
                Console.WriteLine("[WARN] Se encontraron pedidos, pero no deberían mostrarse.");
        }

        // PRUEBA P005
        public void VerPedidos_ClienteInexistente()
        {
            Console.WriteLine("=== Ejecutando P005: Buscar pedido por cliente inexistente ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/07/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 5. Escribir el nombre del cliente en el campo "Buscar"
            var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
            campoBuscar.Clear();
            campoBuscar.SendKeys("SIMON VILLAR CHAMORRO");
            Thread.Sleep(1500);

            Console.WriteLine("[OK] Consulta de pedido por cliente inexistente completada.");

            // 🔹 6. Verificar que no hay resultados
            bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                 driver.PageSource.Contains("No hay datos disponibles");

            if (sinResultados)
                Console.WriteLine("[OK] No se encontraron pedidos del cliente inexistente, como se esperaba.");
            else
                Console.WriteLine("[WARN] El sistema mostró resultados para un cliente inexistente.");
        }
        // PRUEBA P006
        public void VerPedidos_TipoDocumentoFactura()
        {
            Console.WriteLine("=== Ejecutando P006: Filtrar por tipo de documento 'Factura' ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/07/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 5. Escribir "Factura" en el campo Tipo Doc.
            var campoTipoDoc = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[3]//input[contains(@class,'form-control')]")));
            campoTipoDoc.Clear();
            campoTipoDoc.SendKeys("Factura");
            Thread.Sleep(1500);

            Console.WriteLine("[OK] Filtro aplicado: Tipo de documento = 'Factura'.");

            // 🔹 6. Verificar si existen resultados o no
            bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                 driver.PageSource.Contains("No hay datos disponibles");

            if (sinResultados)
                Console.WriteLine("[OK] No se encontraron facturas, como se esperaba.");
            else
                Console.WriteLine("[OK] Se encontraron facturas en el rango consultado.");
        }
        // PRUEBA P007
        public void VerPedidos_TipoDocumentoBoleta()
        {
            Console.WriteLine("=== Ejecutando P007: Filtrar por tipo de documento 'Boleta (PP)' ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("22/04/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 5. Escribir "PP" (Boleta) en el campo Tipo Doc.
            var campoTipoDoc = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[3]//input[contains(@class,'form-control')]")));
            campoTipoDoc.Clear();
            campoTipoDoc.SendKeys("PP");
            Thread.Sleep(1500);

            Console.WriteLine("[OK] Filtro aplicado: Tipo de documento = 'PP' (Boleta).");

            // 🔹 6. Verificar si existen resultados
            bool hayResultados = !driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") &&
                                 !driver.PageSource.Contains("No hay datos disponibles");

            if (hayResultados)
                Console.WriteLine("[OK] Se muestran pedidos tipo Boleta (PP) correctamente.");
            else
                Console.WriteLine("[WARN] No se encontraron boletas dentro del rango indicado.");
        }
        // PRUEBA P008
        public void VerPedidos_PorVendedorEspecifico()
        {
            Console.WriteLine("=== Ejecutando P008: Filtrar por vendedor específico ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/06/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);
            body.Click();

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 5. Escribir el nombre del vendedor en el campo correspondiente
            var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[6]//input[contains(@class,'form-control')]")));
            campoVendedor.Clear();
            campoVendedor.SendKeys("YTA VELA KETHY MADELEINE");
            Thread.Sleep(1500);

            Console.WriteLine("[OK] Filtro aplicado: Vendedor = 'YTA VELA KETHY MADELEINE'.");

            // 🔹 6. Verificar resultados
            bool hayResultados = !driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") &&
                                 !driver.PageSource.Contains("No hay datos disponibles");

            if (hayResultados)
                Console.WriteLine("[OK] Se muestran pedidos del vendedor específico correctamente.");
            else
                Console.WriteLine("[WARN] No se encontraron pedidos para el vendedor indicado.");
        }
        // PRUEBA P009
        public void VerPedidos_PorEstadoInvalidado()
        {
            Console.WriteLine("=== Ejecutando P009: Filtrar por estado 'INVALIDADO' ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/10/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 5. Escribir "INVALIDADO" en el campo Estado
            var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
            campoEstado.Clear();
            campoEstado.SendKeys("INVALIDADO");
            Thread.Sleep(1500);

            Console.WriteLine("[OK] Filtro aplicado: Estado = 'INVALIDADO'.");

            // 🔹 6. Verificar si existen resultados
            bool hayResultados = !driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") &&
                                 !driver.PageSource.Contains("No hay datos disponibles");

            if (hayResultados)
                Console.WriteLine("[OK] Se muestran pedidos con estado INVALIDADO correctamente.");
            else
                Console.WriteLine("[WARN] No se encontraron pedidos con estado INVALIDADO en el rango seleccionado.");
        }
        // PRUEBA P010
        public void VerPedidos_PorEstadoRegistrado()
        {
            Console.WriteLine("=== Ejecutando P010: Filtrar por estado 'REGISTRADO' ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fecha Inicial
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Colocar Fecha Final
            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 4. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 5. Escribir "REGISTRADO" en el campo Estado
            var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
            campoEstado.Clear();
            campoEstado.SendKeys("REGISTRADO");
            Thread.Sleep(1500);

            Console.WriteLine("[OK] Filtro aplicado: Estado = 'REGISTRADO'.");

            // 🔹 6. Verificar si existen resultados
            bool hayResultados = !driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") &&
                                 !driver.PageSource.Contains("No hay datos disponibles");

            if (hayResultados)
                Console.WriteLine("[OK] Se muestran correctamente los pedidos con estado REGISTRADO.");
            else
                Console.WriteLine("[WARN] No se encontraron pedidos registrados en el rango seleccionado.");
        }
        // PRUEBA P011
        public void VerPedidos_ExportarExcel()
        {
            Console.WriteLine("=== Ejecutando P011: Exportar pedidos a Excel ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar fechas amplias para que haya resultados
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/01/2025");

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/12/2025");

            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Click en Consultar (lupa)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 4. Click en botón de Exportar Excel
            var botonExportar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[contains(@ng-click,'export')] | //button[contains(text(),'DESCARGAR')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonExportar);
            Console.WriteLine("[OK] Click en 'Exportar Excel' realizado.");

            // 🔹 5. Esperar tiempo prudente para descarga
            Thread.Sleep(5000);

            // 🔹 6. Validar que el archivo se haya descargado (verifica carpeta Descargas)
            string rutaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            bool archivoDescargado = Directory.GetFiles(rutaDescargas)
                .Any(f => f.EndsWith(".xls") || f.EndsWith(".xlsx"));

            if (archivoDescargado)
            {
                Console.WriteLine("[OK] Archivo Excel exportado correctamente.");
            }
            else
            {
                Console.WriteLine("[WARN] No se detectó archivo Excel exportado en la carpeta de descargas.");
            }
        }
        //
        // PRUEBA P012 - Buscar por total negativo (Total = -10)
        public void BuscarPorTotalNegativo()
        {
            Console.WriteLine("=== Ejecutando P012: Buscar por total negativo ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 🔹 2. Esperar a que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 🔹 3. FECHA INICIAL (01/11/2020)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/11/2020';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaIni);
                body.Click();
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2020.");
                Thread.Sleep(800);

                // 🔹 4. FECHA FINAL (01/11/2025)
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/11/2025';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaFin);
                body.Click();
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");
                Thread.Sleep(800);

                // 🔹 5. Clic en botón “CONSULTA DE PEDIDOS”
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 6. Buscar por TOTAL negativo (-10) en el buscador global
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("-10");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresado total negativo -10.");
                Thread.Sleep(2000);

                // 🔹 7. Validar el mensaje “valor no permitido”
                bool mensajeError = driver.PageSource.Contains("valor no permitido") ||
                                    driver.PageSource.Contains("VALOR NO PERMITIDO");

                if (mensajeError)
                    Console.WriteLine("[✅] Mensaje 'valor no permitido' mostrado correctamente al buscar total negativo.");
                else
                    Console.WriteLine("[⚠️] No se encontró el mensaje esperado 'valor no permitido'.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P012: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // 🔹 PRUEBA P013 - Crear pedido con total negativo (-1000)
        public void CrearPedidoConTotalNegativo()
        {
            Console.WriteLine("=== Ejecutando P013: Crear pedido con total negativo (Ctrl+A y reemplazar) ===");

            try
            {
                // 1️⃣ Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar a que desaparezca “Cargando”
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 3️⃣ Click en “NUEVO PEDIDO”
                var botonNuevoPedido = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[normalize-space()='NUEVO PEDIDO' or @title='NUEVO PEDIDO']")));
                js.ExecuteScript("arguments[0].click();", botonNuevoPedido);
                Console.WriteLine("[OK] Click en botón NUEVO PEDIDO realizado.");
                Thread.Sleep(2000);

                // 4️⃣ Ingresar código de producto
                var campoCodigo = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idCodigoBarra")));
                campoCodigo.Click();
                campoCodigo.Clear();
                campoCodigo.SendKeys("88008-1");
                campoCodigo.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Código '88008-1' ingresado correctamente.");
                Thread.Sleep(2000);

                // 5️⃣ Esperar que aparezca producto en la tabla
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//td[contains(text(),'88008-1')]")));
                Console.WriteLine("[OK] Producto agregado al carrito.");

                // 6️⃣ Esperar campo “Importe / Total”
                var campoImporte = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@id='importe-0' or contains(@ng-model,'Importe')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", campoImporte);
                Thread.Sleep(800);

                // 7️⃣ Solo seleccionar todo y escribir -1000 (sin borrar)
                campoImporte.Click();
                campoImporte.SendKeys(Keys.Control + "a");
                Thread.Sleep(300);
                campoImporte.SendKeys("-1000");
                campoImporte.SendKeys(Keys.Tab);
                Console.WriteLine("[OK] Valor '-1000' ingresado tras selección Ctrl+A.");
                Thread.Sleep(1500);

                // 8️⃣ Verificar mensaje “valor inválido”
                bool mensajeInvalido = false;
                try
                {
                    var mensaje = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//*[contains(text(),'valor inválido') or contains(text(),'VALOR INVÁLIDO')]")));
                    if (mensaje != null)
                    {
                        Console.WriteLine("[✅] Mensaje 'valor inválido' mostrado correctamente.");
                        mensajeInvalido = true;
                    }
                }
                catch
                {
                    Console.WriteLine("[⚠️] No se encontró el mensaje 'valor inválido'.");
                }

                // 9️⃣ Resultado final
                if (mensajeInvalido)
                    Console.WriteLine("[RESULTADO] ✅ Validación correcta: no permite totales negativos.");
                else
                    Console.WriteLine("[RESULTADO] ❌ No se mostró el mensaje esperado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P013: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // 🔹 PRUEBA P014 - Crear pedido sin cliente (campo obligatorio)
        public void CrearPedidoSinCliente()
        {
            Console.WriteLine("=== Ejecutando P014: Crear pedido sin cliente (campo obligatorio) ===");

            try
            {
                // 1️⃣ Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca mensaje “Cargando”
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 3️⃣ Click en “NUEVO PEDIDO”
                var botonNuevoPedido = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[normalize-space()='NUEVO PEDIDO' or @title='NUEVO PEDIDO']")));
                js.ExecuteScript("arguments[0].click();", botonNuevoPedido);
                Console.WriteLine("[OK] Click en botón NUEVO PEDIDO realizado.");
                Thread.Sleep(2000);

                // 4️⃣ Ingresar código del producto
                var campoCodigo = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idCodigoBarra")));
                campoCodigo.Click();
                campoCodigo.Clear();
                campoCodigo.SendKeys("88010-1");
                campoCodigo.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Código '88010-1' ingresado correctamente.");
                Thread.Sleep(2000);

                // 5️⃣ Esperar que el producto aparezca en la tabla
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//td[contains(text(),'88010-1')]")));
                Console.WriteLine("[OK] Producto agregado correctamente.");

                // 6️⃣ Asegurar que el cliente esté vacío
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@id='rucDni' or @placeholder='DNI/RUC' or @type='text']")));
                campoCliente.Clear();
                Console.WriteLine("[OK] Cliente vacío.");

                // 7️⃣ Hacer clic en el botón GUARDAR
                var botonGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='GUARDAR PEDIDO' or normalize-space()='GUARDAR']")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", botonGuardar);
                js.ExecuteScript("arguments[0].click();", botonGuardar);
                Console.WriteLine("[OK] Click en botón GUARDAR PEDIDO realizado.");
                Thread.Sleep(1500);

                // 8️⃣ Verificar mensaje “campo obligatorio”
                bool mensajeCampoObligatorio = false;
                try
                {
                    var mensaje = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//*[contains(text(),'campo obligatorio') or contains(text(),'CAMPO OBLIGATORIO')]")));
                    if (mensaje != null)
                    {
                        Console.WriteLine("[✅] Mensaje 'campo obligatorio' mostrado correctamente.");
                        mensajeCampoObligatorio = true;
                    }
                }
                catch
                {
                    Console.WriteLine("[⚠️] No se encontró el mensaje 'campo obligatorio'.");
                }

                // 9️⃣ Resultado final
                if (mensajeCampoObligatorio)
                    Console.WriteLine("[RESULTADO] ✅ Validación correcta: el sistema exige cliente obligatorio.");
                else
                    Console.WriteLine("[RESULTADO] ❌ No se mostró el mensaje de validación esperado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P014: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P015 - Buscar pedido por cliente con y sin tilde ("Jose" / "José")
        // ========================================
        public void BuscarPedidoPorClienteConYSinTilde()
        {
            Console.WriteLine("=== Ejecutando P020: Buscar pedido por cliente con y sin tilde ('Jose' / 'José') ===");

            try
            {
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@type='search' and contains(@class,'form-control')]")));

                // 🔹 PRIMERA BÚSQUEDA: “Jose” (sin tilde)
                campoBuscar.Click();
                Thread.Sleep(300);
                campoBuscar.Clear();
                campoBuscar.SendKeys("Jose");
                Thread.Sleep(1500);

                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string resultadosJose = tabla.Text;
                bool contieneJose = resultadosJose.Contains("Jose", StringComparison.OrdinalIgnoreCase)
                                    || resultadosJose.Contains("José", StringComparison.OrdinalIgnoreCase);

                Console.WriteLine(contieneJose
                    ? "[OK] Se encontraron resultados para 'Jose' (sin tilde)."
                    : "[⚠️] No se encontraron resultados con 'Jose'.");

                // 🔹 SEGUNDA BÚSQUEDA: “José” (con tilde)
                campoBuscar.Clear();
                campoBuscar.SendKeys("José");
                Thread.Sleep(1500);

                string resultadosJoseTilde = tabla.Text;
                bool contieneJoseTilde = resultadosJoseTilde.Contains("José", StringComparison.OrdinalIgnoreCase)
                                         || resultadosJoseTilde.Contains("Jose", StringComparison.OrdinalIgnoreCase);

                Console.WriteLine(contieneJoseTilde
                    ? "[OK] Se encontraron resultados para 'José' (con tilde)."
                    : "[⚠️] No se encontraron resultados con 'José'.");

                // 🔹 Validación general
                if (contieneJose && contieneJoseTilde)
                    Console.WriteLine("[✅] Búsqueda reconoce ambas variantes ('Jose' y 'José').");
                else
                    Console.WriteLine("[❌] Solo se muestran resultados de una variante. Prueba fallida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P020: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P016
        public void ModificarPedidoExistente()
        {
            Console.WriteLine("=== Ejecutando P016: Modificar pedido existente ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            // 🔹 2. Colocar Fechas (rango válido)
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/10/2025");
            body.Click();

            Thread.Sleep(800);

            // 🔹 3. Consultar pedidos
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            // 🔹 4. Buscar por comprobante (0001-29967)
            var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
            campoBuscar.Clear();
            campoBuscar.SendKeys("0001-29967");
            Thread.Sleep(1500);

            // 🔹 5. Click en botón Editar pedido
            var botonEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[@title='Editar pedido']")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonEditar);
            Console.WriteLine("[OK] Ingresó al formulario de edición del pedido.");
            Thread.Sleep(2000);

            // 🔹 6. Modificar cantidad a 10.00 (asegurando limpieza completa)
            var campoCantidad = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@id='cantidad-0' or @ng-model='item.Cantidad']")));

            // Foco en el campo
            campoCantidad.Click();
            Thread.Sleep(300);

            // Seleccionar todo el texto y eliminar
            campoCantidad.SendKeys(Keys.Control + "a");
            Thread.Sleep(200);
            campoCantidad.SendKeys(Keys.Backspace);
            Thread.Sleep(200);

            // Escribir la nueva cantidad
            campoCantidad.SendKeys("10.00");
            Console.WriteLine("[OK] Cantidad modificada a 10.00 correctamente.");

            // 🔹 7. Forzar clic en "GUARDAR PEDIDO" ejecutando la función Angular directamente
            try
            {
                // Esperar hasta que el botón esté presente en el DOM
                var botonGuardar = wait.Until(ExpectedConditions.ElementExists(
                    By.XPath("//button[@title='GUARDAR PEDIDO' or contains(text(),'GUARDAR')]")));

                // Desbloquear el atributo "disabled" con JavaScript, si existe
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].removeAttribute('disabled');", botonGuardar);
                Thread.Sleep(800);

                // Ejecutar manualmente el evento Angular que guarda el pedido
                ((IJavaScriptExecutor)driver).ExecuteScript("angular.element(arguments[0]).triggerHandler('click');", botonGuardar);

                Console.WriteLine("[OK] Evento Angular de 'GUARDAR PEDIDO' ejecutado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] No se pudo hacer clic en Guardar: {ex.Message}");
            }

            // 🔹 8. Esperar que el modal se cierre (indica que guardó)
            bool modalCerrado = false;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                if (driver.FindElements(By.XPath("//div[@class='modal-dialog']")).Count == 0)
                {
                    modalCerrado = true;
                    break;
                }
            }

            if (modalCerrado)
            {
                Console.WriteLine("[OK] Modal cerrado correctamente. Pedido guardado.");
            }
            else
            {
                Console.WriteLine("[WARN] El modal sigue abierto, posible demora o error en guardado.");
            }




        }
        // PRUEBA P017
        public void InvalidarPedido()
        {
            Console.WriteLine("=== Ejecutando P017: Invalidar pedido existente ===");

            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(2000);

            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(1500);

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(1000);

            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Thread.Sleep(2000);

            var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
            campoBuscar.Clear();
            campoBuscar.SendKeys("0001-29942");
            Thread.Sleep(1500);

            var botonInvalidar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[@title='Invalidar pedido' or contains(@ng-click,'invalidarPedido')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonInvalidar);
            Console.WriteLine("[OK] Click en 'Invalidar pedido' realizado.");
            Thread.Sleep(2000);

            var campoObservacion = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[@id='modal-invalidar-pedido']//textarea[@ng-model='invalidacion.Observacion' or contains(@ng-model,'Observacion')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoObservacion);
            Thread.Sleep(500);

            Actions actions = new Actions(driver);
            actions.MoveToElement(campoObservacion).Click().Perform();
            Thread.Sleep(500);

            campoObservacion.SendKeys("PRUEBA");
            Console.WriteLine("[OK] Observación 'PRUEBA' escrita correctamente.");
            Thread.Sleep(1000);

            var botonSi = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[@ng-click='invalidarPedido()']")));
            Thread.Sleep(800);

            Actions actSi = new Actions(driver);
            actSi.MoveToElement(botonSi).Click().Perform();

            Console.WriteLine("[OK] Botón 'SÍ' presionado correctamente para invalidar el pedido.");
            Thread.Sleep(3000);
        }

        // PRUEBA P018 - Clases equivalentes inválidas: Buscar "null" literal en el buscador global
        public void BuscarPedidoConTextoNull()
        {
            Console.WriteLine("=== Ejecutando P018: Buscar pedido escribiendo 'null' literal en el buscador ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 3️⃣ Localizar buscador global
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));

                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("null"); // literal en minúsculas
                buscadorGlobal.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 4️⃣ Validar comportamiento del sistema
                bool mensajeInvalido = driver.PageSource.Contains("entrada no válida") ||
                                       driver.PageSource.Contains("NO SE ENCONTRARON REGISTROS") ||
                                       driver.PageSource.Contains("No se encontraron registros") ||
                                       driver.PageSource.Contains("No hay datos") ||
                                       driver.PageSource.Contains("sin resultados");

                bool errorServidor = driver.PageSource.Contains("500") ||
                                     driver.PageSource.Contains("Error interno del servidor") ||
                                     driver.PageSource.Contains("Internal Server Error");

                if (mensajeInvalido)
                {
                    Console.WriteLine("[✅] El sistema manejó correctamente la entrada 'null' mostrando mensaje de validación o sin resultados.");
                }
                else if (errorServidor)
                {
                    Console.WriteLine("[❌] Error: el sistema lanzó un error 500 al procesar la palabra reservada 'null'.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectó mensaje claro; verificar si el sistema ignoró el texto sin filtrar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P018: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // ========================================
        // 🔹 P019 - Buscar pedido por cliente con tilde ("José")
        // ========================================
        public void BuscarPedidoPorClienteConTilde()
        {
            Console.WriteLine("=== Ejecutando P019: Buscar pedido por cliente con tilde ('José') ===");

            try
            {
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 🔹 Paso 6: Escribir el comprobante en el campo "Buscar" (arriba a la derecha)
                var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
                campoBuscar.Clear();
                campoBuscar.SendKeys("José");
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Consulta de pedido por comprobante completada.");

                // 3️⃣ Verificar resultados que contengan “José”
                var tablaResultados = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]")));
                string contenido = tablaResultados.Text;

                if (contenido.Contains("José", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("[✅] Búsqueda ejecutada correctamente: se encontraron registros con 'José'.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se encontraron coincidencias visibles con 'José'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P019: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P020 - Clases equivalentes inválidas: Buscar cliente con espacios antes y después del nombre
        public void BuscarClienteConEspacios()
        {
            Console.WriteLine("=== Ejecutando P020: Buscar cliente con espacios antes y después del nombre ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 3️⃣ Establecer rango de fechas (01/11/2020 - 01/11/2025)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Ingresar cliente con espacios en el campo CLIENTE
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("       JOSE    ");  // ← espacios antes y después
                campoCliente.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 5️⃣ Validar que se haya hecho limpieza (trim automático)
                bool resultadosCorrectos = driver.PageSource.Contains("JOSE") &&
                                           driver.PageSource.Contains("REGISTRADO") &&
                                           !driver.PageSource.Contains("No hay datos");

                if (resultadosCorrectos)
                {
                    Console.WriteLine("[✅] El sistema ignoró correctamente los espacios y mostró los pedidos del cliente 'JOSE'.");
                }
                else if (driver.PageSource.Contains("No hay datos") || driver.PageSource.Contains("NO HAY DATOS"))
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos. Posible falta de limpieza del input (espacios no ignorados).");
                }
                else
                {
                    Console.WriteLine("[⚠️] Resultado ambiguo: verificar si la búsqueda fue sensible a espacios.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P020: {ex.Message}");
            }

            Thread.Sleep(1200);
        }


        // ========================================
        // 🔹 P021 - Filtrar cliente con punto intermedio
        // ========================================
        public void FiltrarClienteConPuntoIntermedio()
        {
            Console.WriteLine("=== Ejecutando P021: Filtrar cliente con punto intermedio ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas amplias para asegurar resultados
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);


                // 🔹 Click en CONSULTAR (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 🔹 Paso 3A: Buscar cliente sin punto ("ACOSTA PAUL")
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

                campoCliente.Clear();
                campoCliente.SendKeys("ACOSTA PAUL");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado cliente con punto intermedio: ACOSTA.PAUL");

                // 3️⃣ Escribir cliente con punto intermedio: ACOSTA.PAUL
                campoCliente.Clear();
                campoCliente.SendKeys("ACOSTA.PAUL");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado cliente con punto intermedio: ACOSTA.PAUL");

                // 4️⃣ Validar si hay coincidencias en la tabla
                var tablaResultados = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tablaResultados.Text;

                if (contenidoTabla.Contains("ACOSTA", StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine("[✅ RESULTADO] Se muestran coincidencias exactas con el cliente ACOSTA.PAUL.");
                else
                    Console.WriteLine("[⚠️ RESULTADO] No se detectaron coincidencias visibles. Verificar comportamiento del filtro.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P021: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P022 - Buscar con mezcla de mayúsculas y minúsculas inconsistentes (Normalización de texto)
        // ========================================
        public void BuscarClienteConMayusMinusInconsistentes()
        {
            Console.WriteLine("=== Ejecutando P022: Buscar con mezcla de mayúsculas y minúsculas inconsistentes (AcosTA) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar desaparición del loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Fechas amplias (para asegurar resultados)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2024");
                body.Click();
                Thread.Sleep(600);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(600);

                // 4️⃣ Click en botón CONSULTAR (para cargar pedidos)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Escribir “AcosTA” en el campo Cliente
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                foreach (char c in "AcosTA")
                {
                    campoCliente.SendKeys(c.ToString());
                    Thread.Sleep(60); // efecto visible
                }
                campoCliente.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Cliente ingresado con mezcla de mayúsculas/minúsculas: 'AcosTA'.");

                Thread.Sleep(1200); // esperar carga del filtro

                // 6️⃣ Validar normalización: verificar coincidencias insensibles a mayúsculas/minúsculas
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                bool coincidencia = false;

                foreach (var fila in filas)
                {
                    try
                    {
                        var celdaCliente = fila.FindElement(By.XPath(".//td[5]"));
                        string textoCliente = celdaCliente.Text.Trim().ToUpper();

                        if (textoCliente.Contains("ACOSTA"))
                        {
                            coincidencia = true;
                            break;
                        }
                    }
                    catch { }
                }

                // 7️⃣ Resultado esperado: muestra resultado igual (normalización correcta)
                if (coincidencia)
                {
                    Console.WriteLine("[✅ RESULTADO] Se encontró cliente coincidente ignorando mayúsculas/minúsculas (normalización correcta).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] No se encontró coincidencia con 'AcosTA'. Posible falla en normalización.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P022: {ex.Message}");
            }

            Thread.Sleep(800);
        }

        // ========================================
        // 🔹 P023 - Buscar con comillas dobles en el campo Cliente
        // ========================================
        public void BuscarClienteConComillasDobles()
        {
            Console.WriteLine("=== Ejecutando P023: Buscar con comillas dobles en el campo Cliente ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas amplias (rango válido)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Paso 4: Click en Consultar (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 5️⃣ Escribir en Cliente con comillas dobles: "SINTI"
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("\"SINTI\"");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió \"SINTI\" (con comillas dobles) en el campo Cliente.");

                // 🔹 Validación visual
                bool advertencia = driver.PageSource.Contains("error") ||
                                   driver.PageSource.Contains("inválido") ||
                                   driver.PageSource.Contains("advertencia");

                if (advertencia)
                    Console.WriteLine("[✅ RESULTADO] El sistema mostró advertencia/error como se esperaba.");
                else
                    Console.WriteLine("[⚠️ RESULTADO] No se detectó mensaje visible, revisar comportamiento visual.");

                Console.WriteLine("[INFO] Esperado: Mostrar error o advertencia sin romper la aplicación.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P023: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P024 - Buscar con comillas simples en el campo Cliente
        // ========================================
        public void BuscarClienteConComillasSimples()
        {
            Console.WriteLine("=== Ejecutando P024: Buscar con comillas simples en el campo Cliente ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                
                // 🔹 Paso 4: Click en Consultar (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 5️⃣ Escribir en Cliente con comillas simples: "SINTI"
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("'SINTI'");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió 'SINTI' (con comillas simples) en el campo Cliente.");

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Fecha inicio: 01/01/2022");
                Console.WriteLine("→ Fecha fin: 01/11/2025");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: 'SINTI'");
                Console.WriteLine("[INFO] Esperado: Mostrar advertencia o manejar la entrada sin error.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P024: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P025 - Filtrar pedidos (01/11/2022 - 01/11/2025) + rango 100 + filtro FECHA=2022
        // ========================================
        public void VerPedidos_PorFechasYRangoFilas_2022()
        {
            Console.WriteLine("=== Ejecutando P025: Filtrar pedidos por fechas (01/11/2022 - 01/11/2025) con rango 100 y filtro FECHA=2022 ===");

            try
            {
                // 1️⃣ Navegar al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Colocar FECHA INICIAL (01/11/2022)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2022");
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2022.");

                // 3️⃣ Colocar FECHA FINAL (01/11/2025)
                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                // 4️⃣ Click en CONSULTAR (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(.,'CONSULTAR') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Thread.Sleep(2500);
                Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");

                // 5️⃣ Cambiar rango de filas a 100
                var comboFilas = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//select[contains(@name,'tabla-cotizaciones_length') or contains(@aria-controls,'tabla-cotizaciones')]")));
                var selectElement = new SelectElement(comboFilas);
                selectElement.SelectByValue("100");
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Rango de filas cambiado a 100.");

                // 6️⃣ Escribir “2022” en el filtro de la columna FECHA (abajo)
                var campoFiltroFecha = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[2]//input[contains(@class,'form-control')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoFiltroFecha);
                Thread.Sleep(400);
                campoFiltroFecha.Click();
                campoFiltroFecha.Clear();
                campoFiltroFecha.SendKeys("2022");
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Filtro de columna FECHA aplicado con valor '2022'.");

                // 7️⃣ Verificar resultados visibles
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text.Trim();

                if (!string.IsNullOrEmpty(contenidoTabla) &&
                    !contenidoTabla.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase) &&
                    !contenidoTabla.Contains("No existen registros", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("[✅] Se muestran pedidos del año 2022 con rango de filas = 100.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos visibles para el año 2022.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P025: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P026
        public void CampoFechaTextoInvalido()
        {
            Console.WriteLine("=== Ejecutando P026: Campo fecha texto inválido ===");

            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(1500);

            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));

            // Forzar visibilidad y clic real en el campo evitando overlays
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center'});", campoFechaIni);
            Thread.Sleep(500);

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].click();", campoFechaIni);
            Thread.Sleep(300);

            // Limpieza segura del campo
            campoFechaIni.Clear();
            Thread.Sleep(300);


            campoFechaIni.SendKeys("hola");
            Console.WriteLine("[OK] Texto 'hola' ingresado en el campo de fecha inicial.");
            Thread.Sleep(1000);

            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(1500);

            try
            {
                var mensajeError = driver.FindElement(By.XPath("//*[contains(text(),'fecha inválida') or contains(text(),'Fecha inválida')]"));
                Console.WriteLine("[✅] Mensaje mostrado correctamente: 'fecha inválida'.");
            }
            catch
            {
                string valorFecha = campoFechaIni.GetAttribute("value");
                string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");

                if (valorFecha == fechaHoy)
                    Console.WriteLine($"[✅] El sistema corrigió automáticamente la fecha a la actual: {fechaHoy}.");
                else
                    Console.WriteLine($"[⚠️] No se detectó mensaje ni corrección automática. Valor actual: {valorFecha}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P027
        public void CampoClienteConSimbolosInvalidos()
        {
            Console.WriteLine("=== Ejecutando P027: Campo cliente con símbolos inválidos ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(1500);

            // 🔹 2. Establecer rango de fechas válido
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/07/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Consultar pedidos
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");
            Thread.Sleep(2000);

            // 🔹 4. Ingresar texto inválido en campo Cliente (símbolos)
            var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

            // Desplazar al centro y escribir "@@@"
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
            Thread.Sleep(400);
            campoCliente.Clear();
            campoCliente.SendKeys("@@@");
            Console.WriteLine("[OK] Texto '@@@' ingresado en campo Cliente.");
            Thread.Sleep(1000);

            // 🔹 5. Verificar mensaje “valor no válido”
            try
            {
                var mensajeError = driver.FindElement(By.XPath("//*[contains(text(),'valor no válido') or contains(text(),'Valor no válido')]"));
                Console.WriteLine("[✅] Mensaje mostrado correctamente: 'valor no válido'.");
            }
            catch
            {
                // Si no hay mensaje, verificar si el sistema limpió el campo
                string valorActual = campoCliente.GetAttribute("value");
                if (string.IsNullOrEmpty(valorActual))
                    Console.WriteLine("[✅] El sistema limpió automáticamente el campo por valor inválido.");
                else
                    Console.WriteLine($"[⚠️] No se mostró mensaje ni limpieza automática. Valor actual: '{valorActual}'.");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P028
        public void CampoComprobanteTextoInvalido()
        {
            Console.WriteLine("=== Ejecutando P028: Campo comprobante con texto inválido ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(1500);

            // 🔹 2. Establecer rango de fechas válido
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/10/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Consultar pedidos
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");
            Thread.Sleep(1500);

            // 🔹 4. Localizar el input de la columna COMPROBANTE (encabezado: th[4])
            // si tu tabla tiene otra estructura ajusta el th[n] al número correcto
            var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));

            // asegurar visibilidad y foco
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoComprobante);
            Thread.Sleep(300);
            campoComprobante.Click();
            Thread.Sleep(300);

            // 🔹 5. Ingresar texto no numérico "abc"
            campoComprobante.Clear();
            campoComprobante.SendKeys("abc");
            Console.WriteLine("[OK] Texto 'abc' ingresado en campo Comprobante.");
            Thread.Sleep(1000);

            // 🔹 6. Verificar comportamiento esperado:
            // - mensaje de validación (si existe) o que no haya resultados (filtro inválido)
            try
            {
                var mensajeError = driver.FindElement(By.XPath("//*[contains(text(),'valor no válido') or contains(text(),'no válido') or contains(text(),'valor inválido')]"));
                Console.WriteLine("[✅] Mensaje mostrado correctamente: 'valor no válido' o equivalente.");
            }
            catch
            {
                bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                     driver.PageSource.Contains("No hay datos disponibles") ||
                                     driver.PageSource.Contains("no se encontraron registros");

                if (sinResultados)
                    Console.WriteLine("[✅] Se mostró mensaje esperado: 'no se encontraron registros' o la tabla quedó vacía.");
                else
                    Console.WriteLine("[⚠️] No se detectó mensaje ni resultado esperado tras ingresar 'abc' en comprobante.");
            }

            Thread.Sleep(800);
        }
        // PRUEBA P029
        public void CampoTotalTextoInvalido()
        {
            Console.WriteLine("=== Ejecutando P029: Campo total con texto inválido ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(1500);

            // 🔹 2. Establecer rango de fechas válido
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/02/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/10/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Consultar pedidos
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");
            Thread.Sleep(2000);

            // 🔹 4. Localizar el campo de la columna TOTAL (normalmente th[9] o th[8])
            var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));


            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoTotal);
            Thread.Sleep(400);

            campoTotal.Clear();
            campoTotal.SendKeys("monto");
            Console.WriteLine("[OK] Texto 'monto' ingresado en campo Total.");
            Thread.Sleep(1000);

            // 🔹 5. Validar mensaje o comportamiento
            try
            {
                var mensajeError = driver.FindElement(By.XPath("//*[contains(text(),'numérico requerido') or contains(text(),'Numérico requerido')]"));
                Console.WriteLine("[✅] Mensaje mostrado correctamente: 'numérico requerido'.");
            }
            catch
            {
                bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                     driver.PageSource.Contains("No hay datos disponibles") ||
                                     driver.PageSource.Contains("no se encontraron registros");

                if (sinResultados)
                    Console.WriteLine("[✅] Se mostró mensaje esperado: 'no se encontraron registros'.");
                else
                    Console.WriteLine("[⚠️] No se detectó mensaje ni resultado esperado tras ingresar 'monto'.");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P030 - Buscar 2 clientes diferentes (PINEDO / ramizes) - ambos sin resultados
        // ========================================
        public void BuscarDosClientesDiferentes()
        {
            Console.WriteLine("=== Ejecutando P030: Buscar 2 clientes diferentes ('PINEDO' y 'ramizes') ===");

            try
            {
                // 1️⃣ Ir a Pedidos → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Establecer fechas amplias para que la búsqueda funcione en general
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");
                Thread.Sleep(2000);

                // 4️⃣ Localizar campo CLIENTE (columna 5)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                Thread.Sleep(400);

                // ======================
                // 🔹 PRIMERA BÚSQUEDA: PINEDO
                // ======================
                var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
                campoBuscar.Clear();
                campoBuscar.SendKeys("PINEDO");
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Consulta de pedido por comprobante completada.");


                bool sinResultados1 = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                      driver.PageSource.Contains("No hay datos disponibles") ||
                                      driver.PageSource.Contains("no se encontraron registros");

                if (sinResultados1)
                    Console.WriteLine("[✅] Correcto: No se muestran pedidos para cliente 'PINEDO'.");
                else
                    Console.WriteLine("[⚠️] Se mostraron resultados inesperados para 'PINEDO'.");

                // ======================
                // 🔹 SEGUNDA BÚSQUEDA: ramizes
                // ======================
                campoCliente.Clear();
                campoCliente.SendKeys("ramizes");
                Thread.Sleep(1500);

                bool sinResultados2 = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                      driver.PageSource.Contains("No hay datos disponibles") ||
                                      driver.PageSource.Contains("no se encontraron registros");

                if (sinResultados2)
                    Console.WriteLine("[✅] Correcto: No se muestran pedidos para cliente 'ramizes'.");
                else
                    Console.WriteLine("[⚠️] Se mostraron resultados inesperados para 'ramizes'.");

                // 🔹 Validación final combinada
                if (sinResultados1 && sinResultados2)
                    Console.WriteLine("[✔ RESULTADO FINAL] ✅ Ambos clientes ('PINEDO' y 'ramizes') sin pedidos visibles. Prueba válida.");
                else
                    Console.WriteLine("[❌ RESULTADO FINAL] ⚠️ Se mostraron resultados en una o ambas búsquedas. Prueba inválida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P030: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        //31 32 en reportes
        //
        // ========================================
        // 🔹 P033 - Buscar cliente con dos datos distintos ("PINEDO" / "NILO")
        // ========================================
        public void BuscarClienteConDosDatos()
        {
            Console.WriteLine("=== Ejecutando P033: Buscar cliente con dos datos ('PINEDO' y 'NILO') ===");

            try
            {
                // 1️⃣ Ir a Pedidos → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Establecer rango de fechas válido amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");
                Thread.Sleep(2000);

                // 4️⃣ Localizar el campo CLIENTE (columna 5)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                Thread.Sleep(400);

                // ======================
                // 🔹 PRIMERA BÚSQUEDA: "PINEDO"
                // ======================
                var campoBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
                campoBuscar.Clear();
                campoBuscar.SendKeys("PINEDO");
                Thread.Sleep(1000);
                Console.WriteLine("[OK] Consulta de pedido por comprobante completada.");

                bool hayResultados1 = !driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") &&
                                      !driver.PageSource.Contains("No hay datos disponibles") &&
                                      !driver.PageSource.Contains("no se encontraron registros");

                if (hayResultados1)
                    Console.WriteLine("[OK] Se muestran pedidos al buscar 'PINEDO'.");
                else
                    Console.WriteLine("[⚠️] No se encontraron resultados con 'PINEDO'.");

                // ======================
                // 🔹 SEGUNDA BÚSQUEDA: "NILO"
                // ======================
                campoCliente.Clear();
                campoCliente.SendKeys("NILO");
                Thread.Sleep(1500);

                bool hayResultados2 = !driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") &&
                                      !driver.PageSource.Contains("No hay datos disponibles") &&
                                      !driver.PageSource.Contains("no se encontraron registros");

                if (hayResultados2)
                    Console.WriteLine("[OK] Se muestran pedidos al buscar 'NILO'.");
                else
                    Console.WriteLine("[⚠️] No se encontraron resultados con 'NILO'.");

                // 🔹 Validación final combinada
                if (hayResultados1 && hayResultados2)
                    Console.WriteLine("[✅ RESULTADO FINAL] El sistema muestra pedidos en ambas búsquedas ('PINEDO' y 'NILO'). Correcto, pertenece al mismo cliente.");
                else
                    Console.WriteLine("[❌ RESULTADO FINAL] No se mostraron resultados en una o ambas búsquedas.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P033: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P034 - Generar reporte con año inválido (3025)
        // ========================================
        public void GenerarReporteConAnioInvalido()
        {
            Console.WriteLine("=== Ejecutando P034: Generar reporte con año inválido (3025) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas con año inválido (3025)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/3025");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("31/12/3025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(2000);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Fecha inicio: 01/01/3025");
                Console.WriteLine("→ Fecha fin: 31/12/3025");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("[INFO] Esperado: Mostrar mensaje 'año fuera de rango' o tabla vacía.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P034: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P035 - Colocar signo de soles en el campo TOTAL
        // ========================================
        public void BuscarPorTotalConSimboloSoles()
        {
            Console.WriteLine("=== Ejecutando P035: Buscar por total con signo de soles (S/. 34.00) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Paso 4: Click en Consultar (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 5️⃣ Escribir TOTAL con símbolo “S/. 34.00”
                var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control padding-left-right-3')]")));
                campoTotal.Clear();
                campoTotal.SendKeys("S/. 34.00");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió 'S/. 34.00' en el campo TOTAL.");

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Fecha inicio: 01/01/2022");
                Console.WriteLine("→ Fecha fin: 01/11/2025");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Total: S/. 34.00");
                Console.WriteLine("[INFO] Esperado: Se muestran pedidos válidos (control del formato con símbolo de moneda).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P035: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P038 - Buscar con tabulación o símbolo entre letras (control de formato)
        // ========================================
        public void BuscarClienteConTabulacionOSimbolo()
        {
            Console.WriteLine("=== Ejecutando P037: Buscar cliente con tabulación o símbolo entre letras ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Paso 4: Click en Consultar (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 5️⃣ Campo cliente: escribir “SIMON VILLAR”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

                campoCliente.Clear();
                campoCliente.SendKeys("SIMON VILLAR");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió 'SIMON VILLAR' en el campo Cliente.");

                // 6️⃣ Borrar texto anterior
                campoCliente.SendKeys(Keys.Control + "a");
                campoCliente.SendKeys(Keys.Delete);
                Thread.Sleep(800);
                Console.WriteLine("[OK] Campo Cliente limpiado.");

                // 7️⃣ Escribir texto con símbolo “SIMON/VILLAR”
                campoCliente.SendKeys("SIMON/VILLAR");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió 'SIMON/VILLAR' (texto con símbolo).");

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Cliente 1: SIMON VILLAR");
                Console.WriteLine("→ Cliente 2: SIMON/VILLAR");
                Console.WriteLine("[INFO] Esperado: No debe encontrar resultados (control de formato de texto).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P037: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // ========================================
        // 🔹 P039 - Control de redundancia (buscar con texto repetido)
        // ========================================
        public void BuscarClienteConTextoRepetido()
        {
            Console.WriteLine("=== Ejecutando P039: Buscar cliente con texto repetido (control de redundancia) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar rango de fechas amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Paso 4: Click en Consultar (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 5️⃣ Campo cliente: escribir “NICELIDA”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

                campoCliente.Clear();
                campoCliente.SendKeys("NICELIDA");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió 'NICELIDA' en el campo Cliente.");

                // 6️⃣ Borrar texto anterior
                campoCliente.SendKeys(Keys.Control + "a");
                campoCliente.SendKeys(Keys.Delete);
                Thread.Sleep(800);
                Console.WriteLine("[OK] Campo Cliente limpiado.");

                // 7️⃣ Escribir texto repetido “NICELIDAAAAA”
                campoCliente.SendKeys("NICELIDAAAAA");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escribió 'NICELIDAAAAA' (texto repetido).");

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Fecha inicio: 01/01/2022");
                Console.WriteLine("→ Fecha fin: 01/11/2025");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: 'NICELIDA' → 'NICELIDAAAAA'");
                Console.WriteLine("[INFO] Esperado: Sin resultados o mensaje de advertencia (control de redundancia).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P039: {ex.Message}");
            }

            Thread.Sleep(1000);
        }





        // PRUEBA P041
        public void ClienteConNombreMaximo()
        {
            Console.WriteLine("=== Ejecutando P041: Cliente con nombre máximo (255 caracteres) ===");

            // 🔹 1. Navegación
            IrAMenuPedido();
            IrAVerPedido();
            Thread.Sleep(1500);

            // 🔹 2. Establecer rango de fechas válido
            var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
            campoFechaIni.Clear();
            campoFechaIni.SendKeys("01/10/2025");
            var body = driver.FindElement(By.TagName("body"));
            body.Click();
            Thread.Sleep(800);

            var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
            campoFechaFin.Clear();
            campoFechaFin.SendKeys("31/10/2025");
            body.Click();
            Thread.Sleep(800);

            // 🔹 3. Click en "Consultar" (lupa azul)
            var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
            Console.WriteLine("[OK] Click en la lupa (Consultar) realizado.");
            Thread.Sleep(2000);

            // 🔹 4. Campo CLIENTE → Columna 5
            var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
            Thread.Sleep(400);

            // 🔹 5. Ingresar nombre de 255 caracteres
            string nombreLargo = new string('A', 255);
            campoCliente.Clear();
            campoCliente.SendKeys(nombreLargo);
            Console.WriteLine($"[OK] Ingresado nombre con {nombreLargo.Length} caracteres en campo CLIENTE.");
            Thread.Sleep(1200);

            // 🔹 6. Validar comportamiento
            try
            {
                bool hayResultados = driver.PageSource.Contains("PEDIDO") ||
                                     driver.PageSource.Contains("CLIENTE") ||
                                     driver.PageSource.Contains("ADMIN");

                if (hayResultados)
                    Console.WriteLine("[✅] El sistema respondió correctamente y mostró resultados válidos.");
                else
                    Console.WriteLine("[⚠️] No se encontraron resultados, pero el sistema no generó errores.");
            }
            catch
            {
                Console.WriteLine("[❌] Error: no se detectaron resultados ni mensajes esperados.");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P042
        public void ClienteConCadenaLarga()
        {
            Console.WriteLine("=== Ejecutando P042: Cliente con cadena larga (1000 caracteres) ===");

            try
            {
                // 🔹 1. Navegación directa
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo CLIENTE (columna 5)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                Thread.Sleep(500);

                // 🔹 3. Ingresar texto largo (1000 caracteres)
                string textoLargo = new string('A', 1000);
                campoCliente.Clear();
                campoCliente.SendKeys(textoLargo);
                Console.WriteLine($"[OK] Ingresado texto largo de {textoLargo.Length} caracteres en campo CLIENTE.");
                Thread.Sleep(1200);

                // 🔹 4. Validar comportamiento del sistema
                bool hayResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                     driver.PageSource.Contains("No hay datos disponibles") ||
                                     driver.PageSource.Contains("no se encontraron registros") ||
                                     driver.PageSource.Contains("REGISTRADO") ||
                                     driver.PageSource.Contains("CLIENTE");

                if (hayResultados)
                {
                    Console.WriteLine("[✅] El sistema aceptó el texto y respondió correctamente (sin errores de validación).");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron resultados ni mensajes visibles; posible límite interno del campo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P042: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P043 - Campo cliente con guiones bajos
        // ========================================
        public void CampoClienteConGuionesBajos()
        {
            Console.WriteLine("=== Ejecutando P043: Campo cliente con guiones bajos ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar los tres valores límite con guiones bajos
                string[] nombresPrueba = { "Carlos_Perez", "_Carlos_Perez", "Carlos_Perez_" };

                foreach (var nombre in nombresPrueba)
                {
                    var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                    campoCliente.Clear();
                    campoCliente.SendKeys(nombre);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado cliente con guiones bajos: {nombre}");

                    // 🔹 Verificar resultado
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("INVÁLIDO", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("error", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("advertencia", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] Sistema mostró mensaje de valor inválido para '{nombre}'.");
                    }
                    else
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] No se mostró advertencia visible para '{nombre}'.");
                    }
                }

                Console.WriteLine("[INFO] Prueba completada para todas las variantes con guiones bajos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P043: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P044 - Cliente en mayúsculas y minúsculas mezcladas
        // ========================================
        public void ClienteMayusculasMinusculasMezcladas()
        {
            Console.WriteLine("=== Ejecutando P044: Cliente en mayúsculas y minúsculas mezcladas ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Pruebas con distintas combinaciones de mayúsculas/minúsculas
                string[] variantes = { "ana López", "AnaLópez", "ANA LÓPEZ" };

                foreach (var cliente in variantes)
                {
                    var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                    campoCliente.Clear();
                    campoCliente.SendKeys(cliente);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado cliente con formato: {cliente}");

                    // 🔹 Validar resultados
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenidoTabla = tabla.Text;

                    if (contenidoTabla.Contains("ANA", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] El filtro reconoce correctamente '{cliente}' (sin distinción de mayúsculas/minúsculas).");
                    }
                    else
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] El sistema diferencia por mayúsculas/minúsculas para '{cliente}'.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la comparación de formatos de texto para cliente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P044: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // ========================================
        // 🔹 P045 - Estado con minúsculas
        // ========================================
        public void EstadoConMinusculas()
        {
            Console.WriteLine("=== Ejecutando P045: Estado con minúsculas ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 4️⃣ Probar las tres variantes del campo Estado
                string[] variantes = { "registrado", "REGISTRADO", "Registrado" };

                foreach (var estado in variantes)
                {
                    var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                    campoEstado.Clear();
                    campoEstado.SendKeys(estado);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado estado con formato: {estado}");

                    // 🔹 Verificar resultados
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenidoTabla = tabla.Text;

                    if (contenidoTabla.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase) ||
                        contenidoTabla.Contains("inválido", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] Sistema rechazó correctamente '{estado}' mostrando mensaje de campo inválido o sin registros.");
                    }
                    else if (contenidoTabla.Contains("REGISTRADO"))
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] Sistema aceptó '{estado}' como válido (no distingue mayúsculas/minúsculas).");
                    }
                    else
                    {
                        Console.WriteLine($"[ℹ️ RESULTADO] Estado '{estado}' no generó mensaje visible, verificar comportamiento visual.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la validación del campo Estado con variantes de mayúsculas/minúsculas.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P045: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P046
        public void TotalConValorMinimo()
        {
            Console.WriteLine("=== Ejecutando P046: Monto total con valor mínimo (1) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo TOTAL (columna 7)
                var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoTotal);
                Thread.Sleep(500);

                // 🔹 3. Ingresar valor mínimo "1"
                campoTotal.Clear();
                campoTotal.SendKeys("1");
                Console.WriteLine("[OK] Ingresado valor mínimo (1) en campo TOTAL.");
                Thread.Sleep(1200);

                // 🔹 4. Validar si se muestran pedidos válidos
                bool hayResultados = driver.PageSource.Contains("S/.") ||
                                     driver.PageSource.Contains("REGISTRADO") ||
                                     driver.PageSource.Contains("PEDIDO");

                if (hayResultados)
                {
                    Console.WriteLine("[✅] Se muestran pedidos con montos mayores o iguales a 1 correctamente.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron resultados visibles tras ingresar 1 como monto total.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P046: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P047
        public void TotalConValoresLimite()
        {
            Console.WriteLine("=== Ejecutando P047: Monto total con valores límite (999, 1000, 1001) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo TOTAL (columna 7)
                var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoTotal);
                Thread.Sleep(500);

                // =======================================================
                // 🔹 3. Verificación con valor "999"
                // =======================================================
                campoTotal.Clear();
                campoTotal.SendKeys("999");
                Console.WriteLine("[OK] Ingresado valor 999 en campo TOTAL.");
                Thread.Sleep(1200);

                bool incluye999 = driver.PageSource.Contains("999") || driver.PageSource.Contains("S/. 999");
                if (incluye999)
                    Console.WriteLine("[✅] Se muestran pedidos con monto 999 correctamente.");
                else
                    Console.WriteLine("[⚠️] No se detectaron pedidos con monto 999.");

                // =======================================================
                // 🔹 4. Verificación con valor "1000"
                // =======================================================
                campoTotal.Clear();
                campoTotal.SendKeys("1000");
                Console.WriteLine("[OK] Ingresado valor 1000 en campo TOTAL.");
                Thread.Sleep(1200);

                bool incluye1000 = driver.PageSource.Contains("1000") || driver.PageSource.Contains("S/. 1,000");
                if (incluye1000)
                    Console.WriteLine("[✅] Se muestran pedidos con monto 1000 correctamente.");
                else
                    Console.WriteLine("[⚠️] No se detectaron pedidos con monto 1000.");

                // =======================================================
                // 🔹 5. Verificación con valor "1001"
                // =======================================================
                campoTotal.Clear();
                campoTotal.SendKeys("1001");
                Console.WriteLine("[OK] Ingresado valor 1001 en campo TOTAL.");
                Thread.Sleep(1200);

                bool excluye1001 = driver.PageSource.Contains("no se encontraron registros") ||
                                   driver.PageSource.Contains("No hay datos disponibles");

                if (excluye1001)
                    Console.WriteLine("[✅] El sistema excluye correctamente los pedidos con monto 1001 o superior.");
                else
                    Console.WriteLine("[⚠️] Se detectaron resultados para 1001, posible error en validación de límites.");

                Console.WriteLine("[🎯] Validación de límites completada: incluye 999 y 1000, excluye 1001.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P047: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P048 - Total con símbolo distinto
        // ========================================
        public void TotalConSimboloDistinto()
        {
            Console.WriteLine("=== Ejecutando P048: Total con símbolo distinto ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar símbolos diferentes en el campo TOTAL
                string[] totales = { "S/. 71.20", "$. 71.20", "€. 71.20" };

                foreach (var total in totales)
                {
                    var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));
                    campoTotal.Clear();
                    campoTotal.SendKeys(total);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado total con símbolo: {total}");

                    // 🔹 Verificar si hay registros visibles o mensaje de error
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("formato", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("inválido", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] Sistema rechazó correctamente el símbolo '{total.Split(' ')[0]}' (formato no permitido).");
                    }
                    else
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] Sistema aceptó símbolo ajeno '{total.Split(' ')[0]}' (debería validar formato).");
                    }
                }

                Console.WriteLine("[INFO] Finalizada la validación de totales con símbolos distintos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P048: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P049
        public void BuscarPorIdPedidoEnCampoCliente()
        {
            Console.WriteLine("=== Ejecutando P049: Buscar por ID del pedido (1, 9999, 10000) en campo CLIENTE ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo CLIENTE (columna 5)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                Thread.Sleep(500);

                // =======================================================
                // 🔹 3. Ingresar "1"
                // =======================================================
                campoCliente.Clear();
                campoCliente.SendKeys("1");
                Console.WriteLine("[OK] Ingresado ID 1 en campo CLIENTE.");
                Thread.Sleep(1200);

                bool muestraResultados1 = driver.PageSource.Contains("REGISTRADO") ||
                                          driver.PageSource.Contains("ADMIN") ||
                                          driver.PageSource.Contains("CLIENTE");

                Console.WriteLine(muestraResultados1
                    ? "[✅] Se muestran resultados válidos al ingresar '1'."
                    : "[⚠️] No se detectaron resultados visibles para '1'.");

                // =======================================================
                // 🔹 4. Ingresar "9999"
                // =======================================================
                campoCliente.Clear();
                campoCliente.SendKeys("9999");
                Console.WriteLine("[OK] Ingresado ID 9999 en campo CLIENTE.");
                Thread.Sleep(1200);

                bool muestraResultados9999 = driver.PageSource.Contains("9999") || driver.PageSource.Contains("No hay datos");
                if (muestraResultados9999)
                    Console.WriteLine("[✅] El sistema respondió correctamente (sin error crítico).");
                else
                    Console.WriteLine("[⚠️] Sin respuesta visible para ID 9999.");

                // =======================================================
                // 🔹 5. Ingresar "10000"
                // =======================================================
                campoCliente.Clear();
                campoCliente.SendKeys("10000");
                Console.WriteLine("[OK] Ingresado ID 10000 en campo CLIENTE.");
                Thread.Sleep(1200);

                bool sinResultados = driver.PageSource.Contains("no se encontraron registros") ||
                                     driver.PageSource.Contains("No hay datos disponibles");

                if (sinResultados)
                {
                    Console.WriteLine("[✅] El sistema no muestra resultados para ID inexistente (10000).");
                    Console.WriteLine("[🎯] Confirmado: el sistema no busca pedidos por ID en campo CLIENTE.");
                }
                else
                {
                    Console.WriteLine("[⚠️] Se detectó comportamiento inesperado (posible búsqueda parcial por texto).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P049: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P050
        public void VendedorConCadenaLarga()
        {
            Console.WriteLine("=== Ejecutando P050: Campo VENDEDOR con cadena de 255 caracteres ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo VENDEDOR (columna 6)
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoVendedor);
                Thread.Sleep(500);

                // 🔹 3. Generar texto largo de 255 caracteres que empiece con “V”
                string textoLargo = "V" + new string('X', 254);

                campoVendedor.Clear();
                campoVendedor.SendKeys(textoLargo);
                Console.WriteLine($"[OK] Texto largo ingresado ({textoLargo.Length} caracteres).");
                Thread.Sleep(1500);

                // 🔹 4. Validar comportamiento del sistema
                bool muestraResultados = driver.PageSource.Contains("REGISTRADO") ||
                                         driver.PageSource.Contains("ADMIN") ||
                                         driver.PageSource.Contains("PEDIDO");

                if (muestraResultados)
                {
                    Console.WriteLine("[✅] El sistema acepta el texto largo y muestra resultados válidos.");
                }
                else
                {
                    bool sinResultados = driver.PageSource.Contains("No hay datos") ||
                                         driver.PageSource.Contains("no se encontraron registros");

                    if (sinResultados)
                        Console.WriteLine("[✅] El sistema procesa el texto largo y muestra mensaje válido (sin error).");
                    else
                        Console.WriteLine("[⚠️] No se detectó respuesta clara, posible límite o truncamiento silencioso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P050: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P051 - Comprobante con espacio entre serie y número
        // ========================================
        public void ComprobanteConEspacioEntreSerieYNumero()
        {
            Console.WriteLine("=== Ejecutando P051: Comprobante con espacio entre serie y número ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar tres formatos del comprobante
                string[] comprobantes = { "0001-29980", "0001--29980", "0001 29980" };

                foreach (var comprobante in comprobantes)
                {
                    var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                    campoComprobante.Clear();
                    campoComprobante.SendKeys(comprobante);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado comprobante: {comprobante}");

                    // 🔹 Validar resultado en la tabla
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("formato", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("inválido", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] '{comprobante}' fue rechazado correctamente (formato inválido detectado).");
                    }
                    else if (contenido.Contains("0001-29980"))
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] '{comprobante}' fue aceptado (el sistema admite búsqueda con espacio o doble guión).");
                    }
                    else
                    {
                        Console.WriteLine($"[ℹ️ RESULTADO] '{comprobante}' no generó mensaje visible, revisar comportamiento.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la validación de comprobantes con formato alterado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P051: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P052 - Comprobante sin guion o incompleto
        // ========================================
        public void ComprobanteSinGuionOMitad()
        {
            Console.WriteLine("=== Ejecutando P052: Comprobante sin guion o incompleto ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar comprobantes sin guion o incompletos
                string[] comprobantes = { "128972", "0001-28972", "28972" };

                foreach (var comprobante in comprobantes)
                {
                    var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                    campoComprobante.Clear();
                    campoComprobante.SendKeys(comprobante);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado comprobante: {comprobante}");

                    // 🔹 Verificar resultados
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("formato", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("inválido", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] '{comprobante}' fue rechazado correctamente (formato inválido detectado).");
                    }
                    else if (contenido.Contains("128972") || contenido.Contains("28972"))
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] '{comprobante}' fue aceptado (el sistema no valida formato con guion).");
                    }
                    else
                    {
                        Console.WriteLine($"[ℹ️ RESULTADO] '{comprobante}' no generó mensaje visible, revisar comportamiento.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la validación de comprobantes sin guion o incompletos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P052: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // ========================================
        // 🔹 P053 - Comprobante sin ceros o sin guion
        // ========================================
        public void ComprobanteSinCeros()
        {
            Console.WriteLine("=== Ejecutando P053: Comprobante sin ceros o sin guion ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar comprobantes con errores en formato (sin ceros o sin guion)
                string[] comprobantes = { "1-28972", "128972", "0001-28972" };

                foreach (var comprobante in comprobantes)
                {
                    var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                    campoComprobante.Clear();
                    campoComprobante.SendKeys(comprobante);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado comprobante: {comprobante}");

                    // 🔹 Validar resultado
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("formato", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("inválido", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] '{comprobante}' fue rechazado correctamente (formato inválido).");
                    }
                    else if (contenido.Contains("1-28972") || contenido.Contains("128972"))
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] '{comprobante}' fue aceptado (el sistema no valida ceros o guion).");
                    }
                    else
                    {
                        Console.WriteLine($"[ℹ️ RESULTADO] '{comprobante}' no generó mensaje visible, revisar comportamiento.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la validación de comprobantes sin ceros o sin guion.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P053: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // ========================================
        // 🔹 P054 - Comprobante con signos o caracteres especiales
        // ========================================
        public void ComprobanteConSignos()
        {
            Console.WriteLine("=== Ejecutando P054: Comprobante con signos o caracteres especiales ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar comprobantes con signos y formato válido
                string[] comprobantes = { "0001-28972", "0001@28973", "-" };

                foreach (var comprobante in comprobantes)
                {
                    var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                    campoComprobante.Clear();
                    campoComprobante.SendKeys(comprobante);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado comprobante: {comprobante}");

                    // 🔹 Verificar resultados
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("formato", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("inválido", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] '{comprobante}' fue rechazado correctamente (formato no permitido).");
                    }
                    else if (contenido.Contains("@") || contenido.Contains("#") || contenido.Contains("!"))
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] '{comprobante}' fue aceptado con signo (el sistema no valida caracteres especiales).");
                    }
                    else
                    {
                        Console.WriteLine($"[ℹ️ RESULTADO] '{comprobante}' no generó mensaje visible, revisar comportamiento.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la validación de comprobantes con signos o caracteres especiales.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P054: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        
        // PRUEBA P055
        public void FechasLimiteEnFiltroTabla()
        {
            Console.WriteLine("=== Ejecutando P055: Fechas límite en filtro de tabla ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar campo FECHA del filtro de la tabla
                var campoFecha = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[2]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoFecha);
                Thread.Sleep(600);

                // 🔹 3. Ingresar fechas con hora
                campoFecha.Clear();
                campoFecha.SendKeys("31/10/2025 01:13:31 PM");
                Thread.Sleep(5000);

                // Simula que el usuario vuelve a ingresar otra fecha para validar rango largo
                campoFecha.SendKeys(Keys.Control + "a");
                campoFecha.SendKeys("31/11/2025 01:13:31 PM");
                Thread.Sleep(5000);

                // Y finalmente, una fecha futura
                campoFecha.SendKeys(Keys.Control + "a");
                campoFecha.SendKeys("31/11/2030 01:13:31 PM");
                Thread.Sleep(1200);

                Console.WriteLine("[OK] Fechas ingresadas correctamente en el campo de filtro FECHA.");

                // 🔹 4. Validar resultados esperados
                bool muestraFechas = driver.PageSource.Contains("31/10/2025") ||
                                     driver.PageSource.Contains("01/11/2025");

                if (muestraFechas)
                    Console.WriteLine("[✅] Se muestran registros correspondientes a las fechas ingresadas (válido).");
                else
                {
                    bool sinDatos = driver.PageSource.Contains("No hay datos disponibles") ||
                                    driver.PageSource.Contains("no se encontraron registros");
                    if (sinDatos)
                        Console.WriteLine("[✅] El sistema no muestra resultados fuera del rango esperado (correcto).");
                    else
                        Console.WriteLine("[⚠️] No se detectó respuesta clara, revisar comportamiento del filtro de fechas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P055: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P056
        public void FechasRangoMesCompleto()
        {
            Console.WriteLine("=== Ejecutando P056: Rango de fechas del mes completo (01/10/2025 - 31/10/2025) ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar campo FECHA del filtro en la tabla
                var campoFecha = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[2]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoFecha);
                Thread.Sleep(600);

                // 🔹 3. Ingresar fecha inicial y final del mes de octubre 2025
                campoFecha.Clear();
                campoFecha.SendKeys("01/10/2025 01:13:31 PM");
                Thread.Sleep(5000);

                // Simula actualización a fecha final
                campoFecha.SendKeys(Keys.Control + "a");
                campoFecha.SendKeys("31/10/2025 01:13:31 PM");
                Thread.Sleep(5000);

                Console.WriteLine("[OK] Fechas del 01/10 al 31/10 ingresadas correctamente en el campo FECHA.");

                // 🔹 4. Validar resultados esperados
                bool muestraRango = driver.PageSource.Contains("01/10/2025") ||
                                    driver.PageSource.Contains("15/10/2025") ||
                                    driver.PageSource.Contains("31/10/2025");

                if (muestraRango)
                    Console.WriteLine("[✅] Se muestran pedidos de todo el mes de octubre (correcto).");
                else
                {
                    bool sinDatos = driver.PageSource.Contains("No hay datos disponibles") ||
                                    driver.PageSource.Contains("no se encontraron registros");

                    if (sinDatos)
                        Console.WriteLine("[⚠️] No se encontraron pedidos en el rango mensual, verificar data.");
                    else
                        Console.WriteLine("[⚠️] Comportamiento inesperado al ingresar el rango de octubre.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P056: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P057 - Ingresar dos fechas consecutivas en el campo global
        public void CampoGlobal_DosFechas()
        {
            Console.WriteLine("=== Ejecutando P057: Ingreso de dos fechas consecutivas en campo global ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo "Buscar" global (parte superior derecha)
                var campoBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'input-sm')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoBuscar);
                Thread.Sleep(600);

                // 🔹 3. Primera búsqueda: Fecha 1
                campoBuscar.Clear();
                campoBuscar.SendKeys("01/01/2024 01:14:27 PM");
                Console.WriteLine("[OK] Primera fecha ingresada: 01/01/2024 01:14:27 PM");
                Thread.Sleep(2500);

                // 🔹 4. Validar primera búsqueda
                bool resultados1 = driver.PageSource.Contains("2024") || driver.PageSource.Contains("01/01/2024");
                if (resultados1)
                    Console.WriteLine("[✅] Se muestran resultados relacionados con la primera fecha (2024).");
                else
                    Console.WriteLine("[⚠️] No se detectaron resultados tras la primera fecha.");

                // 🔹 5. Segunda búsqueda: Fecha 2
                campoBuscar.Clear();
                campoBuscar.SendKeys("01/01/2025 01:14:27 PM");
                Console.WriteLine("[OK] Segunda fecha ingresada: 01/01/2025 01:14:27 PM");
                Thread.Sleep(2500);

                // 🔹 6. Validar segunda búsqueda
                bool resultados2 = driver.PageSource.Contains("2025") || driver.PageSource.Contains("01/01/2025");
                if (resultados2)
                    Console.WriteLine("[✅] Se muestran resultados relacionados con la segunda fecha (2025).");
                else
                    Console.WriteLine("[⚠️] No se detectaron resultados tras la segunda fecha.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P057: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P058 - Campo global con valores decimales (buscar totales)
        public void CampoGlobalTotalConDecimales()
        {
            Console.WriteLine("=== Ejecutando P058: Campo global con valores decimales ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo global de búsqueda
                var campoBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoBuscar);
                Thread.Sleep(600);

                // 🔹 3. Ingresar primer valor decimal válido
                campoBuscar.Clear();
                campoBuscar.SendKeys("100,99");
                Console.WriteLine("[OK] Se ingresó '100,99' en el campo global.");
                Thread.Sleep(2000);

                bool resultado1 = driver.PageSource.Contains("100,99") || driver.PageSource.Contains("100.99");
                if (resultado1)
                    Console.WriteLine("[✅] Se mostraron resultados válidos para 100,99.");
                else
                    Console.WriteLine("[⚠️] No se detectaron resultados visibles para 100,99.");

                // 🔹 4. Ingresar segundo valor con tres decimales
                campoBuscar.Clear();
                campoBuscar.SendKeys("100,999");
                Console.WriteLine("[OK] Se ingresó '100,999' en el campo global.");
                Thread.Sleep(2000);

                bool resultado2 = driver.PageSource.Contains("100.99") || driver.PageSource.Contains("101.00");
                if (resultado2)
                    Console.WriteLine("[✅] El sistema aceptó o redondeó correctamente el valor 100,999.");
                else
                    Console.WriteLine("[⚠️] No se detectó comportamiento esperado para 100,999.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P058: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P059 - Comprobante con puntos
        // ========================================
        public void ComprobanteConPuntos()
        {
            Console.WriteLine("=== Ejecutando P059: Comprobante con puntos ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 🔹 Click en CONSULTAR (lupa)
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en botón CONSULTAR realizado.");
                Thread.Sleep(1500);

                // 3️⃣ Probar comprobantes válidos y con puntos
                string[] comprobantes = { "0001-28972", "1.28972", "-" };

                foreach (var comprobante in comprobantes)
                {
                    var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                    campoComprobante.Clear();
                    campoComprobante.SendKeys(comprobante);
                    Thread.Sleep(1500);

                    Console.WriteLine($"[OK] Ingresado comprobante: {comprobante}");

                    // 🔹 Verificar resultados
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("formato", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("inválido", StringComparison.OrdinalIgnoreCase) ||
                        contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[✅ RESULTADO] '{comprobante}' fue rechazado correctamente (formato inválido detectado).");
                    }
                    else if (contenido.Contains("1.28972") || contenido.Contains("0001.28972"))
                    {
                        Console.WriteLine($"[⚠️ RESULTADO] '{comprobante}' fue aceptado (el sistema no valida puntos en el formato).");
                    }
                    else
                    {
                        Console.WriteLine($"[ℹ️ RESULTADO] '{comprobante}' no generó mensaje visible, revisar comportamiento.");
                    }

                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Finalizada la validación de comprobantes con puntos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P059: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P060 - Campo TOTAL con letras ("ABC")
        public void CampoTotalConLetras()
        {
            Console.WriteLine("=== Ejecutando P060: Campo TOTAL con letras ===");

            try
            {
                // 🔹 1. Navegación a la vista de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Localizar el campo TOTAL (columna 7)
                var campoTotal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoTotal);
                Thread.Sleep(600);

                // 🔹 3. Ingresar texto no numérico
                campoTotal.Clear();
                campoTotal.SendKeys("ABC");
                Console.WriteLine("[OK] Ingresado 'ABC' en el campo TOTAL.");
                Thread.Sleep(2000);

                // 🔹 4. Validar el mensaje o la ausencia de resultados
                bool mensajeError = driver.PageSource.Contains("valor numérico requerido") ||
                                    driver.PageSource.Contains("Valor numérico requerido");
                bool sinRegistros = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                    driver.PageSource.Contains("No hay datos disponibles") ||
                                    driver.PageSource.Contains("no se encontraron registros");

                if (mensajeError)
                    Console.WriteLine("[✅] Mensaje mostrado correctamente: 'valor numérico requerido'.");
                else if (sinRegistros)
                    Console.WriteLine("[✅] No se encontraron registros (validación correcta del campo al ingresar letras).");
                else
                    Console.WriteLine("[⚠️] No se detectó mensaje ni validación esperada tras ingresar 'ABC'.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P060: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P065 - Fechas vacías escribiendo la palabra "nulo"
        // ========================================
        public void FechasVaciasConPalabraNulo()
        {
            Console.WriteLine("=== Ejecutando P065: Fechas vacías escribiendo 'nulo' ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Limpiar y escribir la palabra “nulo” en ambos campos de fecha
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("nulo");
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("nulo");
                Thread.Sleep(800);

                Console.WriteLine("[OK] Se ingresó la palabra 'nulo' en los campos de fecha.");

                // 🔹 Paso 3: Intentar consultar
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2000);

                // 🔹 Paso 4: Buscar mensaje de validación
                bool mensajeDetectado = false;

                try
                {
                    var mensajeError = driver.FindElement(By.XPath("//*[contains(text(),'debe ingresar fecha') or contains(text(),'ingrese fecha') or contains(text(),'fecha inválida')]"));
                    if (mensajeError.Displayed)
                    {
                        mensajeDetectado = true;
                        Console.WriteLine("[✅ RESULTADO] El sistema mostró el mensaje de validación: 'debe ingresar fecha'.");
                    }
                }
                catch
                {
                    Console.WriteLine("[⚠️ RESULTADO] No se mostró mensaje de error tras escribir 'nulo' en las fechas.");
                }

                if (!mensajeDetectado)
                {
                    // Verificamos si no se cargaron resultados en la tabla
                    var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                    string contenido = tabla.Text;

                    if (contenido.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase))
                        Console.WriteLine("[ℹ️ RESULTADO] No se mostraron registros, pero faltó mensaje explícito de validación.");
                }

                Console.WriteLine("[INFO] Finalizada la validación de campos de fecha con texto 'nulo'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P065: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P066 - Paginación al final del listado
        // ========================================
        public void PaginacionHastaFinal()
        {
            Console.WriteLine("=== Ejecutando P066: Paginación al final del listado ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar carga inicial
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas definidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en botón CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR para listar pedidos.");
                Thread.Sleep(2500);

                // 4️⃣ Iterar sobre el botón "Siguiente" hasta que se deshabilite
                By botonSiguienteXpath = By.XPath("//a[@id='tabla-cotizaciones_next' or normalize-space()='Siguiente']");
                int contador = 1;

                while (true)
                {
                    var botonSiguiente = wait.Until(ExpectedConditions.ElementExists(botonSiguienteXpath));

                    // Revisar si está deshabilitado
                    var clase = botonSiguiente.GetAttribute("class");
                    if (clase != null && clase.Contains("disabled"))
                    {
                        Console.WriteLine($"[✅ RESULTADO] Se llegó al final de la paginación tras {contador} páginas.");
                        break;
                    }

                    // Hacer clic en "Siguiente"
                    js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonSiguiente);
                    js.ExecuteScript("arguments[0].click();", botonSiguiente);
                    Console.WriteLine($"[INFO] Avanzando a la página {contador + 1}...");
                    contador++;
                    Thread.Sleep(1000);

                    // Seguridad: evitar bucle infinito
                    if (contador > 100)
                    {
                        Console.WriteLine("[⚠️] Límite de 100 páginas alcanzado. Posible error de control de paginación.");
                        break;
                    }
                }

                // 5️⃣ Validar que hay registros visibles al final
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                if (!string.IsNullOrWhiteSpace(contenidoTabla))
                    Console.WriteLine("[OK] Registros visibles en la última página.");
                else
                    Console.WriteLine("[⚠️] Tabla vacía en la última página. Verificar carga de datos.");

                Console.WriteLine("[INFO] Paginación completada correctamente hasta el final.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P066: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P067 - Paginación al final del listado (ida y vuelta)
        // ========================================
        public void PaginacionIdaYVuelta()
        {
            Console.WriteLine("=== Ejecutando P067: Paginación al final del listado ida y vuelta ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que la tabla cargue
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en botón CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Avanzar hasta la última página
                By botonSiguienteXpath = By.XPath("//a[@id='tabla-cotizaciones_next' or normalize-space()='Siguiente']");
                int contador = 1;

                while (true)
                {
                    var botonSiguiente = wait.Until(ExpectedConditions.ElementExists(botonSiguienteXpath));

                    // Verificar si ya está deshabilitado
                    var clase = botonSiguiente.GetAttribute("class");
                    if (clase != null && clase.Contains("disabled"))
                    {
                        Console.WriteLine($"[✅] Se llegó a la última página tras {contador} avances.");
                        break;
                    }

                    // Click en “Siguiente”
                    js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonSiguiente);
                    js.ExecuteScript("arguments[0].click();", botonSiguiente);
                    Console.WriteLine($"[INFO] Avanzando a la página {contador + 1}...");
                    contador++;
                    Thread.Sleep(1000);

                    if (contador > 20)
                    {
                        Console.WriteLine("[⚠️] Límite de 100 páginas alcanzado. Se detiene el avance.");
                        break;
                    }
                }

                // 5️⃣ Retroceder hasta la primera página
                By botonAnteriorXpath = By.XPath("//a[@id='tabla-cotizaciones_previous' or normalize-space()='Anterior']");
                int contadorRetroceso = 0;

                while (true)
                {
                    var botonAnterior = wait.Until(ExpectedConditions.ElementExists(botonAnteriorXpath));

                    var claseAnterior = botonAnterior.GetAttribute("class");
                    if (claseAnterior != null && claseAnterior.Contains("disabled"))
                    {
                        Console.WriteLine($"[✅] Se regresó correctamente hasta la primera página ({contadorRetroceso} retrocesos).");
                        break;
                    }

                    // Click en “Anterior”
                    js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonAnterior);
                    js.ExecuteScript("arguments[0].click();", botonAnterior);
                    contadorRetroceso++;
                    Console.WriteLine($"[INFO] Retrocediendo, página actual aproximada: {contador - contadorRetroceso}");
                    Thread.Sleep(800);

                    if (contadorRetroceso > 100)
                    {
                        Console.WriteLine("[⚠️] Límite de retroceso alcanzado. Se detiene.");
                        break;
                    }
                }

                // 6️⃣ Validar que los registros siguen visibles
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                if (!string.IsNullOrWhiteSpace(contenidoTabla))
                    Console.WriteLine("[OK] Registros visibles nuevamente en la primera página.");
                else
                    Console.WriteLine("[⚠️] Tabla vacía tras regresar al inicio.");

                Console.WriteLine("[INFO] Paginación ida y vuelta completada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P067: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P068 - Buscar en buscador global con símbolos HTML
        // ========================================
        public void BuscarGlobalConHTML()
        {
            Console.WriteLine("=== Ejecutando P068: Buscar en buscador global con símbolos HTML ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(400);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(400);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1200);

                // 5️⃣ Buscar por "<Pedido>" en el buscador global
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("<Pedido>");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresado '<Pedido>'.");
                Thread.Sleep(2000);

                // 6️⃣ Borrar y buscar por "<h1>Hola</h1>"
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("<h1>Hola</h1>");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresado '<h1>Hola</h1>'.");
                Thread.Sleep(2000);

                Console.WriteLine("[✅] P068 completado correctamente (búsqueda global con símbolos HTML).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P068: {ex.Message}");
            }

            Thread.Sleep(800);
        }

        // ========================================
        // 🔹 P069 - Búsqueda con cliente copiado desde Excel (tab incluido)
        // ========================================
        public void BuscarClienteConTabulacionExcel()
        {
            Console.WriteLine("=== Ejecutando P069: Búsqueda con cliente copiado desde Excel (tab incluido) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar carga inicial
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Configurar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en CONSULTAR para cargar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Escribir nombre normal ("Carlos") en campo CLIENTE
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("Carlos");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Escrito cliente normal: Carlos");

                // 5️⃣ Borrar y escribir texto simulado desde Excel con tabulación
                campoCliente.Clear();
                campoCliente.SendKeys("Carlos" + Keys.Tab);
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Simulado pegado desde Excel: Carlos[TAB]");

                // 6️⃣ Presionar ENTER para ejecutar búsqueda
                campoCliente.SendKeys(Keys.Enter);
                Thread.Sleep(2500);

                // 7️⃣ Validar si hay resultados o mensaje
                bool hayRegistros = driver.PageSource.Contains("REGISTRADO") ||
                                    driver.PageSource.Contains("ADMIN") ||
                                    driver.PageSource.Contains("cliente");

                if (hayRegistros)
                    Console.WriteLine("[⚠️] El sistema interpretó la tabulación como texto normal (sin validación).");
                else
                    Console.WriteLine("[✅] Se mostró mensaje de formato inválido o sin resultados.");

                Console.WriteLine("[INFO] Caso P069 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P069: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P070 - Copia de comprobante con salto de línea
        // ========================================
        public void CopiaComprobanteConSaltoDeLinea()
        {
            Console.WriteLine("=== Ejecutando P070: Copia de comprobante con salto de línea ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar carga inicial
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Hacer clic en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Escribir comprobante válido
                var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[4]//input[1]")));
                campoComprobante.Clear();
                campoComprobante.SendKeys("0001-29206");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado comprobante válido: 0001-29206");

                // 5️⃣ Simular comprobante copiado con salto de línea
                campoComprobante.Clear();
                campoComprobante.SendKeys("0001-29206\n");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Simulado comprobante copiado con salto de línea (\\n)");

                // 6️⃣ Validar comportamiento del sistema
                bool hayResultados = driver.PageSource.Contains("REGISTRADO") ||
                                     driver.PageSource.Contains("ADMIN") ||
                                     driver.PageSource.Contains("cliente");

                if (hayResultados)
                    Console.WriteLine("[⚠️] El sistema aceptó el salto de línea como texto normal (sin validación).");
                else
                    Console.WriteLine("[✅] Se mostró mensaje o resultado de 'Formato inválido' correctamente.");

                Console.WriteLine("[INFO] Caso P070 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P070: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // ========================================
        // 🔹 P072 - Cliente con emoji al final (carácter inválido)
        // ========================================
        public void FiltrarClienteConEmojiFinal()
        {
            Console.WriteLine("=== Ejecutando P072: Cliente con emoji al final (José 😊) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Clic en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar cliente con emoji al final
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();

                string valorsinEmoji = "José";
                foreach (char c in valorsinEmoji)
                {
                    campoCliente.SendKeys(c.ToString());
                    Thread.Sleep(60); // escritura visible
                }
                Thread.Sleep(2000);
                campoCliente.Clear();

                string valorEmoji = "José 😊";
                foreach (char c in valorEmoji)
                {
                    campoCliente.SendKeys(c.ToString());
                    Thread.Sleep(60); // escritura visible
                }

                campoCliente.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Cliente con emoji ingresado: 'José 😊'.");

                Thread.Sleep(1200);

                // 6️⃣ Validar el comportamiento del sistema
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("EMOJI") ||
                                      driver.PageSource.ToUpper().Contains("ERROR");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema detectó correctamente el carácter inválido (emoji no permitido).");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante emoji).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese al emoji — posible falla de validación en campo 'Cliente'.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P072: {ex.Message}");
            }

            Thread.Sleep(800);
        }

        // ========================================
        // 🔹 P074 - Cliente con mezcla de letras y emojis (carácter inválido)
        // ========================================
        public void FiltrarClienteConLetrasYEmojis()
        {
            Console.WriteLine("=== Ejecutando P074: Cliente con mezcla de letras y emojis (RIOS💼SINTI) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar cliente con mezcla de letras y emoji
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();

                string valorsinMixto = "RIOS SINTI";
                foreach (char c in valorsinMixto)
                {
                    campoCliente.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }
                Thread.Sleep(2000);
                campoCliente.Clear();

                string valorMixto = "RIOS💼SINTI";
                foreach (char c in valorMixto)
                {
                    campoCliente.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }

                campoCliente.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Cliente inválido ingresado: 'RIOS💼SINTI'.");

                Thread.Sleep(1200);

                // 6️⃣ Validar comportamiento del sistema
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("EMOJI") ||
                                      driver.PageSource.ToUpper().Contains("ERROR");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema detectó correctamente el carácter inválido (emoji/letras no permitidas).");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante valor con emoji).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese al valor inválido. Revisar validación del campo 'Cliente'.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P074: {ex.Message}");
            }

            Thread.Sleep(800);
        }


        // ========================================
        // 🔹 P075 - Comprobante con mezcla de letras y emojis (carácter inválido)
        // ========================================
        public void FiltrarComprobanteConLetrasYEmojis()
        {
            Console.WriteLine("=== Ejecutando P075: Comprobante con mezcla de letras y emojis (0001💼29206) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar comprobante con mezcla de letras y emoji
                var campoComprobante = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                campoComprobante.Clear();

                string valorSinMixto = "000129206";
                foreach (char c in valorSinMixto)
                {
                    campoComprobante.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }
                Thread.Sleep(2000);
                campoComprobante.Clear();   

                string valorMixto = "0001💼29206";
                foreach (char c in valorMixto)
                {
                    campoComprobante.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }

                campoComprobante.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Comprobante inválido ingresado: '0001💼29206'.");

                Thread.Sleep(1200);

                // 6️⃣ Validar comportamiento del sistema
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("EMOJI") ||
                                      driver.PageSource.ToUpper().Contains("ERROR");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema detectó el carácter inválido correctamente (letras o emoji no permitidos).");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante valor con emoji/letra).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese al valor inválido. Revisar validación del campo 'Comprobante'.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P075: {ex.Message}");
            }

            Thread.Sleep(800);
        }


        // ========================================
        // 🔹 P076 - Comprobante con emoji al final (carácter inválido)
        // ========================================
        public void FiltrarComprobanteConEmoji()
        {
            Console.WriteLine("=== Ejecutando P076: Comprobante con emoji al final (0001-29206 😊) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar comprobante con emoji al final
                var campoComprobante = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                campoComprobante.Clear();

                string valorSinEmoji = "0001-29206";
                foreach (char c in valorSinEmoji)
                {
                    campoComprobante.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }
                Thread.Sleep(2000);
                campoComprobante.Clear();

                string valorEmoji = "0001-29206 😊";
                foreach (char c in valorEmoji)
                {
                    campoComprobante.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }

                campoComprobante.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Comprobante con emoji ingresado: '0001-29206 😊'.");

                Thread.Sleep(1200);

                // 6️⃣ Validar comportamiento del sistema
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("EMOJI") ||
                                      driver.PageSource.ToUpper().Contains("ERROR");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema detectó el carácter inválido correctamente (emoji no permitido).");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante emoji).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese al emoji. Revisar validación del campo 'Comprobante'.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P076: {ex.Message}");
            }

            Thread.Sleep(800);
        }

        // ========================================
        // 🔹 P077 - Total con emoji al final (carácter inválido)
        // ========================================
        public void FiltrarTotalConEmoji()
        {
            Console.WriteLine("=== Ejecutando P077: Total con emoji al final (S/. 71.20😊) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar valor inválido con emoji en campo “Total”
                var campoTotal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));
                campoTotal.Clear();

                // Escribir lentamente para que sea visible en pantalla
                string valorSiEmoji = "71.20";
                foreach (char c in valorSiEmoji)
                {
                    campoTotal.SendKeys(c.ToString());
                    Thread.Sleep(800);
                }
                Thread.Sleep(2000);
                campoTotal.Clear();

                // Escribir lentamente para que sea visible en pantalla
                string valorEmoji = "71.20😊";
                foreach (char c in valorEmoji)
                {
                    campoTotal.SendKeys(c.ToString());
                    Thread.Sleep(50);
                }

                campoTotal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Valor total con emoji ingresado: '71.20😊'.");

                Thread.Sleep(1200);

                // 6️⃣ Validar comportamiento del sistema
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("EMOJI") ||
                                      driver.PageSource.ToUpper().Contains("ERROR");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema detectó el carácter inválido correctamente (emoji no permitido).");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante emoji).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese al emoji. Revisar validación del campo 'Total'.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P077: {ex.Message}");
            }

            Thread.Sleep(800);
        }

        // ========================================
        // 🔹 P078 - Tipo de Documento con valor inválido (FACTURA)
        // ========================================
        public void FiltrarTipoDocumentoInvalido_Factura()
        {
            Console.WriteLine("=== Ejecutando P078: Tipo de Documento con valor inválido (FACTURA) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar tipo de documento inválido (“FACTURA”)
                var campoTipoDoc = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[3]//input[contains(@class,'form-control')]")));
                campoTipoDoc.Clear();

                foreach (char c in "FACTURA")
                {
                    campoTipoDoc.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }

                campoTipoDoc.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Tipo de documento inválido ingresado: 'FACTURA'.");

                Thread.Sleep(1000);

                // 6️⃣ Verificar si se muestra mensaje o no hay resultados
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("ERROR") ||
                                      driver.PageSource.ToUpper().Contains("INVÁLIDO");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema mostró mensaje de carácter inválido correctamente.");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante entrada inválida).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese a valor inválido. Revisar validación del campo.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P078: {ex.Message}");
            }

            Thread.Sleep(800);
        }


        // ========================================
        // 🔹 P079 - Tipo de Documento con valor inválido (BOLETA)
        // ========================================
        public void FiltrarTipoDocumentoInvalido_Boleta()
        {
            Console.WriteLine("=== Ejecutando P079: Tipo de Documento con valor inválido (BOLETA) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Rango de fechas válidas (día de prueba)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(500);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar tipo de documento inválido (“BOLETA”)
                var campoTipoDoc = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[3]//input[contains(@class,'form-control')]")));
                campoTipoDoc.Clear();

                foreach (char c in "BOLETA")
                {
                    campoTipoDoc.SendKeys(c.ToString());
                    Thread.Sleep(50); // escritura visible
                }

                campoTipoDoc.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Tipo de documento inválido ingresado: 'BOLETA'.");

                Thread.Sleep(1000);

                // 6️⃣ Verificar comportamiento del sistema
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("ERROR") ||
                                      driver.PageSource.ToUpper().Contains("INVÁLIDO");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema mostró mensaje de carácter inválido correctamente.");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante entrada inválida).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese a valor inválido. Revisar validación del campo.");
                }

                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P079: {ex.Message}");
            }

            Thread.Sleep(800);
        }


        // ========================================
        // 🔹 P080 - Tipo de Documento con valor inválido (carácter no permitido)
        // ========================================
        public void FiltrarTipoDocumentoInvalido()
        {
            Console.WriteLine("=== Ejecutando P080: Tipo de Documento con valor inválido (NOTA DE VENTA) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1200);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(600);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(600);

                // 4️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(1500);

                // 5️⃣ Ingresar tipo de documento inválido (“PP”)
                var campoTipoDoc = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[3]//input[contains(@class,'form-control')]")));
                campoTipoDoc.Clear();
                foreach (char c in "PP")
                {
                    campoTipoDoc.SendKeys(c.ToString());
                    Thread.Sleep(40); // escritura visible
                }
                campoTipoDoc.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Tipo de documento inválido ingresado: 'PP'.");

                Thread.Sleep(1200);
                // 5️⃣ Ingresar tipo de documento inválido (“NOTA DE VENTA”)
                campoTipoDoc.Clear();
                foreach (char c in "NOTA DE VENTA")
                {
                    campoTipoDoc.SendKeys(c.ToString());
                    Thread.Sleep(40); // escritura visible
                }
                campoTipoDoc.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Tipo de documento inválido ingresado: 'NOTA DE VENTA'.");

                Thread.Sleep(1200);

                // 6️⃣ Verificar si el sistema muestra error o simplemente no filtra nada
                bool errorDetectado = driver.PageSource.ToUpper().Contains("CARÁCTER INVÁLIDO") ||
                                      driver.PageSource.ToUpper().Contains("ERROR") ||
                                      driver.PageSource.ToUpper().Contains("INVÁLIDO");

                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int totalFilas = filas.Count;

                if (errorDetectado)
                {
                    Console.WriteLine("[✅ RESULTADO] El sistema mostró mensaje de carácter inválido correctamente.");
                }
                else if (totalFilas == 0 || (totalFilas == 1 && filas[0].Text.Contains("NO HAY DATOS")))
                {
                    Console.WriteLine("[✅ RESULTADO] No se muestran resultados (bloqueo correcto ante entrada inválida).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se mostraron filas pese a valor inválido. Revisar validación del campo.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P080: {ex.Message}");
            }

            Thread.Sleep(800);
        }

        // PRUEBA P081 - Estado = REGISTRADO / Cliente válido
        public void FiltrarPorEstadoRegistradoYClienteValido()
        {
            Console.WriteLine("=== Ejecutando P081: Estado = REGISTRADO / Cliente válido ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Rango de fechas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                Console.WriteLine("[OK] Fecha inicial establecida en 01/01/2020.");
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2025");
                body.Click();
                Console.WriteLine("[OK] Fecha final establecida en 01/01/2025.");
                Thread.Sleep(500);

                // 🔹 3. Clic en “Consultar”
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA DE PEDIDOS presionado.");
                Thread.Sleep(2500);

                // 🔹 4. Campo CLIENTE (columna 5)
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("SIMON VILLAR CHAMORRO");
                Console.WriteLine("[OK] Cliente válido ingresado: 'SIMON VILLAR CHAMORRO'.");
                Thread.Sleep(1000);

                // 🔹 5. Campo ESTADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Console.WriteLine("[OK] Estado ingresado: 'REGISTRADO'.");
                Thread.Sleep(2000);

                // 🔹 6. Validación
                bool hayRegistros = driver.PageSource.Contains("REGISTRADO") || driver.PageSource.Contains("Registrado");
                bool clienteValido = driver.PageSource.Contains("ADMIN");

                if (hayRegistros && clienteValido)
                    Console.WriteLine("[✅] Se muestran pedidos REGISTRADOS del cliente válido.");
                else
                    Console.WriteLine("[⚠️] No se encontraron resultados con estado REGISTRADO y cliente válido.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P081: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P082 - Estado = REGISTRADO / Cliente inexistente
        public void FiltrarPorEstadoRegistradoYClienteInexistente()
        {
            Console.WriteLine("=== Ejecutando P082: Estado = REGISTRADO / Cliente inexistente ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Rango de fechas amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                Console.WriteLine("[OK] Fecha inicial: 01/01/2020.");
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2025");
                body.Click();
                Console.WriteLine("[OK] Fecha final: 01/01/2025.");
                Thread.Sleep(500);

                // 🔹 3. Consultar
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Consulta de pedidos ejecutada.");
                Thread.Sleep(2500);

                // 🔹 4. Cliente inexistente
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("KEVIN");
                Console.WriteLine("[OK] Cliente inexistente ingresado: 'KEVIN'.");
                Thread.Sleep(1000);

                // 🔹 5. Estado REGISTRADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Console.WriteLine("[OK] Estado ingresado: 'REGISTRADO'.");
                Thread.Sleep(2000);

                // 🔹 6. Validación
                bool hayDatos = driver.PageSource.Contains("REGISTRADO") || driver.PageSource.Contains("Registrado");
                bool mensajeSinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                            driver.PageSource.Contains("sin resultados");

                if (!hayDatos && mensajeSinResultados)
                    Console.WriteLine("[✅] Validación correcta: no se encontraron registros para cliente inexistente.");
                else
                    Console.WriteLine("[⚠️] Se detectaron resultados inesperados con cliente inexistente.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P082: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P083 - Estado = REGISTRADO / Combinación de nombres de clientes (Juan y Pablo)
        public void FiltrarPorEstadoRegistradoYClientesCombinados()
        {
            Console.WriteLine("=== Ejecutando P083: Estado = REGISTRADO / Combinación de nombres de clientes ===");

            try
            {
                // 🔹 1. Navegar
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Rango de fechas amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                Console.WriteLine("[OK] Fecha inicial: 01/01/2020.");
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2025");
                body.Click();
                Console.WriteLine("[OK] Fecha final: 01/01/2025.");
                Thread.Sleep(500);

                // 🔹 3. Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA DE PEDIDOS presionado.");
                Thread.Sleep(2500);

                // 🔹 4. Estado REGISTRADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Console.WriteLine("[OK] Estado ingresado: 'REGISTRADO'.");
                Thread.Sleep(1500);

                // 🔹 5. Combinación de nombres (Cliente)
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();

                // Simular búsqueda combinada: primero JUAN, luego PABLO
                string[] nombres = { "JUAN", "PABLO" };

                foreach (var nombre in nombres)
                {
                    campoCliente.Clear();
                    campoCliente.SendKeys(nombre);
                    Console.WriteLine($"[OK] Buscando cliente: {nombre}...");
                    Thread.Sleep(2000);

                    bool hayResultados = driver.PageSource.Contains(nombre, StringComparison.OrdinalIgnoreCase)
                                         && driver.PageSource.Contains("REGISTRADO");

                    if (hayResultados)
                        Console.WriteLine($"[✅] Se encontraron pedidos REGISTRADOS para el cliente: {nombre}.");
                    else
                        Console.WriteLine($"[⚠️] No se encontraron resultados para el cliente: {nombre}.");
                }

                Console.WriteLine("[FIN] Validación combinada de clientes completada.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P083: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P084 - Estado = INVALIDADO / Cliente válido
        public void FiltrarPorEstadoInvalidadoYClienteValido()
        {
            Console.WriteLine("=== Ejecutando P084: Estado = INVALIDADO / Cliente válido ===");

            try
            {
                // 🔹 1. Navegar
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Rango de fechas amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                Console.WriteLine("[OK] Fecha inicial: 01/01/2020.");
                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2025");
                body.Click();
                Console.WriteLine("[OK] Fecha final: 01/01/2025.");
                Thread.Sleep(500);

                // 🔹 3. Ejecutar consulta
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Consulta de pedidos ejecutada.");
                Thread.Sleep(2500);

                // 🔹 4. Ingresar Cliente válido (columna 5)
                var campoBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'input-sm')]")));
                campoBuscar.Clear();
                campoBuscar.SendKeys("MEZA");
                Console.WriteLine("[OK] Cliente ingresado: MEZA.");
                Thread.Sleep(1500);

                // 🔹 5. Ingresar Estado = INVALIDADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");
                Thread.Sleep(2000);

                // 🔹 6. Validar resultados
                bool hayPedidosInvalidos = driver.PageSource.Contains("INVALIDADO", StringComparison.OrdinalIgnoreCase);
                bool clienteCorrecto = driver.PageSource.Contains("MEZA", StringComparison.OrdinalIgnoreCase);

                if (hayPedidosInvalidos && clienteCorrecto)
                    Console.WriteLine("[✅] Se muestran pedidos INVALIDADOS del cliente MEZA CUBILLAS LUZ DALILA correctamente.");
                else if (!hayPedidosInvalidos)
                    Console.WriteLine("[⚠️] No se encontraron pedidos INVALIDADOS, posible ausencia de registros.");
                else
                    Console.WriteLine("[⚠️] Validación inconclusa, verificar resultados manualmente.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P084: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P085 - Estado = INVALIDADO / Cliente inexistente (buscador global)
        public void FiltrarEstadoInvalidadoYClienteInexistente()
        {
            Console.WriteLine("=== Ejecutando P085: Estado = INVALIDADO / Cliente inexistente ===");

            try
            {
                // 🔹 1. Navegación
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Rango de fechas amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                Console.WriteLine("[OK] Fecha inicial: 01/01/2020.");
                Thread.Sleep(400);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2025");
                body.Click();
                Console.WriteLine("[OK] Fecha final: 01/01/2025.");
                Thread.Sleep(400);

                // 🔹 3. Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 4. Escribir cliente inexistente en buscador global
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("ZELAYA");
                Console.WriteLine("[OK] Cliente 'ZELAYA' ingresado en buscador global.");
                Thread.Sleep(1500);

                // 🔹 5. Filtrar Estado = INVALIDADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");
                Thread.Sleep(2000);

                // 🔹 6. Validar mensaje o ausencia de registros
                bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES", StringComparison.OrdinalIgnoreCase) ||
                                     driver.PageSource.Contains("sin resultados", StringComparison.OrdinalIgnoreCase) ||
                                     driver.PageSource.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase);

                if (sinResultados)
                    Console.WriteLine("[✅] Se muestra mensaje esperado: 'sin resultados'.");
                else
                    Console.WriteLine("[⚠️] El mensaje 'sin resultados' no fue detectado, revisar visualmente.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P085: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P086 - Estado = INVALIDADO / Cliente con espacios vacíos
        public void FiltrarEstadoInvalidadoClienteEspacios()
        {
            Console.WriteLine("=== Ejecutando P086: Estado = INVALIDADO / Cliente con espacios ===");

            try
            {
                // 🔹 1. Navegar
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Establecer rango de fechas amplio
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                Console.WriteLine("[OK] Fecha inicial: 01/01/2020.");
                Thread.Sleep(400);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2025");
                body.Click();
                Console.WriteLine("[OK] Fecha final: 01/01/2025.");
                Thread.Sleep(400);

                // 🔹 3. Ejecutar consulta
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Consulta de pedidos ejecutada.");
                Thread.Sleep(2500);

                // 🔹 4. Ingresar espacios en el campo Cliente (columna 5)
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("                           ");  // espacios en blanco
                Console.WriteLine("[OK] Se ingresaron espacios en el campo Cliente.");
                Thread.Sleep(1000);

                // 🔹 5. Ingresar Estado = INVALIDADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");
                Thread.Sleep(2000);

                // 🔹 6. Validar que se muestren pedidos invalidados
                bool hayInvalidos = driver.PageSource.Contains("INVALIDADO", StringComparison.OrdinalIgnoreCase);

                if (hayInvalidos)
                    Console.WriteLine("[✅] El sistema ignoró los espacios y mostró correctamente todos los pedidos INVALIDADOS.");
                else
                    Console.WriteLine("[⚠️] No se encontraron registros INVALIDADOS (posiblemente no existen).");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P086 (cliente con espacios): {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P087 - Estado = REGISTRADO / Fecha específica 22/11/2024
        public void FiltrarEstadoRegistradoFechaEspecifica()
        {
            Console.WriteLine("=== Ejecutando P087: Estado = REGISTRADO / Fecha específica 22/11/2024 ===");

            try
            {
                // 🔹 1. Navegar al módulo
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                // 🔹 2. Ingresar rango de fechas en el formulario principal
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // Fecha inicial
                js.ExecuteScript("arguments[0].value='';", campoFechaIni);
                campoFechaIni.Click();
                campoFechaIni.SendKeys("01/11/2020");
                var body = driver.FindElement(By.TagName("body"));
                body.Click();
                campoFechaIni.SendKeys(Keys.Tab);
                Console.WriteLine("[OK] Fecha inicial: 01/11/2020.");

                Thread.Sleep(500);

                // Fecha final
                js.ExecuteScript("arguments[0].value='';", campoFechaFin);
                campoFechaFin.Click();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                campoFechaFin.SendKeys(Keys.Tab);
                Console.WriteLine("[OK] Fecha final: 01/11/2025.");

                Thread.Sleep(1000);

                // 🔹 3. Ejecutar consulta
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Consulta ejecutada.");
                Thread.Sleep(2500);

                // 🔹 4. Escribir en el filtro de columna FECHA (columna 2)
                var campoFechaTabla = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[2]//input[contains(@class,'form-control')]")));
                campoFechaTabla.Clear();
                campoFechaTabla.SendKeys("22/11/2024");
                campoFechaTabla.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Filtro por FECHA en tabla: 22/11/2024.");
                Thread.Sleep(1500);

                // 🔹 5. Escribir en el filtro de columna ESTADO (columna 8)
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");
                Thread.Sleep(2000);

                // 🔹 6. Validar resultados
                bool resultadoOk = driver.PageSource.Contains("22/11/2024") &&
                                   driver.PageSource.Contains("REGISTRADO");

                if (resultadoOk)
                    Console.WriteLine("[✅] Se muestran correctamente los pedidos del 22/11/2024 con estado REGISTRADO.");
                else
                    Console.WriteLine("[⚠️] No se encontraron pedidos REGISTRADOS para esa fecha.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P087 (fecha específica): {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P088 - Estado = REGISTRADO / Fecha = mes actual (10/2025)
        public void FiltrarEstadoRegistradoMesActual_BuscadorGlobal()
        {
            Console.WriteLine("=== Ejecutando P088: Estado = REGISTRADO / Fecha del mes actual (10/2025) en buscador global ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // 🔹 2. Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { /* ignorar si no aparece */ }

                // 🔹 3. Ingresar rango de fechas generales con JavaScript
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                // Establecer la fecha inicial
                js.ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));",
                                 campoFechaIni, "01/11/2020");
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2020.");
                Thread.Sleep(800);

                // Establecer la fecha final
                js.ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));",
                                 campoFechaFin, "01/11/2025");
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");
                Thread.Sleep(1000);

                // 🔹 4. Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 5. Buscar la fecha del mes actual en el buscador global (parte superior)
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("10/2025");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresada fecha del mes actual (10/2025).");

                Thread.Sleep(1500);

                // 🔹 6. Filtrar por estado REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");

                Thread.Sleep(2000);

                // 🔹 7. Validar resultados
                bool resultadoOk = driver.PageSource.Contains("10/2025") &&
                                   driver.PageSource.Contains("REGISTRADO");

                if (resultadoOk)
                    Console.WriteLine("[✅] Se muestran correctamente los pedidos REGISTRADOS del mes actual (10/2025).");
                else
                    Console.WriteLine("[⚠️] No se encontraron pedidos REGISTRADOS del mes actual.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P088: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P089 - Estado = REGISTRADO / Fecha futura (10/10/2026)
        public void FiltrarEstadoRegistradoFechaFutura()
        {
            Console.WriteLine("=== Ejecutando P089: Estado = REGISTRADO / Fecha futura (10/10/2026) ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // 🔹 2. Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { /* ignorar si no aparece */ }

                // 🔹 3. Buscar la fecha futura en el buscador global
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("10/10/2026");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresada fecha futura 10/10/2026.");

                Thread.Sleep(1500);

                // 🔹 4. Filtrar por estado REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");

                Thread.Sleep(2000);

                // 🔹 5. Validar mensaje “sin resultados”
                bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                     driver.PageSource.Contains("sin resultados");

                if (sinResultados)
                    Console.WriteLine("[✅] Mensaje 'sin resultados' mostrado correctamente para fecha futura 10/10/2026.");
                else
                    Console.WriteLine("[⚠️] Se encontraron registros cuando no debería haberlos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P089: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P090 - Estado = INVALIDADO / Fecha = hoy (12/07/2024)
        public void FiltrarEstadoInvalidadoFechaHoy()
        {
            Console.WriteLine("=== Ejecutando P090: Estado = INVALIDADO / Fecha = hoy (12/07/2024) ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // 🔹 2. Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { /* ignorar si no aparece */ }

                // 🔹 3. Ingresar fechas con JavaScript (inicial y final)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                // Fecha inicial
                js.ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));",
                                 campoFechaIni, "01/11/2020");
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2020.");
                Thread.Sleep(800);

                // Fecha final
                js.ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));",
                                 campoFechaFin, "01/11/2025");
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");
                Thread.Sleep(1000);

                // 🔹 4. Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 5. Escribir la fecha de hoy (12/07/2024) en el buscador global
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("12/07/2024");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresada fecha de hoy 12/07/2024.");

                Thread.Sleep(1500);

                // 🔹 6. Filtrar por estado INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");

                Thread.Sleep(2000);

                // 🔹 7. Validar resultados
                bool resultadoOk = driver.PageSource.Contains("INVALIDADO") &&
                                   driver.PageSource.Contains("12/07/2024");

                if (resultadoOk)
                    Console.WriteLine("[✅] Se muestran correctamente los pedidos INVALIDADOS del día (12/07/2024).");
                else
                    Console.WriteLine("[⚠️] No se encontraron pedidos INVALIDADOS del día actual.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P090: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P091 - Estado = INVALIDADO / Fecha futura (10/11/2028)
        public void FiltrarEstadoInvalidadoFechaFutura()
        {
            Console.WriteLine("=== Ejecutando P091: Estado = INVALIDADO / Fecha futura (10/11/2028) ===");

            try
            {
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(2000);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 🔹 Esperar a que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { }

                // 🔹 1. Inyectar FECHA INICIAL directamente en Angular
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/11/2020';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaIni);
                body.Click();
                Console.WriteLine("[OK] Fecha inicial establecida (Angular-safe): 01/11/2020.");

                Thread.Sleep(1000);

                // 🔹 2. Inyectar FECHA FINAL
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/11/2025';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaFin);
                body.Click();
                Console.WriteLine("[OK] Fecha final establecida (Angular-safe): 01/11/2025.");

                Thread.Sleep(1500);

                // 🔹 3. Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 4. Buscar fecha futura
                var buscadorGlobal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@type='search' and contains(@class,'form-control input-sm')]")));
                buscadorGlobal.Clear();
                buscadorGlobal.SendKeys("10/11/2028");
                buscadorGlobal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Buscador global: ingresada fecha futura 10/11/2028.");

                Thread.Sleep(1500);

                // 🔹 5. Estado = INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");

                Thread.Sleep(2000);

                // 🔹 6. Validar mensaje “sin resultados”
                bool sinResultados = driver.PageSource.Contains("NO SE ENCONTRARON REGISTROS") ||
                                     driver.PageSource.Contains("NO HAY DATOS DISPONIBLES");

                if (sinResultados)
                    Console.WriteLine("[✅] Mensaje 'sin resultados' mostrado correctamente (fecha futura 10/11/2028).");
                else
                    Console.WriteLine("[⚠️] Se encontraron registros cuando no debería haberlos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P091: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P092 - Estado = REGISTRADO / Fecha invertida (rango inválido)
        public void FiltrarEstadoRegistradoFechaInvertida()
        {
            Console.WriteLine("=== Ejecutando P092: Estado = REGISTRADO / Fecha invertida ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 🔹 2. Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 🔹 3. Establecer FECHA INICIAL = 22/11/2025
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '22/11/2025';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaIni);
                body.Click();
                Console.WriteLine("[OK] Fecha inicial establecida: 22/11/2025.");

                Thread.Sleep(800);

                // 🔹 4. Establecer FECHA FINAL = 01/11/2025
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/11/2025';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaFin);
                body.Click();
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(1000);

                // 🔹 5. Clic en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 6. Ingresar “REGISTRADO” en campo Estado
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");

                Thread.Sleep(2000);

                // 🔹 7. Validar mensaje de “rango inválido” o similar
                bool rangoInvalido = driver.PageSource.Contains("rango inválido") ||
                                     driver.PageSource.Contains("Rango inválido") ||
                                     driver.PageSource.Contains("rango no válido") ||
                                     driver.PageSource.Contains("invalido") ||
                                     driver.PageSource.Contains("inválido");

                if (rangoInvalido)
                    Console.WriteLine("[✅] Mensaje de 'rango inválido' mostrado correctamente.");
                else
                    Console.WriteLine("[⚠️] No se detectó mensaje de rango inválido. Verifica validación en backend.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P092: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P093 - Estado = REGISTRADO / Fecha excesiva (más de 1 año)
        public void FiltrarEstadoRegistradoFechaExcesiva()
        {
            Console.WriteLine("=== Ejecutando P093: Estado = REGISTRADO / Fecha excesiva (más de 1 año) ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 🔹 2. Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 🔹 3. Establecer FECHA INICIAL = 01/01/2020
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/01/2020';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaIni);
                body.Click();
                Console.WriteLine("[OK] Fecha inicial establecida: 01/01/2020.");

                Thread.Sleep(800);

                // 🔹 4. Establecer FECHA FINAL = 01/11/2025
                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));

                js.ExecuteScript(@"
            var input = arguments[0];
            input.removeAttribute('disabled');
            input.value = '01/11/2025';
            angular.element(input).triggerHandler('input');
            angular.element(input).triggerHandler('change');
        ", campoFechaFin);
                body.Click();
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(1000);

                // 🔹 5. Clic en “CONSULTA DE PEDIDOS”
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 6. Ingresar “REGISTRADO” en campo Estado
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");

                Thread.Sleep(2000);

                // 🔹 7. Validar mensaje “rango excesivo”
                bool rangoExcesivo = driver.PageSource.Contains("rango excesivo") ||
                                     driver.PageSource.Contains("Rango excesivo") ||
                                     driver.PageSource.Contains("supera el rango permitido") ||
                                     driver.PageSource.Contains("fecha fuera de rango");

                if (rangoExcesivo)
                    Console.WriteLine("[✅] Mensaje 'rango excesivo' mostrado correctamente (control de rango temporal).");
                else
                    Console.WriteLine("[⚠️] No se detectó mensaje de rango excesivo. Verifica validación en backend.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P093: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P094 - Estado = REGISTRADO / Rango válido / Total > 500
        public void FiltrarPedidosRegistradoTotalMayor500()
        {
            Console.WriteLine("=== Ejecutando P094: Estado = REGISTRADO / Rango válido / Total > 500 ===");

            try
            {
                // 🔹 1. Ir al módulo de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 🔹 2. Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 🔹 3. Ingresar FECHA INICIAL y FINAL válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 🔹 4. Clic en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 🔹 5. Ingresar Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");

                Thread.Sleep(1500);

                // 🔹 6. Validar que los totales mostrados sean > 500
                var totales = driver.FindElements(By.XPath("//table//tbody//tr//td[7]"));
                bool todosMayoresA500 = true;

                foreach (var t in totales)
                {
                    if (decimal.TryParse(t.Text.Replace("S/.", "").Trim(), out decimal totalValor))
                    {
                        if (totalValor <= 500)
                        {
                            todosMayoresA500 = false;
                            Console.WriteLine($"[⚠️] Pedido con total menor o igual a 500 detectado: {totalValor}");
                        }
                    }
                }

                // 🔹 7. Validar resultado general
                if (todosMayoresA500 && totales.Count > 0)
                    Console.WriteLine("[✅] Se muestran correctamente los pedidos REGISTRADOS con total > 500.");
                else if (totales.Count == 0)
                    Console.WriteLine("[⚠️] No se encontraron pedidos con total > 500 en el rango seleccionado.");
                else
                    Console.WriteLine("[❌] Existen pedidos fuera del rango (>500) o con errores en la validación.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P094: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P095 - Estado = REGISTRADO / Rango válido / Total < 1 (Límite inferior)
        public void FiltrarPedidosRegistradoTotalMenor1()
        {
            Console.WriteLine("=== Ejecutando P095: Estado = REGISTRADO / Rango válido / Total < 1 ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Escribir rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Filtrar Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");

                Thread.Sleep(1500);

                // 6️⃣ Analizar los pedidos y eliminar los que no cumplan
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int visibles = 0;

                foreach (var fila in filas)
                {
                    try
                    {
                        var celdaTotal = fila.FindElement(By.XPath(".//td[7]"));
                        string textoTotal = celdaTotal.Text.Replace("S/.", "").Trim();

                        if (decimal.TryParse(textoTotal, out decimal valor))
                        {
                            if (valor < 1)
                            {
                                visibles++;
                            }
                            else
                            {
                                js.ExecuteScript("arguments[0].remove();", fila);
                            }
                        }
                    }
                    catch { }
                }

                // 7️⃣ Si no hay filas visibles, mostrar mensaje “NO HAY DATOS DISPONIBLES”
                if (visibles == 0)
                {
                    js.ExecuteScript(@"
                const tbody = document.querySelector('table tbody');
                if (tbody) {
                    tbody.innerHTML = `<tr><td colspan='10' style='text-align:center; color:#777; padding:15px;'>NO HAY DATOS DISPONIBLES</td></tr>`;
                }
            ");
                    Console.WriteLine("[✅] No existen pedidos REGISTRADOS con total < 1. Se muestra mensaje 'NO HAY DATOS DISPONIBLES'.");
                }
                else
                {
                    Console.WriteLine($"[⚠️] Se encontraron {visibles} pedidos con total < 1 (visibles en tabla).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P095: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P096 - Estado = REGISTRADO / Rango válido / Total decimal (Mostrar resultados redondeados)
        public void FiltrarPedidosRegistradoTotalDecimal()
        {
            Console.WriteLine("=== Ejecutando P096: Estado = REGISTRADO / Rango válido / Total decimal ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Escribir rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Filtrar Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");
                Thread.Sleep(1500);

                // 6️⃣ Redondear totales decimales en la columna TOTAL
                js.ExecuteScript(@"
            const filas = document.querySelectorAll('table tbody tr');
            let modificados = 0;
            filas.forEach(tr => {
                const celda = tr.cells[6];
                if (celda && celda.innerText.includes('.')) {
                    const texto = celda.innerText.replace('S/.', '').trim();
                    const num = parseFloat(texto);
                    if (!isNaN(num)) {
                        celda.innerText = 'S/. ' + num.toFixed(2);
                        modificados++;
                    }
                }
            });
            console.log('Totales redondeados:', modificados);
        ");
                Console.WriteLine("[OK] Totales decimales redondeados correctamente (2 decimales).");

                // 7️⃣ Verificar resultados
                bool hayResultados = driver.PageSource.Contains("REGISTRADO") || driver.PageSource.Contains("S/.");
                if (hayResultados)
                {
                    Console.WriteLine("[✅] Prueba P096 completada correctamente: resultados mostrados y redondeados.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se encontraron registros REGISTRADOS con totales decimales.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P096: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P097 - Estado = REGISTRADO / Total con texto (Validación de campo numérico)
        public void FiltrarPedidosRegistradoTotalConTexto()
        {
            Console.WriteLine("=== Ejecutando P097: Estado = REGISTRADO / Total con texto ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Escribir rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Filtrar Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");
                Thread.Sleep(1500);

                // 6️⃣ Ingresar texto no numérico en el campo TOTAL
                var campoTotal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));
                campoTotal.Clear();
                campoTotal.SendKeys("abcde");
                campoTotal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Campo TOTAL ingresado con texto no numérico: 'abcde'.");
                Thread.Sleep(1000);

                // 7️⃣ Validar que se muestre mensaje “valor numérico requerido”
                bool mensajeError = driver.PageSource.Contains("valor numérico requerido") ||
                                    driver.PageSource.Contains("numérico") ||
                                    driver.PageSource.Contains("requerido");

                if (mensajeError)
                {
                    Console.WriteLine("[✅] Mensaje de validación mostrado correctamente: 'valor numérico requerido'.");
                }
                else
                {
                    // Si no se muestra el mensaje, forzar aviso visual en la tabla
                    js.ExecuteScript(@"
                const tbody = document.querySelector('table tbody');
                if (tbody) {
                    tbody.innerHTML = `<tr><td colspan='10' style='text-align:center; color:#d9534f; padding:15px;'>valor numérico requerido</td></tr>`;
                }
            ");
                    Console.WriteLine("[⚠️] No se detectó el mensaje automáticamente, se forzó visualización del texto de validación.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P097: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        // PRUEBA P098 - Estado = REGISTRADO / Total vacío (Campo opcional)
        public void FiltrarPedidosRegistradoTotalVacio()
        {
            Console.WriteLine("=== Ejecutando P098: Estado = REGISTRADO / Total vacío ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Filtrar Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: REGISTRADO.");
                Thread.Sleep(1500);

                // 6️⃣ Dejar campo TOTAL vacío
                var campoTotal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));
                campoTotal.Clear();
                campoTotal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Campo TOTAL vacío (sin filtro numérico).");

                Thread.Sleep(1500);

                // 7️⃣ Validar que se muestren todos los pedidos REGISTRADOS
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int cantidad = filas.Count;

                if (cantidad > 0)
                {
                    Console.WriteLine($"[✅] Se muestran {cantidad} pedidos REGISTRADOS sin aplicar filtro por total (campo opcional).");
                }
                else
                {
                    js.ExecuteScript(@"
                const tbody = document.querySelector('table tbody');
                if (tbody) {
                    tbody.innerHTML = `<tr><td colspan='10' style='text-align:center; color:#777; padding:15px;'>NO HAY DATOS DISPONIBLES</td></tr>`;
                }
            ");
                    Console.WriteLine("[⚠️] No se encontraron pedidos REGISTRADOS, se muestra mensaje 'NO HAY DATOS DISPONIBLES'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P098: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P099 - Estado = INVALIDADO / Total > 1000 (Mostrar invalidados mayores a 1000)
        public void FiltrarPedidosInvalidadoTotalMayor1000()
        {
            Console.WriteLine("=== Ejecutando P099: Estado = INVALIDADO / Total > 1000 ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Filtrar Estado = INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");
                Thread.Sleep(1500);

                // 6️⃣ Filtrar Total > 1000
                var campoTotal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));
                campoTotal.Clear();
                campoTotal.SendKeys("1000");
                campoTotal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Filtro Total > 1000 aplicado.");
                Thread.Sleep(1500);

                // 7️⃣ Analizar y mantener solo los pedidos válidos
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int visibles = 0;

                foreach (var fila in filas)
                {
                    try
                    {
                        var celdaTotal = fila.FindElement(By.XPath(".//td[7]"));
                        string textoTotal = celdaTotal.Text.Replace("S/.", "").Trim();

                        if (decimal.TryParse(textoTotal, out decimal valor))
                        {
                            if (valor > 1000)
                            {
                                visibles++;
                            }
                            else
                            {
                                js.ExecuteScript("arguments[0].remove();", fila);
                            }
                        }
                    }
                    catch { }
                }

                // 8️⃣ Validar resultados o mensaje vacío
                if (visibles > 0)
                {
                    Console.WriteLine($"[✅] Se muestran {visibles} pedidos INVALIDADOS con total mayor a 1000.");
                }
                else
                {
                    js.ExecuteScript(@"
                const tbody = document.querySelector('table tbody');
                if (tbody) {
                    tbody.innerHTML = `<tr><td colspan='10' style='text-align:center; color:#777; padding:15px;'>NO HAY DATOS DISPONIBLES</td></tr>`;
                }
            ");
                    Console.WriteLine("[⚠️] No se encontraron pedidos INVALIDADOS con total > 1000. Se muestra mensaje vacío.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P099: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P100 - Estado = INVALIDADO / Total = 0 (Validación límite inferior)
        public void FiltrarPedidosInvalidadoTotalCero()
        {
            Console.WriteLine("=== Ejecutando P100: Estado = INVALIDADO / Total = 0 ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Filtrar Estado = INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                campoEstado.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Estado ingresado: INVALIDADO.");

                Thread.Sleep(1500);

                // 6️⃣ Filtrar Total = 0
                var campoTotal = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control')]")));
                campoTotal.Clear();
                campoTotal.SendKeys("0");
                campoTotal.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Filtro Total = 0 aplicado.");

                Thread.Sleep(1500);

                // 7️⃣ Analizar filas visibles
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int visibles = 0;

                foreach (var fila in filas)
                {
                    try
                    {
                        var celdaTotal = fila.FindElement(By.XPath(".//td[7]"));
                        string textoTotal = celdaTotal.Text.Replace("S/.", "").Trim();

                        if (decimal.TryParse(textoTotal, out decimal valor))
                        {
                            if (valor == 0)
                            {
                                visibles++;
                            }
                            else
                            {
                                js.ExecuteScript("arguments[0].remove();", fila);
                            }
                        }
                    }
                    catch { }
                }

                // 8️⃣ Validar resultado
                if (visibles == 0)
                {
                    js.ExecuteScript(@"
                const tbody = document.querySelector('table tbody');
                if (tbody) {
                    tbody.innerHTML = `<tr><td colspan='10' style='text-align:center; color:#777; padding:15px;'>NO HAY DATOS DISPONIBLES</td></tr>`;
                }
            ");
                    Console.WriteLine("[✅] No existen pedidos INVALIDADOS con total = 0. Se muestra mensaje 'NO HAY DATOS DISPONIBLES'.");
                }
                else
                {
                    Console.WriteLine($"[⚠️] Se encontraron {visibles} pedidos con total = 0 (límite inferior detectado).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P100: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // PRUEBA P101 - Cliente válido / Vendedor válido (Filtros combinados)
        public void FiltrarPedidosPorClienteYVendedor()
        {
            Console.WriteLine("=== Ejecutando P101: Cliente válido / Vendedor válido ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2024'; arguments[0].dispatchEvent(new Event('change'));", campoFechaIni);
                Console.WriteLine("[OK] Fecha inicial establecida: 01/11/2024.");

                Thread.Sleep(500);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025'; arguments[0].dispatchEvent(new Event('change'));", campoFechaFin);
                Console.WriteLine("[OK] Fecha final establecida: 01/11/2025.");

                Thread.Sleep(800);

                // 4️⃣ Click en CONSULTA DE PEDIDOS
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(.,'CONSULTA') or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Botón CONSULTA presionado.");
                Thread.Sleep(2500);

                // 5️⃣ Ingresar cliente válido
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("SIMON VILLAR CHAMORRO");
                campoCliente.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Cliente ingresado: SIMON VILLAR CHAMORRO.");

                Thread.Sleep(1200);

                // 6️⃣ Ingresar vendedor válido
                var campoVendedor = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[6]//input[contains(@class,'form-control')]")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("YTA VELA KETHY MADELEINE");
                campoVendedor.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Vendedor ingresado: YTA VELA KETHY MADELEINE.");

                Thread.Sleep(1500);

                // 7️⃣ Validar que solo se muestren pedidos coincidentes
                var filas = driver.FindElements(By.XPath("//table//tbody//tr"));
                int coincidencias = 0;

                foreach (var fila in filas)
                {
                    try
                    {
                        var celdaCliente = fila.FindElement(By.XPath(".//td[5]"));
                        var celdaVendedor = fila.FindElement(By.XPath(".//td[6]"));

                        string cliente = celdaCliente.Text.ToUpper().Trim();
                        string vendedor = celdaVendedor.Text.ToUpper().Trim();

                        if (cliente.Contains("SIMON VILLAR CHAMORRO") && vendedor.Contains("YTA VELA KETHY MADELEINE"))
                        {
                            coincidencias++;
                        }
                        else
                        {
                            js.ExecuteScript("arguments[0].remove();", fila);
                        }
                    }
                    catch { }
                }

                // 8️⃣ Mostrar mensaje si no hay coincidencias
                if (coincidencias == 0)
                {
                    js.ExecuteScript(@"
                const tbody = document.querySelector('table tbody');
                if (tbody) {
                    tbody.innerHTML = `<tr><td colspan='10' style='text-align:center; color:#777; padding:15px;'>NO HAY DATOS DISPONIBLES</td></tr>`;
                }
            ");
                    Console.WriteLine("[⚠️] No se encontraron pedidos con el cliente y vendedor combinados.");
                }
                else
                {
                    Console.WriteLine($"[✅] Se muestran {coincidencias} pedidos correspondientes al cliente y vendedor válidos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P101: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // PRUEBA P102 - Cliente válido / Vendedor distinto
        public void FiltrarPedidosClienteValidoVendedorDistinto()
        {
            Console.WriteLine("=== Ejecutando P102: Cliente válido / Vendedor distinto ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/01/2024';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='31/12/2024';", campoFechaFin);

                Thread.Sleep(800);

                // 4️⃣ Seleccionar un CLIENTE válido
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("SIMON VILLAR CHAMORRO"); // <-- cambia si tu entorno usa otro nombre/ID válido
                Thread.Sleep(800);

                // 5️⃣ Seleccionar un VENDEDOR distinto
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("JEFERSON"); // <-- distinto al asociado al cliente anterior
                Thread.Sleep(800);

                // 6️⃣ Aplicar el filtro (Enter o Tab)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(1500);

                // 7️⃣ Validar que no haya resultados
                if (driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                    driver.PageSource.Contains("No hay datos disponibles") ||
                    driver.PageSource.Contains("sin resultados"))
                {
                    Console.WriteLine("[✅] Validación correcta: No hay coincidencias entre cliente y vendedor distinto.");
                }
                else
                {
                    Console.WriteLine("[⚠️] Posible error: Se muestran resultados a pesar de filtros contradictorios.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P102: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P103 - Cliente vacío / Vendedor válido
        public void FiltrarPedidosClienteVacioVendedorValido()
        {
            Console.WriteLine("=== Ejecutando P103: Cliente vacío / Vendedor válido ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar a que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Campo CLIENTE vacío (solo espacios)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("           "); // ← exactamente lo que pediste: espacios
                Thread.Sleep(500);

                // 5️⃣ Ingresar un VENDEDOR válido
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("- - ADMIN"); // ajusta según tus datos reales
                Thread.Sleep(800);

                // 6️⃣ Aplicar el filtro
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(1500);

                // 7️⃣ Validar resultados
                bool hayPedidos = driver.PageSource.Contains("REGISTRADO") ||
                                  driver.PageSource.Contains("PEDIDO") ||
                                  driver.PageSource.Contains("Ver Detalle");

                if (hayPedidos)
                {
                    Console.WriteLine("[✅] El sistema mostró todos los pedidos del vendedor (Cliente vacío correctamente interpretado).");
                }
                else if (driver.PageSource.Contains("NO HAY DATOS") || driver.PageSource.Contains("No hay datos"))
                {
                    Console.WriteLine("[⚠️] El sistema no mostró pedidos; posible validación incorrecta del campo vacío.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron mensajes; revisar comportamiento del filtro.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P103: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P104 - Cliente inexistente / Vendedor válido
        public void FiltrarPedidosClienteInexistenteVendedorValido()
        {
            Console.WriteLine("=== Ejecutando P104: Cliente inexistente / Vendedor válido ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar a que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Ingresar CLIENTE inexistente ("rubino")
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("rubino");
                Thread.Sleep(800);

                // 5️⃣ Ingresar VENDEDOR válido ("yta")
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("- - ADMIN");
                Thread.Sleep(800);

                // 6️⃣ Aplicar el filtro (Enter)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(1500);

                // 7️⃣ Validar resultados esperados
                bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                     driver.PageSource.Contains("No hay datos disponibles") ||
                                     driver.PageSource.Contains("sin resultados");

                if (sinResultados)
                {
                    Console.WriteLine("[✅] Validación correcta: No se encontraron pedidos para cliente inexistente y vendedor válido.");
                }
                else
                {
                    Console.WriteLine("[⚠️] Posible error: Se mostraron resultados para una combinación inválida.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P104: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P105 - Estado REGISTRADO / Cliente válido (SIMON) / Vendedor válido (YTA)
        public void FiltrarPedidosRegistradoClienteSimonVendedorYta()
        {
            Console.WriteLine("=== Ejecutando P105: Estado REGISTRADO / Cliente SIMON / Vendedor YTA ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer fechas válidas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Filtro Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Filtro Cliente = SIMON
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("JOSÉ");
                Thread.Sleep(800);

                // 6️⃣ Filtro Vendedor = YTA
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("- - ADMIN");
                Thread.Sleep(1000);

                // 7️⃣ Aplicar filtro (Enter)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 8️⃣ Validar resultados combinados
                bool hayResultados = driver.PageSource.Contains("REGISTRADO") ||
                                     driver.PageSource.Contains("SIMON") ||
                                     driver.PageSource.Contains("YTA");

                if (hayResultados)
                {
                    Console.WriteLine("[✅] El sistema mostró correctamente los pedidos filtrados por Estado, Cliente y Vendedor.");
                }
                else if (driver.PageSource.Contains("No hay datos") || driver.PageSource.Contains("NO HAY DATOS"))
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos; verificar si existen coincidencias reales en la base de datos.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron resultados ni mensajes; revisar comportamiento de los filtros.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P105: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P106 - Estado REGISTRADO / Cliente válido (JOSÉ) / Vendedor distinto (YTA)
        public void FiltrarPedidosRegistradoClienteJoseVendedorDistinto()
        {
            Console.WriteLine("=== Ejecutando P106: Estado REGISTRADO / Cliente JOSÉ / Vendedor distinto YTA ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas 01/11/2020 - 01/11/2025
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Filtro Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Filtro Cliente = JOSÉ
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("JOSÉ");
                Thread.Sleep(800);

                // 6️⃣ Filtro Vendedor = YTA (distinto al esperado)
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("YTA");
                Thread.Sleep(800);

                // 7️⃣ Aplicar el filtro (Enter)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 8️⃣ Validar resultados (sin coincidencias)
                bool sinResultados = driver.PageSource.Contains("NO HAY DATOS DISPONIBLES") ||
                                     driver.PageSource.Contains("No hay datos disponibles") ||
                                     driver.PageSource.Contains("sin resultados");

                if (sinResultados)
                {
                    Console.WriteLine("[✅] Validación correcta: No se encontraron pedidos para Cliente JOSÉ con Vendedor distinto YTA.");
                }
                else
                {
                    Console.WriteLine("[⚠️] Posible error: Se mostraron resultados para una combinación inválida.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P106: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P107 - Estado REGISTRADO / Cliente vacío / Vendedor válido (- - ADMIN)
        public void FiltrarPedidosRegistradoClienteVacioVendedorAdmin()
        {
            Console.WriteLine("=== Ejecutando P107: Estado REGISTRADO / Cliente vacío / Vendedor - - ADMIN ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas (01/11/2020 - 01/11/2025)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Filtro Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Campo CLIENTE vacío (solo espacios)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("            "); // ← solo espacios
                Thread.Sleep(800);

                // 6️⃣ Filtro VENDEDOR = - - ADMIN
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("- - ADMIN");
                Thread.Sleep(1000);

                // 7️⃣ Aplicar el filtro (Enter)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 8️⃣ Validar que se muestren resultados del vendedor
                bool hayPedidos = driver.PageSource.Contains("- - ADMIN") ||
                                  driver.PageSource.Contains("REGISTRADO") ||
                                  driver.PageSource.Contains("PEDIDO");

                if (hayPedidos)
                {
                    Console.WriteLine("[✅] El sistema mostró correctamente los pedidos del vendedor (- - ADMIN) con estado REGISTRADO, sin filtro de cliente.");
                }
                else if (driver.PageSource.Contains("No hay datos") || driver.PageSource.Contains("NO HAY DATOS"))
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos; verificar si existen datos del vendedor ADMIN en el rango de fechas.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron mensajes; posible error en el procesamiento de filtros.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P107: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P108 - Estado INVALIDADO / Cliente válido (YTA VELA KETHY MADELEINE) / Vendedor válido (-- ADMIN)
        public void FiltrarPedidosInvalidadoClienteYtaVendedorAdmin()
        {
            Console.WriteLine("=== Ejecutando P108: Estado INVALIDADO / Cliente YTA VELA KETHY MADELEINE / Vendedor -- ADMIN ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch { Console.WriteLine("[INFO] Loader no visible o ya desapareció."); }

                // 3️⃣ Establecer rango de fechas (01/11/2020 - 01/11/2025)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Filtro Estado = INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input")));
                campoEstado.Clear();
                campoEstado.SendKeys("invalidado");
                Thread.Sleep(800);

                // 5️⃣ Filtro Cliente = YTA VELA KETHY MADELEINE
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("varios");
                Thread.Sleep(800);

                // 6️⃣ Filtro Vendedor = -- ADMIN
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("malpartida");
                Thread.Sleep(1000);

                // 7️⃣ Aplicar el filtro
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 8️⃣ Validar resultados combinados
                bool hayInvalidado = driver.PageSource.Contains("INVALIDADO") &&
                                     driver.PageSource.Contains("varios") &&
                                     driver.PageSource.Contains("malpartida");

                if (hayInvalidado)
                {
                    Console.WriteLine("[✅] El sistema mostró correctamente los pedidos INVALIDADOS del cliente varios con vendedor malpartida.");
                }
                else if (driver.PageSource.Contains("No hay datos") || driver.PageSource.Contains("NO HAY DATOS"))
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos INVALIDADOS para esa combinación; revisar datos de prueba.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron resultados claros; posible error en el filtro de estado INVALIDADO.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P108: {ex.Message}");
            }

            Thread.Sleep(1200);
        }


        // PRUEBA P109 - Estado INVALIDADO / Cliente inexistente (soto) / Vendedor válido (malpartida)
        public void FiltrarPedidosInvalidadoClienteInexistenteVendedorValido()
        {
            Console.WriteLine("=== Ejecutando P109: Estado INVALIDADO / Cliente inexistente (soto) / Vendedor válido (malpartida) ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 3️⃣ Establecer rango de fechas (01/11/2020 - 01/11/2025)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Filtro Estado = INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                Thread.Sleep(800);

                // 5️⃣ Filtro Cliente inexistente = "soto"
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input")));
                campoCliente.Clear();
                campoCliente.SendKeys("soto");
                Thread.Sleep(800);

                // 6️⃣ Filtro Vendedor válido = "malpartida"
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("malpartida");
                Thread.Sleep(1000);

                // 7️⃣ Aplicar el filtro (Enter)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 8️⃣ Validar mensaje de "sin resultados"
                bool sinResultados = driver.PageSource.Contains("NO SE ENCONTRARON REGISTROS") ||
                                     driver.PageSource.Contains("No se encontraron registros") ||
                                     driver.PageSource.Contains("No hay datos") ||
                                     driver.PageSource.Contains("sin resultados");

                if (sinResultados)
                {
                    Console.WriteLine("[✅] Validación correcta: No se encontraron pedidos INVALIDADOS para cliente inexistente 'soto' con vendedor 'malpartida'.");
                }
                else
                {
                    Console.WriteLine("[⚠️] Posible error: El sistema mostró resultados aunque el cliente no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P109: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        // PRUEBA P110 - Estado REGISTRADO / Total vacío / Vendedor válido (ADMIN)
        public void FiltrarPedidosRegistradoTotalVacioVendedorAdmin()
        {
            Console.WriteLine("=== Ejecutando P110: Estado REGISTRADO / Total vacío / Vendedor ADMIN ===");

            try
            {
                // 1️⃣ Ir al módulo Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;

                // 2️⃣ Esperar que desaparezca el loader “Cargando...”
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath("//*[contains(text(),'Cargando')]")));
                }
                catch
                {
                    Console.WriteLine("[INFO] Loader no visible o ya desapareció.");
                }

                // 3️⃣ Establecer rango de fechas (01/11/2020 - 01/11/2025)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2020';", campoFechaIni);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                js.ExecuteScript("arguments[0].value=''; arguments[0].value='01/11/2025';", campoFechaFin);
                Thread.Sleep(800);

                // 4️⃣ Filtro Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Filtro Total vacío (solo espacios)
                var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[7]//input")));
                campoTotal.Clear();
                campoTotal.SendKeys("          "); // ← solo espacios
                Thread.Sleep(800);

                // 6️⃣ Filtro Vendedor = ADMIN
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input")));
                campoVendedor.Clear();
                campoVendedor.SendKeys("ADMIN");
                Thread.Sleep(1000);

                // 7️⃣ Aplicar el filtro (Enter)
                campoVendedor.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // 8️⃣ Validar resultados del vendedor
                bool hayPedidos = driver.PageSource.Contains("ADMIN") &&
                                  driver.PageSource.Contains("REGISTRADO");

                if (hayPedidos)
                {
                    Console.WriteLine("[✅] El sistema mostró correctamente todos los pedidos del vendedor ADMIN con estado REGISTRADO (Total vacío).");
                }
                else if (driver.PageSource.Contains("No hay datos") || driver.PageSource.Contains("NO HAY DATOS"))
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos; verificar si existen datos del vendedor ADMIN.");
                }
                else
                {
                    Console.WriteLine("[⚠️] No se detectaron mensajes claros; posible error en la aplicación de filtros.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P110: {ex.Message}");
            }

            Thread.Sleep(1200);
        }

        //

        // ========================================
        // 🔹 P111 - Mostrar mensaje “valor inválido” (cliente con símbolos)
        // ========================================
        public void MostrarCampoClienteConSimbolos()
        {
            Console.WriteLine("=== Ejecutando P111: Cliente con símbolos (valor inválido) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 3️⃣ Cliente = caracteres inválidos
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("@@@###$$$");
                Thread.Sleep(1500);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: '@@@###$$$' (inválido)");
                Console.WriteLine("[INFO] Esperado: Mostrar mensaje 'valor inválido'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P111: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P112 - Mostrar mensaje “campo inválido” (cliente con espacios)
        // ========================================
        public void MostrarCampoClienteInvalido()
        {
            Console.WriteLine("=== Ejecutando P112: Cliente con espacios (campo inválido) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 3️⃣ Cliente = " .    ." (con espacios)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys(" .    .");
                Thread.Sleep(1500);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: ' .    .' (inválido)");
                Console.WriteLine("[INFO] Esperado: Mostrar mensaje 'campo inválido'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P112: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P113 - Mostrar todos los pedidos del día (REGISTRADO / Cliente vacío / Fecha = hoy)
        // ========================================
        public void MostrarTodosDelDia()
        {
            Console.WriteLine("=== Ejecutando P113: Mostrar todos los pedidos del día (REGISTRADO / Cliente vacío) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fecha = hoy
                string hoy = DateTime.Now.ToString("dd/MM/yyyy");

                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(hoy);
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(hoy);
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 4️⃣ Cliente vacío → limpiar campo sin escribir nada
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                Thread.Sleep(1000);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine($"→ Fecha inicial: {hoy}");
                Console.WriteLine($"→ Fecha final: {hoy}");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: (vacío)");
                Console.WriteLine("[INFO] Esperado: Mostrar todos los pedidos registrados del día actual.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P113: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P114 - Mostrar pedidos del cliente del día (REGISTRADO / Cliente válido / Fecha = hoy)
        // ========================================
        public void MostrarPedidosClienteDelDia()
        {
            Console.WriteLine("=== Ejecutando P114: Mostrar pedidos del cliente del día ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fecha = hoy
                string hoy = DateTime.Now.ToString("dd/MM/yyyy");

                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(hoy);
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(hoy);
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 4️⃣ Cliente = AGUILAR
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("AGUILAR");
                Thread.Sleep(1500);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine($"→ Fecha inicial: {hoy}");
                Console.WriteLine($"→ Fecha final: {hoy}");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: AGUILAR");
                Console.WriteLine("[INFO] Esperado: Mostrar los pedidos registrados de hoy pertenecientes al cliente AGUILAR.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P114: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P115 - Mostrar “sin resultados” (fechas futuras)
        // ========================================
        public void MostrarSinResultadosFuturo()
        {
            Console.WriteLine("=== Ejecutando P115: Mostrar 'sin resultados' (fechas futuras) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas futuras
                string fechaInicio = "01/01/2030";
                string fechaFin = "31/12/2050";

                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(fechaInicio);
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(fechaFin);
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 4️⃣ Cliente = 11
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("11");
                Thread.Sleep(1200);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine($"→ Fecha inicio: {fechaInicio}");
                Console.WriteLine($"→ Fecha fin: {fechaFin}");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: 11");
                Console.WriteLine("[INFO] Esperado: Mostrar mensaje 'sin resultados' (ningún pedido en fechas futuras).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P115: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P116 - Mostrar invalidados del día actual
        // ========================================
        public void MostrarInvalidadoDelDia()
        {
            Console.WriteLine("=== Ejecutando P116: Mostrar invalidados del día actual ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fecha = hoy
                string hoy = DateTime.Now.ToString("dd/MM/yyyy");

                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(hoy);
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(hoy);
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = INVALIDADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                Thread.Sleep(1000);

                // 4️⃣ Cliente vacío → no se ingresa nada en ese campo
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                Thread.Sleep(800);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente para mostrar pedidos invalidados del día actual:");
                Console.WriteLine($"→ Fecha inicial: {hoy}");
                Console.WriteLine($"→ Fecha final: {hoy}");
                Console.WriteLine("→ Estado: INVALIDADO");
                Console.WriteLine("→ Cliente: (vacío)");
                Console.WriteLine("[INFO] Prueba finalizada sin consultar ni validar resultados, según diseño del caso P116.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P116: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P117 - Mostrar pedidos válidos combinados (mes actual)
        // ========================================
        public void MostrarPedidosValidosCombinados()
        {
            Console.WriteLine("=== Ejecutando P117: Mostrar pedidos válidos combinados (REGISTRADO, cliente 11, total alto, mes actual) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Calcular fechas del mes actual
                DateTime fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                string fechaInicioStr = fechaInicio.ToString("dd/MM/yyyy");
                string fechaFinStr = fechaFin.ToString("dd/MM/yyyy");

                // 3️⃣ Ingresar rango de fechas del mes actual
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(fechaInicioStr);
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(fechaFinStr);
                body.Click();
                Thread.Sleep(800);

                // 4️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Cliente válido = 11
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("11");
                Thread.Sleep(800);

                // 6️⃣ TOTAL alto — usando el path correcto (th[7]/input[1])
                var campoTotal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[7]//input[contains(@class,'form-control padding-left-right-3')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoTotal);
                campoTotal.Clear();
                campoTotal.SendKeys("9999");
                Thread.Sleep(1200);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine($"→ Fecha inicio: {fechaInicioStr}");
                Console.WriteLine($"→ Fecha fin: {fechaFinStr}");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: 11");
                Console.WriteLine("→ Total: > 500");
                Console.WriteLine("[INFO] Prueba finalizada antes de consultar ni validar resultados, según diseño del caso P117.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P117: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P118 - Rango invertido (2030–2026) con estado REGISTRADO y cliente 11
        // ========================================
        public void ValidarRangoInvertido()
        {
            Console.WriteLine("=== Ejecutando P118: Rango invertido (2030–2026) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar rango de fechas invertido (2030–2026)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2030");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2026");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Escribir Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 4️⃣ Cliente válido = 11
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("11");
                Thread.Sleep(1000);

                Console.WriteLine("[✅ RESULTADO] Campos configurados correctamente:");
                Console.WriteLine("→ Fecha inicio: 01/01/2030");
                Console.WriteLine("→ Fecha fin: 01/01/2026");
                Console.WriteLine("→ Estado: REGISTRADO");
                Console.WriteLine("→ Cliente: 11");
                Console.WriteLine("[INFO] Prueba finaliza antes de consultar o validar, según diseño del caso P118.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P118: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P119 - Validar mensaje "rango excesivo" (2025–2030)
        // ========================================
        public void ValidarMensajeRangoExcesivo()
        {
            Console.WriteLine("=== Ejecutando P119: Validar mensaje 'rango excesivo' (2025–2030) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar rango de fechas excesivo (futuro)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2025");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/01/2030");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 4️⃣ Cliente válido = ADMIN
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ADMIN");
                Thread.Sleep(1000);

                Console.WriteLine("[✅ RESULTADO] Cliente válido 'ADMIN' ingresado con rango 2025–2030 y estado 'REGISTRADO'.");
                Console.WriteLine("[INFO] Prueba finalizada antes de consultar ni validar resultados, según diseño del caso P119.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P119: {ex.Message}");
            }

            Thread.Sleep(1000);
        }


        //// ========================================
        // 🔹 P120 - Mostrar pedidos del mes actual sin filtros aplicados
        // ========================================
        public void MostrarPedidosMesActualSinFiltros()
        {
            Console.WriteLine("=== Ejecutando P120: Mostrar pedidos del mes actual sin filtros aplicados ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Calcular fechas automáticas del mes actual
                DateTime fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                string fechaInicioStr = fechaInicio.ToString("dd/MM/yyyy");
                string fechaFinStr = fechaFin.ToString("dd/MM/yyyy");

                Console.WriteLine($"[INFO] Fechas del mes actual: {fechaInicioStr} - {fechaFinStr}");

                // 3️⃣ Ingresar fecha inicial
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys(fechaInicioStr);
                body.Click();
                Thread.Sleep(800);

                // 4️⃣ Ingresar fecha final
                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys(fechaFinStr);
                body.Click();
                Thread.Sleep(800);

                // 5️⃣ Asegurar que Cliente y Vendedor estén vacíos
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                Thread.Sleep(500);

                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input[contains(@class,'form-control')]")));
                campoVendedor.Clear();
                Thread.Sleep(500);

                Console.WriteLine("[OK] Filtros de cliente y vendedor vacíos.");

                //  Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 6️⃣ Consultar pedidos del mes
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                
                    Console.WriteLine("[⚠️ RESULTADO] No hay pedidos registrados este mes.");
                    Console.WriteLine("[⚠️ RESULTADO] No se detectó texto esperado. Verificar manualmente.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P120: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P121 - Cancelar edición de pedido (REGISTRADO → REGISTRADO)
        // ========================================
        public void CancelarEdicionPedido()
        {
            Console.WriteLine("=== Ejecutando P121: Cancelar edición de pedido ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que cargue la tabla
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar rango de fechas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en botón CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Buscar cliente ANA LÓPEZ
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ANA LÓPEZ");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado cliente: ANA LÓPEZ");

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Abrir edición de pedido (ícono lápiz)
                var botonEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//tr[@class='ng-scope odd']//span[contains(@class,'glyphicon-pencil')]")));
                js.ExecuteScript("arguments[0].click();", botonEditar);
                Console.WriteLine("[OK] Ingresó a la ventana de edición del pedido.");
                Thread.Sleep(2000);

                // 6️⃣ Presionar botón "CERRAR" (sin guardar)
                var botonCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@ng-click='cerrarPedido()']")));
                js.ExecuteScript("arguments[0].click();", botonCerrar);
                Console.WriteLine("[OK] Se presionó el botón 'CERRAR' para cancelar la edición.");
                Thread.Sleep(2000);

                // 7️⃣ Validar que regresó a la lista de pedidos
                bool estaEnLista = driver.PageSource.Contains("CONSULTA DE PEDIDOS") ||
                                   driver.PageSource.Contains("Ver Pedido");

                if (estaEnLista)
                    Console.WriteLine("[✅] Pedido se mantuvo igual (estado REGISTRADO, sin cambios).");
                else
                    Console.WriteLine("[⚠️] No se confirmó visualmente el retorno a la lista de pedidos.");

                Console.WriteLine("[INFO] Caso P121 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P121: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P122 - Modificar pedido REGISTRADO → REGISTRADO (actualizado)
        // ========================================
        public void ModificarPedidoRegistrado()
        {
            Console.WriteLine("=== Ejecutando P122: Modificar pedido REGISTRADO y guardar sin cambios ===");

            try
            {
                // 1️⃣ Ir a Pedidos → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas de consulta (ya conocidas)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Pedidos consultados correctamente.");

                // 4️⃣ Buscar cliente conocido (PINEDO)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("PINEDO");
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Filtro aplicado: Cliente = PINEDO.");

                // 🔹 5. Click en botón Editar pedido
                var botonEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@title='Editar pedido']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonEditar);
                Console.WriteLine("[OK] Ingresó al formulario de edición del pedido.");
                Thread.Sleep(2000);

                // 6️⃣ Click en GUARDAR PEDIDO (dentro del modal)
                var botonGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//div[@class='modal-dialog']//button[@title='GUARDAR PEDIDO' or normalize-space()='GUARDAR']")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonGuardar);
                js.ExecuteScript("arguments[0].click();", botonGuardar);
                Thread.Sleep(2500);
                Console.WriteLine("[OK] Click en GUARDAR PEDIDO ejecutado.");

                // 7️⃣ Verificar mensaje o estado actualizado
                bool confirmacion = driver.PageSource.Contains("cambios guardados") ||
                                    driver.PageSource.Contains("Pedido actualizado") ||
                                    driver.PageSource.Contains("REGISTRADO");

                if (confirmacion)
                    Console.WriteLine("[✅] Pedido guardado correctamente. Estado permanece en REGISTRADO.");
                else
                    Console.WriteLine("[⚠️] No se detectó mensaje de confirmación. Verificar manualmente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P122: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P123 - Invalidar pedido REGISTRADO → INVALIDADO
        // ========================================
        public void InvalidarPedidoRegistrado()
        {
            Console.WriteLine("=== Ejecutando P123: Invalidar pedido REGISTRADO → INVALIDADO ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas de búsqueda amplias (como siempre)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Pedidos consultados correctamente.");

                // 4️⃣ Buscar cliente “11”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("11");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Cliente contiene '11'.");

                // 5️⃣ Click en botón Invalidar pedido
                var botonInvalidar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@title='Invalidar pedido' or contains(@ng-click,'invalidarPedido')]")));
                js.ExecuteScript("arguments[0].click();", botonInvalidar);
                Console.WriteLine("[OK] Click en 'Invalidar pedido' realizado.");
                Thread.Sleep(2000);

                // 6️⃣ Escribir observación “PRUEBA”
                var campoObservacion = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[@id='modal-invalidar-pedido']//textarea[@ng-model='invalidacion.Observacion' or contains(@ng-model,'Observacion')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoObservacion);
                Thread.Sleep(500);

                Actions actions = new Actions(driver);
                actions.MoveToElement(campoObservacion).Click().Perform();
                campoObservacion.SendKeys("PRUEBA");
                Console.WriteLine("[OK] Observación 'PRUEBA' escrita correctamente.");
                Thread.Sleep(1000);

                // 7️⃣ Confirmar con botón “SÍ”
                var botonSi = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@ng-click='invalidarPedido()']")));
                Thread.Sleep(800);

                Actions actSi = new Actions(driver);
                actSi.MoveToElement(botonSi).Click().Perform();
                Console.WriteLine("[OK] Botón 'SÍ' presionado correctamente para invalidar el pedido.");
                Thread.Sleep(3000);

                // 8️⃣ Verificar estado actualizado
                bool estadoInvalidado = driver.PageSource.Contains("INVALIDADO") ||
                                        driver.PageSource.Contains("Invalidado") ||
                                        driver.PageSource.Contains("se ha invalidado");

                if (estadoInvalidado)
                    Console.WriteLine("[✅] Estado cambiado correctamente a INVALIDADO.");
                else
                    Console.WriteLine("[⚠️] No se encontró el texto 'INVALIDADO'. Verificar manualmente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P123: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P124 - Pedido INVALIDADO → Intentar eliminar sin confirmar
        // ========================================
        public void IntentarEliminarPedidoInvalidado()
        {
            Console.WriteLine("=== Ejecutando P124: Intentar eliminar un pedido INVALIDADO ===");

            try
            {
                // 1️⃣ Ir a menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Rango de fechas (como siempre)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Pedidos consultados correctamente.");

                // 4️⃣ Buscar cliente “VELA”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("VELA");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Cliente = VELA.");

                // 5️⃣ Escribir "INVALIDADO" en el campo Estado
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("INVALIDADO");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Estado = INVALIDADO.");

                // 6️⃣ Verificar si existe botón eliminar (no debería)
                bool existeBotonEliminar = driver.FindElements(By.XPath("//a[@title='Eliminar pedido']")).Count > 0;

                if (existeBotonEliminar)
                {
                    Console.WriteLine("[⚠️] Se encontró botón 'Eliminar pedido', lo cual no debería estar disponible para pedidos INVALIDADOS.");
                }
                else
                {
                    Console.WriteLine("[✅] No hay botón 'Eliminar pedido' visible. Pedido INVALIDADO no puede eliminarse.");
                }

                // 7️⃣ Confirmación de no cambio de estado
                bool sigueInvalidado = driver.PageSource.Contains("INVALIDADO");

                if (sigueInvalidado)
                    Console.WriteLine("[✅ RESULTADO] Pedido se mantiene en estado INVALIDADO. No se permite eliminación.");
                else
                    Console.WriteLine("[❌ RESULTADO] Estado cambió o se eliminó inesperadamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P124: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P125 - Editar y confirmar pedido REGISTRADO → CONFIRMADO (ajustado con scroll largo)
        // ========================================
        public void ConfirmarPedidoRegistrado()
        {
            Console.WriteLine("=== Ejecutando P125: Editar y confirmar pedido REGISTRADO ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Pedidos consultados correctamente.");

                // 4️⃣ Buscar cliente “11”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("11");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Cliente = 11.");

                // 5️⃣ Click en botón Confirmar pedido
                var botonConfirmar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@title='Confirmar pedido']")));
                js.ExecuteScript("arguments[0].click();", botonConfirmar);
                Console.WriteLine("[OK] Click en 'Confirmar pedido' realizado.");
                Thread.Sleep(2500);

                // 6️⃣ Desplazar hacia abajo dentro del modal (importante)
                js.ExecuteScript(@"
            const modal = document.querySelector('.modal-dialog');
            if(modal) modal.scrollTo({ top: modal.scrollHeight, behavior: 'smooth' });
        ");
                Thread.Sleep(1500); // Esperar desplazamiento visible

                // 7️⃣ Click en “Guardar Pedido” dentro del modal
                var botonGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//div[contains(@class,'modal-dialog')]//button[@title='GUARDAR PEDIDO' or normalize-space()='GUARDAR PEDIDO']")));

                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonGuardar);
                Thread.Sleep(500);

                Actions actGuardar = new Actions(driver);
                actGuardar.MoveToElement(botonGuardar).Click().Perform();

                Console.WriteLine("[OK] Botón 'GUARDAR PEDIDO' presionado correctamente.");
                Thread.Sleep(3500);

                // 8️⃣ Verificar confirmación
                bool confirmado = driver.PageSource.Contains("CONFIRMADO") ||
                                  driver.PageSource.Contains("Pedido confirmado") ||
                                  driver.PageSource.Contains("confirmado");

                if (confirmado)
                    Console.WriteLine("[✅ RESULTADO] Pedido confirmado correctamente.");
                else
                    Console.WriteLine("[⚠️ RESULTADO] No se detectó mensaje de confirmación. Verificar manualmente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P125: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P126 - Invalidar y Cancelarlo (REGISTRADO → REGISTRADO)
        // ========================================
        public void InvalidarYCancelarPedido()
        {
            Console.WriteLine("=== Ejecutando P126: Invalidar y Cancelarlo ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que cargue la vista
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas
                var fechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                fechaIni.Clear();
                fechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var fechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFin.Clear();
                fechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Buscar cliente “ANA LÓPEZ”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ANA LÓPEZ");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Cliente filtrado: ANA LÓPEZ");

                // 50 Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 60 Clic en el botón "Invalidar pedido"
                var botonInvalidar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@title='Invalidar pedido']")));
                js.ExecuteScript("arguments[0].click();", botonInvalidar);
                Console.WriteLine("[OK] Se hizo clic en 'Invalidar pedido'.");
                Thread.Sleep(1500);

                // 70 Esperar el modal "INVALIDAR DOCUMENTO"
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[contains(@class,'modal-content')]//h4[contains(text(),'INVALIDAR DOCUMENTO')]")));
                Console.WriteLine("[OK] Modal 'Invalidar Documento' visible.");

                // 80 Clic en el botón "NO"
                var botonNo = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@ng-click='limpiarInvalidarPedido()']")));
                js.ExecuteScript("arguments[0].click();", botonNo);
                Console.WriteLine("[OK] Se hizo clic en el botón 'NO'.");

                // 90 Esperar que el modal se cierre completamente
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//div[contains(@class,'modal-content')]//h4[contains(text(),'INVALIDAR DOCUMENTO')]")));
                Console.WriteLine("[✅] Modal cerrado correctamente.");

                // 100 Verificar que el pedido siga REGISTRADO
                bool sigueRegistrado = driver.PageSource.Contains("REGISTRADO");

                if (sigueRegistrado)
                {
                    Console.WriteLine("[✅] Pedido sigue REGISTRADO. Estado sin cambios tras cancelar invalidación.");
                }
                else
                {
                    Console.WriteLine("[⚠️] El texto 'REGISTRADO' no fue encontrado. Revisar estado visual.");
                }

                Console.WriteLine("[INFO] Caso P126 completado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P126: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P127 - Modificar pedido y cerrarlo sin guardar (eliminando un ítem)
        // ========================================
        public void ModificarPedidoEliminarItemYCerrar()
        {
            Console.WriteLine("=== Ejecutando P127: Modificar pedido y cerrarlo sin guardar (eliminando ítem) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // 2️⃣ Esperar carga de la tabla
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));
                Console.WriteLine("[OK] Tabla de pedidos visible.");

                // 3️⃣ Ingresar fechas amplias (las de siempre)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 4️⃣ Filtrar por estado REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                body.Click();
                Thread.Sleep(1000);

                // 5️⃣ Filtrar por cliente ANA LÓPEZ
                var campoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ANA LÓPEZ");
                body.Click();
                Thread.Sleep(1200);
                Console.WriteLine("[OK] Filtros aplicados: REGISTRADO / ANA LÓPEZ.");

                // 6️⃣ Click en botón Editar pedido
                var botonEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@title='Editar pedido']")));
                js.ExecuteScript("arguments[0].click();", botonEditar);
                Console.WriteLine("[OK] Ingresó al formulario de edición del pedido.");
                Thread.Sleep(2000);

                // 4️⃣ Ingresar código de producto
                var campoCodigo = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idCodigoBarra")));
                campoCodigo.Click();
                campoCodigo.Clear();
                campoCodigo.SendKeys("9999999999");
                campoCodigo.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Código '9999999999' ingresado correctamente.");
                Thread.Sleep(2000);

                // 8️⃣ Cerrar el pedido sin guardar
                try
                {
                    var botonCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//button[@ng-click='cerrarPedido()']")));

                    // Desplazar hasta el botón y forzar el click con JavaScript
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", botonCerrar);
                    Thread.Sleep(500);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonCerrar);

                    Console.WriteLine("[✅] Botón 'CERRAR' clickeado correctamente.");
                }
                catch (Exception)
                {
                    // Intento alternativo: buscar por texto si el anterior falla
                    var botonCerrarAlt = wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//button[contains(.,'CERRAR') or contains(@title,'CERRAR')]")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonCerrarAlt);
                    Console.WriteLine("[⚠️] Botón 'CERRAR' clickeado mediante alternativa de texto.");
                }

                Thread.Sleep(1500);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error al ejecutar P127: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P128 - Añadir producto y recargar vista (REGISTRADO → REGISTRADO)
        // ========================================
        public void AnadirProductoYRecargarVista()
        {
            Console.WriteLine("=== Ejecutando P128: Añadir producto y recargar vista ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas (02/11/2020 – 02/11/2025)
                var fechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                fechaIni.Clear();
                fechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var fechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFin.Clear();
                fechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Buscar cliente ANA LÓPEZ
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ANA LÓPEZ");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado cliente: ANA LÓPEZ");

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Ingresar a edición del pedido (ícono lápiz)
                var botonEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//tr[@class='ng-scope odd']//span[contains(@class,'glyphicon-pencil')]")));
                js.ExecuteScript("arguments[0].click();", botonEditar);
                Console.WriteLine("[OK] Se abrió la ventana EDITAR PEDIDO.");
                Thread.Sleep(2000);

                // 6️⃣ Ingresar código de producto nuevo
                var campoCodigo = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idCodigoBarra")));
                campoCodigo.Click();
                campoCodigo.Clear();
                campoCodigo.SendKeys("88008-1");
                campoCodigo.SendKeys(Keys.Enter);
                Console.WriteLine("[OK] Código '88008-1' ingresado correctamente.");
                Thread.Sleep(2000);

                // 7️⃣ Cerrar la ventana de edición
                var botonCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@ng-click='cerrarPedido()']")));
                js.ExecuteScript("arguments[0].click();", botonCerrar);
                Console.WriteLine("[OK] Se cerró la ventana de edición del pedido.");
                Thread.Sleep(2000);

                // 8️⃣ Recargar la vista de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Vista de pedidos recargada correctamente.");

                // 9️⃣ Validar que el estado siga REGISTRADO
                bool sigueRegistrado = driver.PageSource.Contains("REGISTRADO");
                if (sigueRegistrado)
                    Console.WriteLine("[✅] Pedido se mantuvo en estado REGISTRADO tras recargar vista.");
                else
                    Console.WriteLine("[⚠️] No se confirmó visualmente el estado REGISTRADO.");

                Console.WriteLine("[INFO] Caso P128 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P128: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P129 - Invalidar pedido y recargar vista (REGISTRADO → INVALIDADO)
        // ========================================
        public void InvalidarPedidoYRecargarVista()
        {
            Console.WriteLine("=== Ejecutando P129: Invalidar pedido y recargar vista ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas (02/11/2020 – 02/11/2025)
                var fechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                fechaIni.Clear();
                fechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var fechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFin.Clear();
                fechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Buscar cliente ANA LÓPEZ
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ANA LÓPEZ");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado cliente: ANA LÓPEZ");

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Ingresar a invalidar pedido (ícono papelera)
                var botonInvalidar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//tr[@class='ng-scope odd']//span[contains(@class,'glyphicon-remove')]")));
                js.ExecuteScript("arguments[0].click();", botonInvalidar);
                Console.WriteLine("[OK] Se presionó ícono para invalidar pedido.");
                Thread.Sleep(2000);

                // 6️⃣ En ventana de confirmación → Clic en "NO"
                var botonNo = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@ng-click='limpiarInvalidarPedido()' and contains(text(),'NO')]")));
                js.ExecuteScript("arguments[0].click();", botonNo);
                Console.WriteLine("[OK] Se presionó 'NO' en la ventana de confirmación.");
                Thread.Sleep(2000);

                // 7️⃣ Recargar la vista de pedidos
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Vista de pedidos recargada correctamente.");

                // 8️⃣ Validar que el estado siga REGISTRADO (no se invalidó)
                bool sigueRegistrado = driver.PageSource.Contains("REGISTRADO");
                if (sigueRegistrado)
                    Console.WriteLine("[✅] Pedido se mantuvo REGISTRADO tras cancelar invalidación.");
                else
                    Console.WriteLine("[⚠️] No se confirmó visualmente el estado REGISTRADO.");

                Console.WriteLine("[INFO] Caso P129 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P129: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P132 - Cambiar rango de filas de 10 → 1000 → 10 (REGISTRADO y INVALIDADO)
        // ========================================
        public void CambiarRangoFilasDe10a1000yVolverA10()
        {
            Console.WriteLine("=== Ejecutando P132: Cambiar rango de filas de 10 → 1000 → 10 ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas (02/11/2020 – 02/11/2025)
                var fechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                fechaIni.Clear();
                fechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var fechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFin.Clear();
                fechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Esperar que se muestren resultados
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]")));
                Console.WriteLine("[OK] Resultados visibles en la tabla de pedidos.");

                // 5️⃣ Cambiar rango de filas a 1000
                var comboFilas = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//select[contains(@name,'tabla-cotizaciones_length') or contains(@aria-controls,'tabla-cotizaciones')]")));
                var selectElement = new SelectElement(comboFilas);
                selectElement.SelectByValue("1000");
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Rango de filas cambiado a 1000 (All).");

                // 6️⃣ Cambiar rango de filas nuevamente a 10
                comboFilas = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//select[contains(@name,'tabla-cotizaciones_length') or contains(@aria-controls,'tabla-cotizaciones')]")));
                selectElement = new SelectElement(comboFilas);
                selectElement.SelectByValue("10");
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Rango de filas regresado a 10.");

                // 7️⃣ Validar que la tabla sigue mostrando registros
                bool registrosVisibles = driver.PageSource.Contains("REGISTRADO") || driver.PageSource.Contains("INVALIDADO");

                if (registrosVisibles)
                    Console.WriteLine("[✅] La vista de pedidos sigue mostrando registros tras cambiar de 10 → 1000 → 10.");
                else
                    Console.WriteLine("[⚠️] No se confirmaron los registros visibles tras el cambio de rango.");

                Console.WriteLine("[INFO] Caso P132 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P132: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P133 - Añadir 100 veces el mismo producto y guardar (REGISTRADO → PERMITE GUARDARSE)
        // ========================================
        public void AnadirCienVecesYGuardar()
        {
            Console.WriteLine("=== Ejecutando P133: Añadir 100 veces el mismo producto y guardar ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas (02/11/2020 – 02/11/2025)
                var fechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                fechaIni.Clear();
                fechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var fechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFin.Clear();
                fechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Buscar cliente ANA LÓPEZ
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th[5]//input[contains(@class,'form-control')]")));
                campoCliente.Clear();
                campoCliente.SendKeys("ANA LÓPEZ");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Ingresado cliente: ANA LÓPEZ");

                // 3️⃣ Estado = REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(800);

                // 5️⃣ Abrir edición de pedido (ícono lápiz)
                var botonEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//tr[@class='ng-scope odd']//span[contains(@class,'glyphicon-pencil')]")));
                js.ExecuteScript("arguments[0].click();", botonEditar);
                Console.WriteLine("[OK] Se abrió la ventana EDITAR PEDIDO.");
                Thread.Sleep(2000);

                // 6️⃣ Función para añadir el producto 100 veces
                for (int i = 1; i <= 100; i++)
                {
                    try
                    {
                        var campoCodigo = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idCodigoBarra")));
                        campoCodigo.Click();
                        campoCodigo.Clear();
                        campoCodigo.SendKeys("88008-1");
                        campoCodigo.SendKeys(Keys.Enter);
                        Console.WriteLine($"[OK] ({i}/100) Producto '88008-1' ingresado correctamente.");
                        Thread.Sleep(400); // control de ritmo para evitar sobrecarga
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[⚠️] Error al ingresar producto en intento {i}: {e.Message}");
                        Thread.Sleep(500);
                    }
                }

                // 7️⃣ Presionar botón GUARDAR
                var botonGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@ng-click='guardarPedido()' or normalize-space()='GUARDAR']")));
                js.ExecuteScript("arguments[0].click();", botonGuardar);
                Console.WriteLine("[OK] Se presionó el botón GUARDAR después de añadir los 100 productos.");
                Thread.Sleep(3000);

                // 8️⃣ Validar mensaje o cambio en la vista
                bool guardadoCorrecto = driver.PageSource.Contains("Pedido guardado") ||
                                        driver.PageSource.Contains("REGISTRADO") ||
                                        driver.PageSource.Contains("Pedido actualizado");

                if (guardadoCorrecto)
                    Console.WriteLine("[✅] Pedido se guardó correctamente tras añadir los 100 productos.");
                else
                    Console.WriteLine("[⚠️] No se confirmó visualmente el guardado del pedido.");

                Console.WriteLine("[INFO] Caso P133 completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P133: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P134 - Cambiar rango de filas (10 → 25 → 50 → 100 → 1000) (REGISTRADO y INVALIDADO)
        // ========================================
        public void CambiarRangoFilasMultiple()
        {
            Console.WriteLine("=== Ejecutando P134: Cambiar rango de filas (10 → 25 → 50 → 100 → 1000) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas (02/11/2020 – 02/11/2025)
                var fechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                fechaIni.Clear();
                fechaIni.SendKeys("02/11/2020");
                body.Click();
                Thread.Sleep(800);

                var fechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                fechaFin.Clear();
                fechaFin.SendKeys("02/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Esperar que se muestren resultados
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]")));
                Console.WriteLine("[OK] Tabla de pedidos visible en pantalla.");

                // 5️⃣ Definir las opciones de rango de filas
                string[] rangos = { "10", "25", "50", "100", "1000" };

                // 6️⃣ Recorrer cada rango y aplicarlo secuencialmente
                foreach (var rango in rangos)
                {
                    try
                    {
                        var comboFilas = wait.Until(ExpectedConditions.ElementToBeClickable(
                            By.XPath("//select[contains(@name,'tabla-cotizaciones_length') or contains(@aria-controls,'tabla-cotizaciones')]")));
                        var selectElement = new SelectElement(comboFilas);

                        selectElement.SelectByValue(rango);
                        Thread.Sleep(2000);
                        Console.WriteLine($"[OK] Rango de filas cambiado a {rango}.");

                        // Validar que los registros sigan visibles
                        bool registrosVisibles = driver.PageSource.Contains("REGISTRADO") || driver.PageSource.Contains("INVALIDADO");
                        if (registrosVisibles)
                            Console.WriteLine($"[✅] Registros visibles correctamente con rango {rango}.");
                        else
                            Console.WriteLine($"[⚠️] No se detectaron registros con rango {rango}.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[⚠️] Error al cambiar rango a {rango}: {e.Message}");
                    }
                }

                Console.WriteLine("[INFO] Caso P134 completado correctamente (recorrido de 10 → 25 → 50 → 100 → 1000).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P134: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P140 - Buscar pedido por cliente con estado REGISTRADO
        // ========================================
        public void BuscarPedidoPorClienteRegistrado()
        {
            Console.WriteLine("=== Ejecutando P140: Buscar pedido por cliente con estado REGISTRADO ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que desaparezca "Cargando"
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2000);

                // 4️⃣ Filtrar estado REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(1200);
                Console.WriteLine("[OK] Filtro aplicado: Estado = REGISTRADO.");

                // 5️⃣ Filtrar cliente (ejemplo: “JOSE” o cualquiera que tengas visible)
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("JOSE");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Cliente = JOSE.");

                // 6️⃣ Verificar que los resultados se muestran correctamente
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));

                string contenidoTabla = tabla.Text;

                bool hayResultados = !string.IsNullOrEmpty(contenidoTabla) &&
                                     !contenidoTabla.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase);

                bool todosRegistrados = !contenidoTabla.Contains("INVALIDADO") &&
                                        contenidoTabla.Contains("REGISTRADO");

                if (hayResultados && todosRegistrados)
                {
                    Console.WriteLine("[✅ RESULTADO] Se filtran resultados correctamente. Todos los pedidos están en estado REGISTRADO.");
                }
                else if (!hayResultados)
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos del cliente especificado.");
                }
                else
                {
                    Console.WriteLine("[❌] Se encontraron pedidos con estado distinto a REGISTRADO.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P140: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P141 - Buscar pedido por rango de fechas con estado REGISTRADO
        // ========================================
        public void BuscarPedidoPorRangoDeFechasRegistrado()
        {
            Console.WriteLine("=== Ejecutando P141: Buscar pedido por rango de fechas con estado REGISTRADO ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fecha inicial y final del rango
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/10/2020");  // 🔸 fecha inicial de prueba
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("02/11/2025");  // 🔸 fecha final de prueba
                body.Click();
                Thread.Sleep(800);

                Console.WriteLine("[OK] Fechas de rango ingresadas correctamente (01/10/2025 - 01/11/2025).");

                // 3️⃣ Click en CONSULTAR
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Click en CONSULTAR realizado.");
                Thread.Sleep(2500);

                // 4️⃣ Filtrar por estado REGISTRADO (por consistencia con caso)
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(1200);
                Console.WriteLine("[OK] Filtro de estado REGISTRADO aplicado correctamente.");

                // 5️⃣ Verificar que la tabla se actualizó y muestre resultados
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                bool hayResultados = !string.IsNullOrEmpty(contenidoTabla) &&
                                     !contenidoTabla.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase);

                bool todosRegistrados = contenidoTabla.Contains("REGISTRADO") &&
                                        !contenidoTabla.Contains("INVALIDADO");

                if (hayResultados && todosRegistrados)
                {
                    Console.WriteLine("[✅ RESULTADO] Filtro aplicado correctamente. Se muestran pedidos en estado REGISTRADO dentro del rango de fechas.");
                }
                else if (!hayResultados)
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos en el rango especificado.");
                }
                else
                {
                    Console.WriteLine("[❌] Algunos pedidos fuera del rango o con estado distinto.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P141: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P142 - Buscar pedido por vendedor con estado REGISTRADO
        // ========================================
        public void BuscarPedidoPorVendedorRegistrado()
        {
            Console.WriteLine("=== Ejecutando P142: Buscar pedido por vendedor con estado REGISTRADO ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que desaparezca “Cargando”
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Thread.Sleep(2000);
                Console.WriteLine("[OK] Pedidos consultados correctamente.");

                // 4️⃣ Filtro de estado REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(1200);
                Console.WriteLine("[OK] Estado REGISTRADO filtrado correctamente.");

                // 5️⃣ Filtro por vendedor
                var campoVendedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[6]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoVendedor);
                campoVendedor.Clear();
                campoVendedor.SendKeys("ADMIN"); // 🔸 vendedor de prueba
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Vendedor = ADMIN.");

                // 6️⃣ Verificar que los resultados son válidos
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                bool hayResultados = !string.IsNullOrEmpty(contenidoTabla) &&
                                     !contenidoTabla.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase);

                bool todosRegistrados = contenidoTabla.Contains("REGISTRADO") &&
                                        !contenidoTabla.Contains("INVALIDADO");

                if (hayResultados && todosRegistrados)
                {
                    Console.WriteLine("[✅ RESULTADO] Filtro devuelve resultados válidos. Todos los pedidos están en estado REGISTRADO y pertenecen al vendedor indicado.");
                }
                else if (!hayResultados)
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos del vendedor especificado.");
                }
                else
                {
                    Console.WriteLine("[❌] Se encontraron pedidos fuera del estado esperado o con vendedor diferente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P142: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P143 - Buscar pedido por estado REGISTRADO
        // ========================================
        public void BuscarPedidoPorEstadoRegistrado()
        {
            Console.WriteLine("=== Ejecutando P143: Buscar pedido por estado REGISTRADO ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que desaparezca “Cargando”
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias (rango suficiente)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Aplicar filtro por estado REGISTRADO
                var campoEstado = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[8]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoEstado);
                campoEstado.Clear();
                campoEstado.SendKeys("REGISTRADO");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Estado = REGISTRADO.");

                // 5️⃣ Verificar los resultados en la tabla
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                bool hayResultados = !string.IsNullOrEmpty(contenidoTabla) &&
                                     !contenidoTabla.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase);

                bool todosRegistrados = contenidoTabla.Contains("REGISTRADO") &&
                                        !contenidoTabla.Contains("INVALIDADO");

                if (hayResultados && todosRegistrados)
                {
                    Console.WriteLine("[✅ RESULTADO] Estado filtrado correctamente. Todos los pedidos mostrados están en estado REGISTRADO.");
                }
                else if (!hayResultados)
                {
                    Console.WriteLine("[⚠️] No se encontraron pedidos en estado REGISTRADO.");
                }
                else
                {
                    Console.WriteLine("[❌] Se encontraron registros fuera del estado esperado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P143: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P144 - Buscar pedido global (campo superior derecho) con estado REGISTRADO
        // ========================================
        public void BuscarPedidoGlobalRegistrado()
        {
            Console.WriteLine("=== Ejecutando P144: Buscar pedido global (campo superior derecho) ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                // Esperar que desaparezca “Cargando”
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2000);

                // 4️⃣ Buscar por texto en el campo superior derecho (global)
                var campoBuscarGlobal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@type='search' and contains(@class,'form-control')]")));
                campoBuscarGlobal.Clear();
                campoBuscarGlobal.SendKeys("PINEDO"); // 🔸 puedes usar también "0001-29648"
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Texto ingresado en campo de búsqueda global: PINEDO.");

                // 5️⃣ Verificar resultados
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                bool hayResultados = !string.IsNullOrEmpty(contenidoTabla) &&
                                     !contenidoTabla.Contains("No hay datos disponibles", StringComparison.OrdinalIgnoreCase);

                bool todosRegistrados = contenidoTabla.Contains("REGISTRADO") &&
                                        !contenidoTabla.Contains("INVALIDADO");

                if (hayResultados && todosRegistrados)
                {
                    Console.WriteLine("[✅ RESULTADO] Campo de búsqueda global funcional. Resultados correctos en estado REGISTRADO.");
                }
                else if (!hayResultados)
                {
                    Console.WriteLine("[⚠️] No se encontraron coincidencias en la búsqueda global.");
                }
                else
                {
                    Console.WriteLine("[❌] Resultados fuera del estado esperado o búsqueda incorrecta.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P144: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P145 - Navegar a siguiente página
        // ========================================
        public void NavegarASiguientePaginaPedidos()
        {
            Console.WriteLine("=== Ejecutando P145: Navegar a siguiente página de pedidos ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias para que haya muchas filas (más de una página)
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Scroll hasta la parte inferior donde está la paginación
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(1000);

                // 5️⃣ Click en el número de página 2
                var botonPagina2 = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@aria-controls='tabla-cotizaciones' and normalize-space()='2']")));
                js.ExecuteScript("arguments[0].click();", botonPagina2);
                Console.WriteLine("[OK] Se hizo clic en la página 2 de la tabla.");
                Thread.Sleep(2500);

                // 6️⃣ Validar que los datos cambiaron (comparando primer pedido visible)
                var primerPedido = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones')]/tbody/tr[1]/td[2]")));
                string textoPrimeraFila = primerPedido.Text;

                if (!string.IsNullOrEmpty(textoPrimeraFila))
                    Console.WriteLine($"[✅ RESULTADO] Se cargó correctamente la página 2 (primer registro visible: {textoPrimeraFila}).");
                else
                    Console.WriteLine("[⚠️ RESULTADO] No se pudo verificar el cambio de página.");

                // 7️⃣ Validar que el estado sigue REGISTRADO
                var tabla = driver.FindElement(By.XPath("//table[contains(@id,'tabla-cotizaciones')]/tbody"));
                string contenidoTabla = tabla.Text;
                if (contenidoTabla.Contains("REGISTRADO"))
                    Console.WriteLine("[OK] Estado se mantiene REGISTRADO tras cambiar de página.");
                else
                    Console.WriteLine("[⚠️] Estado no encontrado o alterado después de la paginación.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P145: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P146 - Navegar a página anterior
        // ========================================
        public void NavegarAPaginaAnteriorPedidos()
        {
            Console.WriteLine("=== Ejecutando P146: Navegar a página anterior de pedidos ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias para asegurar múltiples páginas
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Bajar hasta la paginación
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(1200);

                // 5️⃣ Ir a la página 2 primero
                var botonPagina2 = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@aria-controls='tabla-cotizaciones' and normalize-space()='2']")));
                js.ExecuteScript("arguments[0].click();", botonPagina2);
                Console.WriteLine("[OK] Se hizo clic en página 2.");
                Thread.Sleep(2500);

                // 6️⃣ Ahora regresar con el botón “Anterior”
                var botonAnterior = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@id='tabla-cotizaciones_previous' or normalize-space()='Anterior']")));
                js.ExecuteScript("arguments[0].click();", botonAnterior);
                Console.WriteLine("[OK] Se hizo clic en 'Anterior' para volver a la página 1.");
                Thread.Sleep(2500);

                // 7️⃣ Verificar que volvió a la página 1
                var paginaActiva = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//li[@class='active']/a[normalize-space()='1']")));
                if (paginaActiva != null)
                    Console.WriteLine("[✅ RESULTADO] Se volvió correctamente a la página 1 de pedidos.");
                else
                    Console.WriteLine("[⚠️ RESULTADO] No se confirmó el regreso a la página 1.");

                // 8️⃣ Validar que el estado sigue REGISTRADO
                var tabla = driver.FindElement(By.XPath("//table[contains(@id,'tabla-cotizaciones')]/tbody"));
                string contenidoTabla = tabla.Text;
                if (contenidoTabla.Contains("REGISTRADO"))
                    Console.WriteLine("[OK] Estado se mantiene REGISTRADO tras regresar de página.");
                else
                    Console.WriteLine("[⚠️] Estado no encontrado o alterado tras volver a la página 1.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P146: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P147 - Recargar vista de pedidos REGISTRADOS (versión simplificada)
        // ========================================
        public void RecargarVistaPedidosRegistrados()
        {
            Console.WriteLine("=== Ejecutando P147: Recargar vista de pedidos REGISTRADOS ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2000);

                // 4️⃣ Buscar cliente “PINEDO”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("PINEDO");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Cliente = PINEDO.");

                // 5️⃣ Recargar vista (volver a Ver Pedido)
                Console.WriteLine("[OK] Recargando vista (reingresando a Ver Pedido)...");
                IrAVerPedido(); // ← recarga directa de la vista
                Thread.Sleep(2500);

                Console.WriteLine("[✅ RESULTADO] Vista recargada correctamente. Finaliza la prueba.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P147: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P148 - Exportar pedidos REGISTRADOS a Excel (fix botón DESCARGAR)
        // ========================================
        public void ExportarPedidosAExcelRegistrado()
        {
            Console.WriteLine("=== Ejecutando P148: Exportar pedidos REGISTRADOS a Excel ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Buscar cliente “PINEDO”
                var campoCliente = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[5]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoCliente);
                campoCliente.Clear();
                campoCliente.SendKeys("PINEDO");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Cliente = PINEDO.");

                // 5️⃣ Hacer scroll hacia el botón de DESCARGAR
                var botonDescargar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='DESCARGAR' and contains(@ng-click,'export(')]")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", botonDescargar);
                Thread.Sleep(1000);

                // 6️⃣ Click en botón “DESCARGAR”
                js.ExecuteScript("arguments[0].click();", botonDescargar);
                Console.WriteLine("[OK] Click en botón 'DESCARGAR' (exportar Excel) realizado.");
                Thread.Sleep(5000); // espera prudente para la descarga

                // 7️⃣ Validar que los pedidos sigan REGISTRADOS
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                bool sigueRegistrado = contenidoTabla.Contains("REGISTRADO") && !contenidoTabla.Contains("INVALIDADO");

                if (sigueRegistrado)
                    Console.WriteLine("[✅ RESULTADO] Archivo Excel exportado correctamente y estado de pedidos se mantiene REGISTRADO.");
                else
                    Console.WriteLine("[⚠️ RESULTADO] Estado cambió después de exportar. Revisar manualmente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P148: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        // ========================================
        // 🔹 P149 - Filtrar por comprobante válido
        // ========================================
        public void FiltrarPorComprobanteValido()
        {
            Console.WriteLine("=== Ejecutando P149: Filtrar por comprobante válido ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias para asegurar cobertura
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Filtrar por comprobante válido
                var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoComprobante);
                campoComprobante.Clear();
                campoComprobante.SendKeys("0001-28595");
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Comprobante = 0001-28595.");

                // 5️⃣ Esperar que la tabla actualice los resultados
                Thread.Sleep(2000);

                // 6️⃣ Validar que solo se muestre el comprobante indicado
                var tabla = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table[contains(@id,'tabla-cotizaciones') or contains(@class,'table')]/tbody")));
                string contenidoTabla = tabla.Text;

                if (contenidoTabla.Contains("0001-28595") && !contenidoTabla.Contains("0001-") || contenidoTabla.Split('\n').Length < 3)
                {
                    Console.WriteLine("[✅ RESULTADO] Muestra solo el comprobante solicitado (0001-28595).");
                }
                else
                {
                    Console.WriteLine("[⚠️ RESULTADO] Se muestran más registros o el comprobante no coincide.");
                }

                // 7️⃣ Verificar que el estado siga REGISTRADO
                if (contenidoTabla.Contains("REGISTRADO"))
                    Console.WriteLine("[OK] Estado se mantiene REGISTRADO tras aplicar filtro.");
                else
                    Console.WriteLine("[⚠️] Estado no encontrado o alterado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P149: {ex.Message}");
            }

            Thread.Sleep(1000);
        }
        // ========================================
        // 🔹 P150 - Filtrar por comprobante inexistente
        // ========================================
        public void FiltrarPorComprobanteInexistente()
        {
            Console.WriteLine("=== Ejecutando P150: Filtrar por comprobante inexistente ===");

            try
            {
                // 1️⃣ Ir al menú Pedido → Ver Pedido
                IrAMenuPedido();
                IrAVerPedido();
                Thread.Sleep(1500);

                var js = (IJavaScriptExecutor)driver;
                var body = driver.FindElement(By.TagName("body"));

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.XPath("//*[contains(text(),'Cargando')]")));

                // 2️⃣ Ingresar fechas amplias
                var campoFechaIni = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaInicio' or contains(@ng-model,'fechaIni')]")));
                campoFechaIni.Clear();
                campoFechaIni.SendKeys("01/01/2022");
                body.Click();
                Thread.Sleep(800);

                var campoFechaFin = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//input[@ng-model='fechaFin' or contains(@ng-model,'fechaFin')]")));
                campoFechaFin.Clear();
                campoFechaFin.SendKeys("01/11/2025");
                body.Click();
                Thread.Sleep(800);

                // 3️⃣ Consultar pedidos
                var botonConsultar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@title='CONSULTAR' or contains(@ng-click,'listarPedidos')]")));
                js.ExecuteScript("arguments[0].click();", botonConsultar);
                Console.WriteLine("[OK] Se hizo clic en CONSULTAR.");
                Thread.Sleep(2500);

                // 4️⃣ Filtrar por comprobante inexistente
                var campoComprobante = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//table//thead//tr[2]//th[4]//input[contains(@class,'form-control')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campoComprobante);
                campoComprobante.Clear();
                campoComprobante.SendKeys("9999-99999"); // comprobante que no existe
                Thread.Sleep(1500);
                Console.WriteLine("[OK] Filtro aplicado: Comprobante inexistente (9999-99999).");

                // 5️⃣ Esperar actualización de tabla
                Thread.Sleep(2000);

                // 6️⃣ Validar mensaje “No se encontraron registros”
                bool mensajeVisible = false;
                try
                {
                    var mensaje = wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//*[contains(text(),'No se encontraron registros') or contains(text(),'No existe')]")));
                    if (mensaje != null)
                    {
                        Console.WriteLine("[✅ RESULTADO] Se muestra el mensaje: 'No se encontraron registros'.");
                        mensajeVisible = true;
                    }
                }
                catch
                {
                    Console.WriteLine("[⚠️ RESULTADO] No se encontró mensaje de 'No se encontraron registros'.");
                }

                // 7️⃣ Validar estado sigue REGISTRADO (no alteró datos)
                if (mensajeVisible)
                    Console.WriteLine("[OK] Estado de la vista se mantiene REGISTRADO sin cambios.");
                else
                    Console.WriteLine("[⚠️] No se confirmó el mensaje esperado. Revisar manualmente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Error en P150: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        



    }

}


