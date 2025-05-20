using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Runtime.InteropServices;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TricentisAutomacao.Utils
{
    /// <summary>
    /// Classe responsável pela configuração e gerenciamento do driver do navegador
    /// </summary>
    public class ConfiguracaoDriver
    {
        private static IWebDriver? _driver;
        private static WebDriverWait? _wait;

        /// <summary>
        /// Inicializa o driver do Chrome com as configurações necessárias
        /// </summary>
        /// <returns>Instância do WebDriver</returns>
        public static IWebDriver InicializarDriver()
        {
            if (_driver == null)
            {
                try
                {
                    Console.WriteLine("Configurando ChromeDriver...");

                    // Caminho para a pasta 'drivers'
                    string driverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drivers");
                    Console.WriteLine($"Procurando ChromeDriver em: {driverPath}");

                    // Determina o nome correto do executável dependendo do sistema operacional
                    string driverFileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? "chromedriver.exe"
                        : "chromedriver";

                    string chromeDriverExe = Path.Combine(driverPath, driverFileName);

                    if (File.Exists(chromeDriverExe))
                    {
                        Console.WriteLine($"ChromeDriver encontrado: {chromeDriverExe}");
                    }
                    else
                    {
                        Console.WriteLine($"AVISO: ChromeDriver não encontrado em {chromeDriverExe}");
                        Console.WriteLine("Tentando baixar automaticamente usando WebDriverManager...");

                        try
                        {
                            new DriverManager().SetUpDriver(new ChromeConfig());
                            Console.WriteLine("ChromeDriver baixado com sucesso.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao baixar o ChromeDriver: {ex.Message}");
                        }
                    }

                    var chromeOptions = new ChromeOptions();

                    // Modo headless se a variável de ambiente "CHROME_HEADLESS" estiver definida
                    bool headless = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHROME_HEADLESS"));

                    if (headless)
                    {
                        Console.WriteLine("Executando em modo headless.");
                        chromeOptions.AddArgument("--headless=new"); // Melhor compatibilidade
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    else
                    {
                        chromeOptions.AddArgument("--start-maximized");
                    }

                    // Configurações adicionais para evitar erros em ambientes CI/CD
                    chromeOptions.AddArgument("--disable-notifications");
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");

                    // Cria serviço apontando para o caminho do driver
                    var service = ChromeDriverService.CreateDefaultService(driverPath);
                    service.SuppressInitialDiagnosticInformation = true;

                    Console.WriteLine("Iniciando ChromeDriver...");
                    _driver = new ChromeDriver(service, chromeOptions);
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

                    Console.WriteLine("Chrome WebDriver iniciado com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao inicializar o ChromeDriver:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.ToString());

                    throw new Exception(
                        "Falha ao iniciar o ChromeDriver. Verifique se o Google Chrome está instalado e se o chromedriver está presente na pasta 'drivers'.",
                        ex
                    );
                }
            }

            return _driver;
        }

        /// <summary>
        /// Obtém a instância atual do driver
        /// </summary>
        public static IWebDriver Driver => _driver ?? throw new NullReferenceException("O driver ainda não foi inicializado.");

        /// <summary>
        /// Obtém a instância de espera explícita
        /// </summary>
        public static WebDriverWait Wait => _wait ?? throw new NullReferenceException("O WebDriverWait ainda não foi inicializado.");

        /// <summary>
        /// Encerra o driver e libera os recursos
        /// </summary>
        public static void EncerrarDriver()
        {
            if (_driver != null)
            {
                Console.WriteLine("Encerrando o ChromeDriver...");
                _driver.Quit();
                _driver = null;
                _wait = null;
                Console.WriteLine("ChromeDriver encerrado com sucesso.");
            }
        }
    }
}
