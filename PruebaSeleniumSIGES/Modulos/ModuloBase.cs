using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace PruebaSeleniumSIGES.Modulos
{
    public class ModuloBase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;



        protected readonly string BaseUrl = "http://161.132.67.82:31096/";
        protected readonly string Email = "admin@plazafer.com";
        protected readonly string Password = "calidad";

        public void Inicializar(bool headless = true)
        {
            ChromeOptions options = new ChromeOptions();

            if (headless)
                options.AddArgument("--headless=new");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1366,768");

            // ⏳ Aumentar tiempo de sesión general
            options.AddExcludedArgument("enable-automation"); // opcional: quita el banner "Chrome is being controlled..."

            driver = new ChromeDriver(options);

            // 🔹 Esperas globales más largas
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));

            Console.WriteLine("[INFO] Esperas configuradas: Implicit=30s, Explicit=45s, PageLoad=60s.");
        }


        public void Login()
        {
            driver.Navigate().GoToUrl(BaseUrl);

            // Ingresar usuario y contraseña
            driver.FindElement(By.Id("Email")).SendKeys(Email);
            driver.FindElement(By.Id("Password")).SendKeys(Password);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Esperar botón "Aceptar"
            var aceptar = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Aceptar']")));
            aceptar.Click();

            Console.WriteLine("[OK] Login exitoso.");
        }

        public void IrAMenuPedido()
        {
            var menuPedido = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[normalize-space()='Pedido']")));
            menuPedido.Click();
            Console.WriteLine("[OK] Entró al menú Pedido.");
            Thread.Sleep(800);
        }

        public void IrAVerPedido()
        {
            var verPedido = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[@href='/Pedido/Index' or normalize-space()='Ver Pedido']")));
            verPedido.Click();
            Console.WriteLine("[OK] Entró a Ver Pedido.");
            Thread.Sleep(1500);
        }
        public void IrAReportes()
        {
            var enlaceReportes = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[@href='/PedidoReportes/Index']")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", enlaceReportes);
            Thread.Sleep(300);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", enlaceReportes);

            Console.WriteLine("[OK] Entró a Reportes.");
            Thread.Sleep(1500);
        }


        public void EsperarCargaYFinalizar()
        {
            try
            {
                // Esperar hasta que el documento esté completamente cargado
                var js = (IJavaScriptExecutor)driver;
                for (int i = 0; i < 10; i++) // máximo 10 segundos de espera
                {
                    string estado = js.ExecuteScript("return document.readyState").ToString();
                    if (estado.Equals("complete"))
                        break;
                    Thread.Sleep(1000);
                }

                Console.WriteLine("[INFO] Página cargada completamente. Cerrando navegador...");
                driver.Quit();
                Console.WriteLine("[INFO] Navegador cerrado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[WARN] Error al finalizar: " + ex.Message);
            }
        }


        public IWebDriver Driver => driver;

        public void Finalizar()
        {
            driver.Quit();
            Console.WriteLine("[INFO] Navegador cerrado correctamente.");
        }
    }
}
