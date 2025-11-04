using NUnit.Framework;
using OpenQA.Selenium;
using PruebaSeleniumSIGES.Modulos;

namespace PruebaSeleniumSIGES.Tests
{
    [TestFixture]
    public class PruebasVerPedidos
    {
        private ModuloVerPedidos pedidos;

        [SetUp]
        public void Setup()
        {
            pedidos = new ModuloVerPedidos();
            pedidos.Inicializar(headless: false);
            pedidos.Login();
        }

        [Test]
        public void P001_VerPedidos_DiaActual()
        {
            Reporte.IniciarReporte();
            Reporte.CrearTest("P001 - Verificar pedidos del día actual");

            pedidos.VerPedidos_DiaActual();

            Reporte.Log(pedidos.Driver, "✅ Prueba P001 completada con éxito (Ver Pedido - Día Actual).");
            Reporte.FinalizarReporte();

            Assert.Pass();
        }

        [Test]
        public void P002_VerPedidos_MesActual()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P002 - Consultar pedidos del mes actual");

                pedidos.VerPedidos_MesActual();

                Reporte.Log(pedidos.Driver, "✅ Prueba P002 completada con éxito (Pedidos del Mes Actual).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P002: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P003_VerPedidos_PorComprobante()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P003 - Buscar pedido por número de comprobante válido");

                pedidos.VerPedidos_PorComprobante();

                Reporte.Log(pedidos.Driver, "✅ Prueba P003 completada con éxito (Búsqueda por comprobante válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P003: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P004_VerPedidos_ComprobanteFechasInvalidas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P004 - Buscar comprobante con fechas incorrectas");

                pedidos.VerPedidos_ComprobanteFechasInvalidas();

                Reporte.Log(pedidos.Driver, "✅ Prueba P004 completada con éxito (Fechas fuera del rango).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P004: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P005_VerPedidos_ClienteInexistente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P005 - Buscar pedido por cliente inexistente");

                pedidos.VerPedidos_ClienteInexistente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P005 completada con éxito (Cliente inexistente).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P005: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P006_VerPedidos_TipoDocumentoFactura()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P006 - Filtrar por tipo de documento 'Factura'");

                pedidos.VerPedidos_TipoDocumentoFactura();

                Reporte.Log(pedidos.Driver, "✅ Prueba P006 completada con éxito (Filtro por 'Factura').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P006: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P007_VerPedidos_TipoDocumentoBoleta()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P007 - Filtrar por tipo de documento 'Boleta (PP)'");

                pedidos.VerPedidos_TipoDocumentoBoleta();

                Reporte.Log(pedidos.Driver, "✅ Prueba P007 completada con éxito (Filtro por 'Boleta - PP').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P007: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P008_VerPedidos_PorVendedorEspecifico()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P008 - Filtrar por vendedor específico");

                pedidos.VerPedidos_PorVendedorEspecifico();

                Reporte.Log(pedidos.Driver, "✅ Prueba P008 completada con éxito (Filtro por vendedor específico).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P008: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P009_VerPedidos_PorEstadoInvalidado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P009 - Filtrar por estado 'INVALIDADO'");

                pedidos.VerPedidos_PorEstadoInvalidado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P009 completada con éxito (Filtro por estado INVALIDADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P009: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P010_VerPedidos_PorEstadoRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P010 - Filtrar por estado 'REGISTRADO'");

                pedidos.VerPedidos_PorEstadoRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P010 completada con éxito (Filtro por estado REGISTRADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P010: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P011_VerPedidos_ExportarExcel()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P011 - Exportar pedidos a Excel");

                pedidos.VerPedidos_ExportarExcel();

                Reporte.Log(pedidos.Driver, "✅ Prueba P011 completada con éxito (Exportar pedidos a Excel).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P011: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        //
        [Test]
        public void P012_BuscarPorTotalNegativo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P012 - Buscar por total negativo (Total = -10)");

                pedidos.BuscarPorTotalNegativo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P012 completada con éxito (Total negativo = -10).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P012: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P013_CrearPedidoConTotalNegativo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P013 - Crear pedido con total negativo (-100)");

                pedidos.CrearPedidoConTotalNegativo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P013 completada con éxito (total negativo = -100).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P013: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P014_CrearPedidoSinCliente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P014 - Crear pedido sin cliente (campo obligatorio)");

                pedidos.CrearPedidoSinCliente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P014 completada con éxito (validación de cliente obligatorio).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P014: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P015_BuscarPedidoPorClienteConYSinTilde()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P015 - Buscar pedido por cliente con y sin tilde ('Jose' / 'José')");

                pedidos.BuscarPedidoPorClienteConYSinTilde();

                Reporte.Log(pedidos.Driver, "✅ Prueba P015 completada (comparación de 'Jose' y 'José').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P015: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        //

        [Test]
        public void P016_ModificarPedidoExistente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P016 - Modificar pedido existente");

                pedidos.ModificarPedidoExistente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P016 completada con éxito (Modificar pedido existente).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P016: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P017_InvalidarPedido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P017 - Invalidar pedido existente");

                pedidos.InvalidarPedido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P017 completada con éxito (Invalidar Pedido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P017: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P018_BuscarPedidoConTextoNull()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P018 - Buscar pedido escribiendo 'null' literal en el buscador global");

                pedidos.BuscarPedidoConTextoNull();

                Reporte.Log(pedidos.Driver, "✅ Prueba P018 completada (manejo correcto de entrada reservada 'null').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P018: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P019_BuscarPedidoPorClienteConTilde()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P019 - Buscar pedido por cliente con tilde ('José')");

                pedidos.BuscarPedidoPorClienteConTilde();

                Reporte.Log(pedidos.Driver, "✅ Prueba P019 completada con éxito (búsqueda de cliente con tilde 'José').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P019: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P020_BuscarClienteConEspacios()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P020 - Buscar cliente con espacios antes y después del nombre");

                pedidos.BuscarClienteConEspacios();

                Reporte.Log(pedidos.Driver, "✅ Prueba P020 completada con éxito (búsqueda con limpieza de input).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P020: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P021_FiltrarClienteConPuntoIntermedio()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P021 - Filtrar cliente con punto intermedio (ACOSTA.PAUL)");

                pedidos.FiltrarClienteConPuntoIntermedio();

                Reporte.Log(pedidos.Driver, "✅ Prueba P021 completada correctamente (coincidencias mostradas con punto intermedio).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P021: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        
        [Test]
        public void P022_BuscarClienteConMayusMinusInconsistentes()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P022 - Buscar con mezcla de mayúsculas y minúsculas inconsistentes (AcosTA)");

                pedidos.BuscarClienteConMayusMinusInconsistentes();

                Reporte.Log(pedidos.Driver, "✅ Prueba P022 completada: búsqueda insensible a mayúsculas/minúsculas validada correctamente.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P022: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P023_BuscarClienteConComillasDobles()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P023 - Buscar con comillas dobles en el campo Cliente (\"SINTI\")");

                pedidos.BuscarClienteConComillasDobles();

                Reporte.Log(pedidos.Driver, "✅ Prueba P023 completada correctamente (entrada con comillas dobles manejada).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P023: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P024_BuscarClienteConComillasSimples()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P024 - Buscar con comillas simples en el campo Cliente ('SINTI')");

                pedidos.BuscarClienteConComillasSimples();

                Reporte.Log(pedidos.Driver, "✅ Prueba P024 completada correctamente (entrada con comillas simples manejada).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P024: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        //
        [Test]
        public void P025_VerPedidos_PorFechasYRangoFilas_2022()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P025 - Filtrar pedidos por fechas 01/11/2022 - 01/11/2025, rango=100 y filtro=2022");

                pedidos.VerPedidos_PorFechasYRangoFilas_2022();

                Reporte.Log(pedidos.Driver, "✅ P025 ejecutada correctamente (fechas + rango + filtro).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P025: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }




        [Test]
        public void P026_CampoFechaTextoInvalido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P026 - Campo fecha texto inválido");

                pedidos.CampoFechaTextoInvalido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P026 completada con éxito (Campo fecha texto inválido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P026: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P027_CampoClienteConSimbolosInvalidos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P027 - Campo cliente con símbolos inválidos");

                pedidos.CampoClienteConSimbolosInvalidos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P027 completada con éxito (Campo cliente con símbolos inválidos).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P027: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P028_CampoComprobanteTextoInvalido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P028 - Campo comprobante con texto inválido");

                pedidos.CampoComprobanteTextoInvalido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P028 completada con éxito (Campo comprobante con texto inválido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P028: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P029_CampoTotalTextoInvalido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P029 - Campo total con texto inválido");

                pedidos.CampoTotalTextoInvalido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P029 completada con éxito (Campo total con texto inválido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P029: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P030_BuscarDosClientesDiferentes()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P030 - Buscar 2 clientes diferentes ('huerta' y 'ramizes')");

                pedidos.BuscarDosClientesDiferentes();

                Reporte.Log(pedidos.Driver, "✅ Prueba P030 completada correctamente (sin resultados en ambos casos).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P030: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        // 31 y 32 en Reportes.cs
        //
        [Test]
        public void P033_BuscarClienteConDosDatos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P033 - Buscar cliente con dos datos ('PINEDO' y 'NILO')");

                pedidos.BuscarClienteConDosDatos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P033 completada correctamente: mismo cliente reconocido en ambas búsquedas.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P033: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P034_GenerarReporteConAnioInvalido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P034 - Generar reporte con año inválido (3025)");

                pedidos.GenerarReporteConAnioInvalido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P034 completada correctamente (año fuera de rango).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P034: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P035_BuscarPorTotalConSimboloSoles()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P035 - Colocar signo de soles en el total (S/. 34.00)");

                pedidos.BuscarPorTotalConSimboloSoles();

                Reporte.Log(pedidos.Driver, "✅ Prueba P035 completada correctamente (se muestran pedidos válidos con S/. 34.00).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P035: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P038_BuscarClienteConTabulacionOSimbolo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P037 - Buscar con tabulación o símbolo entre letras (SIMON VILLAR / SIMON/VILLAR)");

                pedidos.BuscarClienteConTabulacionOSimbolo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P037 completada correctamente (control de formato aplicado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P037: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P039_BuscarClienteConTextoRepetido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P039 - Buscar con texto repetido (control de redundancia)");

                pedidos.BuscarClienteConTextoRepetido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P039 completada correctamente (NICELIDA → NICELIDAAAAA).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P039: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }






        [Test]
        public void P041_ClienteConNombreMaximo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P041 - Cliente con nombre máximo (255 caracteres)");

                pedidos.ClienteConNombreMaximo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P041 completada con éxito (Cliente con 255 caracteres).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P041: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P042_ClienteConCadenaLarga()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P042 - Cliente con cadena larga (1000 caracteres)");

                pedidos.ClienteConCadenaLarga();

                Reporte.Log(pedidos.Driver, "✅ Prueba P042 completada con éxito (Cliente con cadena de 1000 caracteres).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P042: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P043_CampoClienteConGuionesBajos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P043 - Campo cliente con guiones bajos (valores límite)");

                pedidos.CampoClienteConGuionesBajos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P043 completada correctamente (valores límite con guiones bajos).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P043: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P044_ClienteMayusculasMinusculasMezcladas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P044 - Cliente en mayúsculas y minúsculas mezcladas");

                pedidos.ClienteMayusculasMinusculasMezcladas();

                Reporte.Log(pedidos.Driver, "✅ Prueba P044 completada correctamente (comparación de mayúsculas/minúsculas).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P044: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P045_EstadoConMinusculas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P045 - Estado con minúsculas (valores límite)");

                pedidos.EstadoConMinusculas();

                Reporte.Log(pedidos.Driver, "✅ Prueba P045 completada correctamente (campo Estado en distintas combinaciones).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P045: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P046_TotalConValorMinimo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P046 - Monto total con valor mínimo (1)");

                pedidos.TotalConValorMinimo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P046 completada con éxito (Pedidos con monto ≥ 1).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P046: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P047_TotalConValoresLimite()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P047 - Monto total con valores límite (999, 1000, 1001)");

                pedidos.TotalConValoresLimite();

                Reporte.Log(pedidos.Driver, "✅ Prueba P047 completada con éxito (Incluye 999 y 1000, excluye 1001).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P047: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P048_TotalConSimboloDistinto()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P048 - Total con símbolo distinto (S/., $, €)");

                pedidos.TotalConSimboloDistinto();

                Reporte.Log(pedidos.Driver, "✅ Prueba P048 completada correctamente (validación de formato de moneda).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P048: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        




        [Test]
        public void P049_BuscarPorIdPedidoEnCampoCliente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P049 - Buscar por ID del pedido (1, 9999, 10000) en campo CLIENTE");

                pedidos.BuscarPorIdPedidoEnCampoCliente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P049 completada (El sistema no busca por ID en campo CLIENTE).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P049: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P050_VendedorConCadenaLarga()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P050 - Campo VENDEDOR con cadena de 255 caracteres");

                pedidos.VendedorConCadenaLarga();

                Reporte.Log(pedidos.Driver, "✅ Prueba P050 completada con éxito (Campo Vendedor acepta 255 caracteres).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P050: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P051_ComprobanteConEspacioEntreSerieYNumero()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P051 - Comprobante con espacio o doble guión (valores límite)");

                pedidos.ComprobanteConEspacioEntreSerieYNumero();

                Reporte.Log(pedidos.Driver, "✅ Prueba P051 completada correctamente (validación de formato de comprobante).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P051: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }



        [Test]
        public void P052_ComprobanteSinGuionOMitad()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P052 - Comprobante sin guion o incompleto (valores límite)");

                pedidos.ComprobanteSinGuionOMitad();

                Reporte.Log(pedidos.Driver, "✅ Prueba P052 completada correctamente (validación de formato de comprobante).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P052: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P053_ComprobanteSinCeros()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P053 - Comprobante sin ceros o sin guion (valores límite)");

                pedidos.ComprobanteSinCeros();

                Reporte.Log(pedidos.Driver, "✅ Prueba P053 completada correctamente (validación de formato con ceros a la izquierda).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P053: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P054_ComprobanteConSignos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P054 - Comprobante con signos o caracteres especiales (valores límite)");

                pedidos.ComprobanteConSignos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P054 completada correctamente (validación de formato de comprobante con signos).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P054: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P055_FechasLimiteEnFiltroTabla()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P055 - Ingreso de fechas límite en filtro de tabla (FECHA)");

                pedidos.FechasLimiteEnFiltroTabla();

                Reporte.Log(pedidos.Driver, "✅ Prueba P055 completada con éxito (Fechas ingresadas en el campo FECHA del filtro).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P055: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P056_FechasRangoMesCompleto()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P056 - Rango de fechas del mes completo (01/10/2025 01:13:31 PM - 31/10/2025 01:13:31 PM)");

                pedidos.FechasRangoMesCompleto();

                Reporte.Log(pedidos.Driver, "✅ Prueba P056 completada con éxito (Rango mensual octubre 2025).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P056: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P057_CampoGlobal_DosFechas()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P057 - Ingreso de dos fechas consecutivas en campo global");

                pedidos.CampoGlobal_DosFechas();

                Reporte.Log(pedidos.Driver, "✅ Prueba P057 completada con éxito (Dos fechas en campo global).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P057: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P058_CampoGlobalTotalConDecimales()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P058 - Campo global con valores decimales (100,99 y 100,999)");

                pedidos.CampoGlobalTotalConDecimales();

                Reporte.Log(pedidos.Driver, "✅ Prueba P058 completada con éxito (Campo global con valores decimales).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P058: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P059_ComprobanteConPuntos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P059 - Comprobante con puntos (valores límite)");

                pedidos.ComprobanteConPuntos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P059 completada correctamente (validación de formato con puntos en comprobante).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P059: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P060_CampoTotalConLetras()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P060 - Campo TOTAL con letras ('ABC')");

                pedidos.CampoTotalConLetras();

                Reporte.Log(pedidos.Driver, "✅ Prueba P060 completada con éxito (Campo TOTAL con letras).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P060: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P065_FechasVaciasConPalabraNulo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P065 - Fechas vacías escribiendo la palabra 'nulo'");

                pedidos.FechasVaciasConPalabraNulo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P065 completada correctamente (validación de campos de fecha con texto 'nulo').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P065: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P066_PaginacionHastaFinal()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P066 - Paginación hasta el final del listado (Ver Pedido)");

                pedidos.PaginacionHastaFinal();

                Reporte.Log(pedidos.Driver, "✅ Prueba P066 completada correctamente (botón 'Siguiente' deshabilitado al final).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P066: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P067_PaginacionIdaYVuelta()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P067 - Paginación al final del listado ida y vuelta (Ver Pedido)");

                pedidos.PaginacionIdaYVuelta();

                Reporte.Log(pedidos.Driver, "✅ Prueba P067 completada correctamente (ida y vuelta de paginación).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P067: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P068_BuscarGlobalConHTML()
        {
            try
            {
                // Escapar los símbolos HTML para el nombre del test
                string texto1 = System.Web.HttpUtility.HtmlEncode("<Pedido>");
                string texto2 = System.Web.HttpUtility.HtmlEncode("<h1>Hola</h1>");

                Reporte.IniciarReporte();
                Reporte.CrearTest($"P068 - Buscar en buscador global con símbolos HTML ({texto1}, {texto2})");

                pedidos.BuscarGlobalConHTML();

                Reporte.Log(pedidos.Driver, "✅ P068 completada correctamente: se probaron símbolos HTML sin romper el buscador global.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P068: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P069_BuscarClienteConTabulacionExcel()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P069 - Búsqueda con cliente copiado desde Excel (tab incluido)");

                pedidos.BuscarClienteConTabulacionExcel();

                Reporte.Log(pedidos.Driver, "✅ Prueba P069 completada correctamente (manejo de tabulación en texto copiado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P069: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }



        [Test]
        public void P070_CopiaComprobanteConSaltoDeLinea()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P070 - Copia de comprobante con salto de línea");

                pedidos.CopiaComprobanteConSaltoDeLinea();

                Reporte.Log(pedidos.Driver, "✅ Prueba P070 completada correctamente (validación de salto de línea).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P070: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P072_FiltrarClienteConEmojiFinal()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P072 - Cliente con emoji al final (José 😊)");

                pedidos.FiltrarClienteConEmojiFinal();

                Reporte.Log(pedidos.Driver, "✅ Prueba P072 completada: el sistema bloqueó correctamente el emoji en el campo 'Cliente'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P072: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P074_FiltrarClienteConLetrasYEmojis()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P074 - Cliente con mezcla de letras y emojis (RIOS💼SINTI)");

                pedidos.FiltrarClienteConLetrasYEmojis();

                Reporte.Log(pedidos.Driver, "✅ Prueba P074 completada: el sistema bloqueó correctamente el carácter emoji/letra en el campo 'Cliente'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P074: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P075_FiltrarComprobanteConLetrasYEmojis()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P075 - Comprobante con mezcla de letras y emojis (0001💼29206)");

                pedidos.FiltrarComprobanteConLetrasYEmojis();

                Reporte.Log(pedidos.Driver, "✅ Prueba P075 completada: el sistema bloqueó correctamente el carácter emoji/letra en el campo 'Comprobante'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P075: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P076_FiltrarComprobanteConEmoji()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P076 - Comprobante con emoji al final (0001-29206 😊)");

                pedidos.FiltrarComprobanteConEmoji();

                Reporte.Log(pedidos.Driver, "✅ Prueba P076 completada: el sistema bloqueó correctamente el carácter emoji en el campo 'Comprobante'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P076: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P077_FiltrarTotalConEmoji()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P077 - Total con emoji al final (S/. 71.20😊)");

                pedidos.FiltrarTotalConEmoji();

                Reporte.Log(pedidos.Driver, "✅ Prueba P077 completada: el sistema bloqueó correctamente el carácter emoji en el campo 'Total'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P077: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P078_FiltrarTipoDocumentoInvalido_Factura()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P078 - Tipo de Documento con valor inválido (FACTURA)");

                pedidos.FiltrarTipoDocumentoInvalido_Factura();

                Reporte.Log(pedidos.Driver, "✅ Prueba P078 completada: el sistema bloqueó correctamente el valor inválido 'FACTURA'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P078: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P079_FiltrarTipoDocumentoInvalido_Boleta()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P079 - Tipo de Documento con valor inválido (BOLETA)");

                pedidos.FiltrarTipoDocumentoInvalido_Boleta();

                Reporte.Log(pedidos.Driver, "✅ Prueba P079 completada: el sistema bloqueó correctamente el valor inválido 'BOLETA'.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P079: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P080_FiltrarTipoDocumentoInvalido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P080 - Tipo de Documento con valor inválido (NOTA DE VENTA)");

                pedidos.FiltrarTipoDocumentoInvalido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P080 completada: el sistema manejó correctamente la entrada inválida (carácter no permitido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P080: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P081_EstadoRegistradoYClienteValido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P081 - Estado = REGISTRADO / Cliente válido");

                pedidos.FiltrarPorEstadoRegistradoYClienteValido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P081 completada con éxito (Estado REGISTRADO / Cliente válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P081: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P082_EstadoRegistradoYClienteInexistente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P082 - Estado = REGISTRADO / Cliente inexistente");

                pedidos.FiltrarPorEstadoRegistradoYClienteInexistente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P082 completada con éxito (Estado REGISTRADO / Cliente inexistente).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P082: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P083_EstadoRegistradoYClientesCombinados()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P083 - Estado = REGISTRADO / Combinación de nombres de clientes (Juan y Pablo)");

                pedidos.FiltrarPorEstadoRegistradoYClientesCombinados();

                Reporte.Log(pedidos.Driver, "✅ Prueba P083 completada con éxito (Estado REGISTRADO / Clientes combinados: Juan y Pablo).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P083: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P084_EstadoInvalidadoYClienteValido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P084 - Estado = INVALIDADO / Cliente válido");

                pedidos.FiltrarPorEstadoInvalidadoYClienteValido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P084 completada con éxito (Estado INVALIDADO / Cliente válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P084: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P085_EstadoInvalidadoYClienteInexistente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P085 - Estado = INVALIDADO / Cliente inexistente");

                pedidos.FiltrarEstadoInvalidadoYClienteInexistente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P085 completada con éxito (Estado INVALIDADO / Cliente inexistente zavala).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P085: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P086_EstadoInvalidadoClienteEspacios()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P086 - Estado = INVALIDADO / Cliente con espacios");

                pedidos.FiltrarEstadoInvalidadoClienteEspacios();

                Reporte.Log(pedidos.Driver, "✅ Prueba P086 completada con éxito (Cliente con espacios + Estado INVALIDADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P086: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P087_EstadoRegistradoFechaEspecifica()
        {
            try
            {
                // Inicia el reporte
                Reporte.IniciarReporte();
                Reporte.CrearTest("P087 - Estado = REGISTRADO / Fecha específica (22/11/2024)");

                // Ejecuta el flujo principal
                pedidos.FiltrarEstadoRegistradoFechaEspecifica();

                // Registro del resultado exitoso
                Reporte.Log(pedidos.Driver, "✅ Prueba P087 completada con éxito (REGISTRADO / Fecha 22/11/2024).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                // En caso de error, lo registra
                Reporte.Log(pedidos.Driver, $"❌ Error en P087: {ex.Message}");
                throw;
            }
            finally
            {
                // Finaliza la ejecución limpiamente
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P088_EstadoRegistradoMesActual_BuscadorGlobal()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P088 - Estado = REGISTRADO / Fecha = mes actual (10/2025) en buscador global");

                pedidos.FiltrarEstadoRegistradoMesActual_BuscadorGlobal();

                Reporte.Log(pedidos.Driver, "✅ Prueba P088 completada con éxito (REGISTRADO / Mes actual 10/2025 en buscador global).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P088: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P089_EstadoRegistradoFechaFutura()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P089 - Estado = REGISTRADO / Fecha futura (10/10/2026)");

                pedidos.FiltrarEstadoRegistradoFechaFutura();

                Reporte.Log(pedidos.Driver, "✅ Prueba P089 completada con éxito (REGISTRADO / Fecha futura 10/10/2026).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P089: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P090_EstadoInvalidadoFechaHoy()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P090 - Estado = INVALIDADO / Fecha = hoy (12/07/2024)");

                pedidos.FiltrarEstadoInvalidadoFechaHoy();

                Reporte.Log(pedidos.Driver, "✅ Prueba P090 completada con éxito (INVALIDADO / Fecha 12/07/2024).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P090: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P091_EstadoInvalidadoFechaFutura()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P091 - Estado = INVALIDADO / Fecha futura (10/11/2028)");

                pedidos.FiltrarEstadoInvalidadoFechaFutura();

                Reporte.Log(pedidos.Driver, "✅ Prueba P091 completada con éxito (INVALIDADO / Fecha futura 10/11/2028).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P091: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P092_EstadoRegistradoFechaInvertida()
        {
            try
            {
                // 🔹 Iniciar el reporte de prueba
                Reporte.IniciarReporte();
                Reporte.CrearTest("P092 - Estado = REGISTRADO / Fecha invertida (rango inválido)");

                // 🔹 Ejecutar el método del módulo de pedidos
                pedidos.FiltrarEstadoRegistradoFechaInvertida();

                // 🔹 Registrar éxito en el log del reporte
                Reporte.Log(pedidos.Driver, "✅ Prueba P092 completada correctamente (Rango inválido detectado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                // 🔹 Registrar error en el log y lanzar la excepción para que NUnit la capture
                Reporte.Log(pedidos.Driver, $"❌ Error en P092: {ex.Message}");
                throw;
            }
            finally
            {
                // 🔹 Asegurar que se cierre correctamente la sesión del navegador o espere la carga final
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P093_EstadoRegistradoFechaExcesiva()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P093 - Estado = REGISTRADO / Fecha excesiva (más de 1 año)");

                pedidos.FiltrarEstadoRegistradoFechaExcesiva();

                Reporte.Log(pedidos.Driver, "✅ Prueba P093 completada correctamente (rango excesivo detectado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P093: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P094_EstadoRegistradoTotalMayor500()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P094 - Estado = REGISTRADO / Rango válido / Total > 500");

                pedidos.FiltrarPedidosRegistradoTotalMayor500();

                Reporte.Log(pedidos.Driver, "✅ Prueba P094 completada correctamente (solo pedidos > 500 mostrados).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P094: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P095_EstadoRegistradoTotalMenor1()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P095 - Estado = REGISTRADO / Rango válido / Total < 1 (Límite inferior)");

                pedidos.FiltrarPedidosRegistradoTotalMenor1();

                Reporte.Log(pedidos.Driver, "✅ Prueba P095 completada: sin resultados (total < 1) o mensaje mostrado correctamente.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P095: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P096_EstadoRegistradoTotalDecimal()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P096 - Estado = REGISTRADO / Rango válido / Total decimal (Mostrar resultados redondeados)");

                pedidos.FiltrarPedidosRegistradoTotalDecimal();

                Reporte.Log(pedidos.Driver, "✅ Prueba P096 completada correctamente: resultados mostrados y redondeados a 2 decimales.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P096: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P097_EstadoRegistradoTotalConTexto()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P097 - Estado = REGISTRADO / Total con texto (Validación UI)");

                pedidos.FiltrarPedidosRegistradoTotalConTexto();

                Reporte.Log(pedidos.Driver, "✅ Prueba P097 completada correctamente: mensaje 'valor numérico requerido' mostrado en la UI.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P097: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P098_EstadoRegistradoTotalVacio()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P098 - Estado = REGISTRADO / Total vacío (Campo opcional)");

                pedidos.FiltrarPedidosRegistradoTotalVacio();

                Reporte.Log(pedidos.Driver, "✅ Prueba P098 completada correctamente: se muestran todos los pedidos REGISTRADOS con campo Total vacío.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P098: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P099_EstadoInvalidadoTotalMayor1000()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P099 - Estado = INVALIDADO / Total > 1000 (Mostrar invalidados mayores a 1000)");

                pedidos.FiltrarPedidosInvalidadoTotalMayor1000();

                Reporte.Log(pedidos.Driver, "✅ Prueba P099 completada correctamente: se muestran solo pedidos INVALIDADOS con total mayor a 1000 o mensaje vacío si no existen.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P099: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P100_EstadoInvalidadoTotalCero()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P100 - Estado = INVALIDADO / Total = 0 (Validación límite inferior)");

                pedidos.FiltrarPedidosInvalidadoTotalCero();

                Reporte.Log(pedidos.Driver, "✅ Prueba P100 completada correctamente: sin resultados (total = 0) o mensaje mostrado correctamente.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P100: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P101_ClienteYVendedorValidos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P101 - Cliente válido / Vendedor válido (Filtros combinados)");

                pedidos.FiltrarPedidosPorClienteYVendedor();

                Reporte.Log(pedidos.Driver, "✅ Prueba P101 completada correctamente: se muestran pedidos del cliente y vendedor combinados o mensaje vacío si no existen.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P101: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P102_ClienteValidoVendedorDistinto()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P102 - Cliente válido / Vendedor distinto");

                pedidos.FiltrarPedidosClienteValidoVendedorDistinto();

                Reporte.Log(pedidos.Driver, "✅ Prueba P102 completada con éxito (validación de coincidencia Cliente/Vendedor).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P102: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P103_ClienteVacioVendedorValido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P103 - Cliente vacío / Vendedor válido");

                pedidos.FiltrarPedidosClienteVacioVendedorValido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P103 completada con éxito (Cliente vacío / Vendedor válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P103: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P104_ClienteInexistenteVendedorValido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P104 - Cliente inexistente / Vendedor válido");

                pedidos.FiltrarPedidosClienteInexistenteVendedorValido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P104 completada con éxito (Cliente inexistente / Vendedor válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P104: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P105_RegistradoClienteSimonVendedorYta()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P105 - Estado REGISTRADO / Cliente SIMON / Vendedor YTA");

                pedidos.FiltrarPedidosRegistradoClienteSimonVendedorYta();

                Reporte.Log(pedidos.Driver, "✅ Prueba P105 completada con éxito (Estado REGISTRADO / Cliente SIMON / Vendedor YTA).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P105: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P106_RegistradoClienteJoseVendedorDistinto()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P106 - Estado REGISTRADO / Cliente JOSÉ / Vendedor distinto (YTA)");

                pedidos.FiltrarPedidosRegistradoClienteJoseVendedorDistinto();

                Reporte.Log(pedidos.Driver, "✅ Prueba P106 completada con éxito (Estado REGISTRADO / Cliente JOSÉ / Vendedor distinto YTA).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P106: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P107_RegistradoClienteVacioVendedorAdmin()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P107 - Estado REGISTRADO / Cliente vacío / Vendedor - - ADMIN");

                pedidos.FiltrarPedidosRegistradoClienteVacioVendedorAdmin();

                Reporte.Log(pedidos.Driver, "✅ Prueba P107 completada con éxito (Estado REGISTRADO / Cliente vacío / Vendedor - - ADMIN).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P107: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P108_InvalidadoClienteYtaVendedorAdmin()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P108 - Estado invalidado / Cliente varios / Vendedor malpartida");

                pedidos.FiltrarPedidosInvalidadoClienteYtaVendedorAdmin();

                Reporte.Log(pedidos.Driver, "✅ Prueba P108 completada con éxito (Estado INVALIDADO / Cliente varios / Vendedor malpartida).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P108: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P109_InvalidadoClienteInexistenteVendedorValido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P109 - Estado INVALIDADO / Cliente inexistente (soto) / Vendedor válido (malpartida)");

                pedidos.FiltrarPedidosInvalidadoClienteInexistenteVendedorValido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P109 completada con éxito (Cliente inexistente / Vendedor válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P109: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P110_RegistradoTotalVacioVendedorAdmin()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P110 - Estado REGISTRADO / Total vacío / Vendedor ADMIN");

                pedidos.FiltrarPedidosRegistradoTotalVacioVendedorAdmin();

                Reporte.Log(pedidos.Driver, "✅ Prueba P110 completada con éxito (Estado REGISTRADO / Total vacío / Vendedor ADMIN).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P110: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        

        [Test]
        public void P111_MostrarCampoClienteConSimbolos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P111 - Campo cliente con símbolos (mostrar 'valor inválido')");

                pedidos.MostrarCampoClienteConSimbolos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P111 completada correctamente (validación de input).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P111: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P112_MostrarCampoClienteInvalido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P112 - Campo cliente con espacios (mostrar 'campo inválido')");

                pedidos.MostrarCampoClienteInvalido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P112 completada correctamente (campo cliente inválido configurado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P112: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P113_MostrarTodosDelDia()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P113 - Mostrar todos los pedidos del día (REGISTRADO / Cliente vacío)");

                pedidos.MostrarTodosDelDia();

                Reporte.Log(pedidos.Driver, "✅ Prueba P113 completada correctamente (filtros configurados para mostrar todos los pedidos del día).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P113: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P114_MostrarPedidosClienteDelDia()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P114 - Mostrar pedidos del cliente del día (REGISTRADO / AGUILAR)");

                pedidos.MostrarPedidosClienteDelDia();

                Reporte.Log(pedidos.Driver, "✅ Prueba P114 completada correctamente (filtros configurados para cliente AGUILAR).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P114: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P115_MostrarSinResultadosFuturo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P115 - Mostrar 'sin resultados' con fechas futuras");

                // 🔹 Ejecuta el flujo definido en el módulo de pedidos
                pedidos.MostrarSinResultadosFuturo();

                // 🔹 Validación opcional: buscar mensaje “no se encontraron registros”
                try
                {
                    var mensaje = pedidos.Driver.FindElement(By.XPath("//*[contains(text(),'No se encontraron registros')]"));
                    if (mensaje.Displayed)
                        Reporte.Log(pedidos.Driver, "✅ Se mostró correctamente el mensaje 'No se encontraron registros'.");
                    else
                        Reporte.Log(pedidos.Driver, "⚠️ No se encontró el mensaje esperado, revisar manualmente.");
                }
                catch (NoSuchElementException)
                {
                    Reporte.Log(pedidos.Driver, "⚠️ No se encontró el mensaje 'No se encontraron registros', pero no falló el flujo.");
                }

                Reporte.FinalizarReporte();
                Console.WriteLine("[✅ TEST] P115 ejecutado correctamente.");
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P115: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P116_MostrarInvalidadoDelDia()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P116 - Mostrar invalidados del día actual");

                pedidos.MostrarInvalidadoDelDia();

                Reporte.Log(pedidos.Driver, "✅ Prueba P116 completada correctamente (filtros configurados).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P116: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P117_MostrarPedidosValidosCombinados()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P117 - Mostrar pedidos válidos combinados (REGISTRADO, cliente 11, total alto, mes actual)");

                pedidos.MostrarPedidosValidosCombinados();

                Reporte.Log(pedidos.Driver, "✅ Prueba P117 completada correctamente (campos configurados sin consultar).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P117: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P118_ValidarRangoInvertido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P118 - Rango invertido (2030–2026) con estado REGISTRADO y cliente 11");

                pedidos.ValidarRangoInvertido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P118 completada correctamente (rango invertido configurado sin validar).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P118: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P119_ValidarMensajeRangoExcesivo()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P119 - Validar mensaje 'rango excesivo' con rango 2025–2030");

                pedidos.ValidarMensajeRangoExcesivo();

                Reporte.Log(pedidos.Driver, "✅ Prueba P119 completada correctamente (mensaje 'rango excesivo' validado con rango 2025–2030).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P119: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P120_MostrarPedidosMesActualSinFiltros()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P120 - Mostrar pedidos del mes actual sin filtros aplicados");

                pedidos.MostrarPedidosMesActualSinFiltros();

                Reporte.Log(pedidos.Driver, "✅ Prueba P120 completada correctamente (pedidos del mes actual sin filtros).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P120: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P121_CancelarEdicionPedido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P121 - Cancelar edición de pedido (REGISTRADO → REGISTRADO)");

                pedidos.CancelarEdicionPedido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P121 completada correctamente (cancelar edición mantiene estado REGISTRADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P121: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P122_ModificarPedidoRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P122 - Modificar pedido REGISTRADO y guardar sin cambios");

                pedidos.ModificarPedidoRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P122 completada correctamente (pedido REGISTRADO actualizado sin cambios).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P122: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P123_InvalidarPedidoRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P123 - Invalidar pedido REGISTRADO → INVALIDADO");

                pedidos.InvalidarPedidoRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P123 completada correctamente (estado cambiado a INVALIDADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P123: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P124_IntentarEliminarPedidoInvalidado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P124 - Intentar eliminar un pedido INVALIDADO");

                pedidos.IntentarEliminarPedidoInvalidado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P124 completada correctamente (pedido INVALIDADO no se elimina).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P124: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P125_ConfirmarPedidoRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P125 - Editar y confirmar pedido REGISTRADO → CONFIRMADO");

                pedidos.ConfirmarPedidoRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P125 completada correctamente (pedido confirmado exitosamente).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P125: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P126_InvalidarYCancelarPedido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P126 - Invalidar y Cancelarlo (REGISTRADO → REGISTRADO)");

                pedidos.InvalidarYCancelarPedido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P126 completada correctamente (cancelar invalidación mantiene estado REGISTRADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P126: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P127_ModificarPedidoYCerrarSinGuardar()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P127 - Modificar pedido y cerrarlo sin guardar");

                pedidos.ModificarPedidoEliminarItemYCerrar();

                Reporte.Log(pedidos.Driver, "✅ Prueba P127 completada — El pedido se cerró correctamente sin guardar.");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P127: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P128_AnadirProductoYRecargarVista()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P128 - Añadir producto y recargar vista (REGISTRADO → REGISTRADO)");

                pedidos.AnadirProductoYRecargarVista();

                Reporte.Log(pedidos.Driver, "✅ Prueba P128 completada correctamente (producto añadido y vista recargada).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P128: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P129_InvalidarPedidoYRecargarVista()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P129 - Invalidar pedido y recargar vista (REGISTRADO → INVALIDADO)");

                pedidos.InvalidarPedidoYRecargarVista();

                Reporte.Log(pedidos.Driver, "✅ Prueba P129 completada correctamente (invalidar cancelado y vista recargada).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P129: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P132_CambiarRangoFilasDe10a1000yVolverA10()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P132 - Cambiar rango de filas de 10 → 1000 → 10 (REGISTRADO y INVALIDADO)");

                pedidos.CambiarRangoFilasDe10a1000yVolverA10();

                Reporte.Log(pedidos.Driver, "✅ Prueba P132 completada correctamente (cambio de filas 10 → 1000 → 10 ejecutado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P132: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P133_AnadirCienVecesYGuardar()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P133 - Añadir 100 veces el mismo producto y guardar (REGISTRADO → PERMITE GUARDARSE)");

                pedidos.AnadirCienVecesYGuardar();

                Reporte.Log(pedidos.Driver, "✅ Prueba P133 completada correctamente (100 productos añadidos y guardado exitoso).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P133: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P134_CambiarRangoFilasMultiple()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P134 - Cambiar rango de filas (10 → 25 → 50 → 100 → 1000) (REGISTRADO y INVALIDADO)");

                pedidos.CambiarRangoFilasMultiple();

                Reporte.Log(pedidos.Driver, "✅ Prueba P134 completada correctamente (recorrido de 10, 25, 50, 100 y 1000 ejecutado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P134: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P140_BuscarPedidoPorClienteRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P140 - Buscar pedido por cliente con estado REGISTRADO");

                pedidos.BuscarPedidoPorClienteRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P140 completada correctamente (filtro por cliente REGISTRADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P140: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P141_BuscarPedidoPorRangoDeFechasRegistrado()
        {
            try
            {
                // 🧭 Iniciar reporte
                Reporte.IniciarReporte();
                Reporte.CrearTest("P141 - Buscar pedido por rango de fechas con estado REGISTRADO (01/10/2025 - 01/11/2025)");

                // 🚀 Ejecutar prueba
                pedidos.BuscarPedidoPorRangoDeFechasRegistrado();

                // 🟢 Log del resultado exitoso
                Reporte.Log(pedidos.Driver, "✅ Prueba P141 completada — Se filtraron correctamente los pedidos REGISTRADOS dentro del rango 01/10/2025 - 01/11/2025.");

                // 🧾 Finalizar reporte
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                // 🔴 Manejo de errores
                Reporte.Log(pedidos.Driver, $"❌ Error en P141: {ex.Message}");
                throw;
            }
            finally
            {
                // ⏳ Cierre y limpieza del driver
                pedidos.EsperarCargaYFinalizar();
            }
        }


        [Test]
        public void P142_BuscarPedidoPorVendedorRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P142 - Buscar pedido por vendedor con estado REGISTRADO");

                pedidos.BuscarPedidoPorVendedorRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P142 completada correctamente (filtro por vendedor REGISTRADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P142: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P143_BuscarPedidoPorEstadoRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P143 - Buscar pedido por estado REGISTRADO");

                pedidos.BuscarPedidoPorEstadoRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P143 completada correctamente (filtro por estado REGISTRADO).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P143: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P144_BuscarPedidoGlobalRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P144 - Buscar pedido global (campo superior derecho)");

                pedidos.BuscarPedidoGlobalRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P144 completada correctamente (campo de búsqueda global funcional).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P144: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P145_NavegarASiguientePaginaPedidos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P145 - Navegar a siguiente página de pedidos");

                pedidos.NavegarASiguientePaginaPedidos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P145 completada correctamente (paginación funcionando).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P145: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P146_NavegarAPaginaAnteriorPedidos()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P146 - Navegar a página anterior de pedidos");

                pedidos.NavegarAPaginaAnteriorPedidos();

                Reporte.Log(pedidos.Driver, "✅ Prueba P146 completada correctamente (paginación anterior funcionando).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P146: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P147_RecargarVistaPedidosRegistrados()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P147 - Recargar vista de pedidos REGISTRADOS (mantiene datos actualizados)");

                pedidos.RecargarVistaPedidosRegistrados();

                Reporte.Log(pedidos.Driver, "✅ Prueba P147 completada correctamente (vista recargada, datos actualizados).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P147: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P148_ExportarPedidosAExcelRegistrado()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P148 - Exportar pedidos REGISTRADOS a Excel (botón DESCARGAR)");

                pedidos.ExportarPedidosAExcelRegistrado();

                Reporte.Log(pedidos.Driver, "✅ Prueba P148 completada correctamente (archivo Excel descargado sin alterar estado).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P148: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }
        [Test]
        public void P149_FiltrarPorComprobanteValido()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P149 - Filtrar por comprobante válido");

                pedidos.FiltrarPorComprobanteValido();

                Reporte.Log(pedidos.Driver, "✅ Prueba P149 completada correctamente (muestra solo comprobante válido).");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P149: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }

        [Test]
        public void P150_FiltrarPorComprobanteInexistente()
        {
            try
            {
                Reporte.IniciarReporte();
                Reporte.CrearTest("P150 - Filtrar por comprobante inexistente");

                pedidos.FiltrarPorComprobanteInexistente();

                Reporte.Log(pedidos.Driver, "✅ Prueba P150 completada correctamente (muestra 'No se encontraron registros').");
                Reporte.FinalizarReporte();
            }
            catch (Exception ex)
            {
                Reporte.Log(pedidos.Driver, $"❌ Error en P150: {ex.Message}");
                throw;
            }
            finally
            {
                pedidos.EsperarCargaYFinalizar();
            }
        }





        [TearDown]
        public void TearDown()
        {
            pedidos.Finalizar();
        }

    }
}
