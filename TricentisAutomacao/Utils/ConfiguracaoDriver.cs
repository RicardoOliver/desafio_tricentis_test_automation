using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
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

                    // Caminho para o chromedriver.exe na pasta drivers
                    string driverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drivers");
                    Console.WriteLine($"Procurando ChromeDriver em: {driverPath}");

                    // Verifica se o arquivo chromedriver.exe existe
                    string chromeDriverExe = Path.Combine(driverPath, "chromedriver.exe");
                    if (File.Exists(chromeDriverExe))
                    {
                        Console.WriteLine($"ChromeDriver encontrado: {chromeDriverExe}");
                    }
                    else
                    {
                        Console.WriteLine($"AVISO: ChromeDriver não encontrado em {chromeDriverExe}");

                        // Tenta usar o WebDriverManager para baixar o ChromeDriver automaticamente
                        try
                        {
                            // Usa a sintaxe correta do WebDriverManager
                            new DriverManager().SetUpDriver(new ChromeConfig());
                            Console.WriteLine("ChromeDriver baixado automaticamente pelo WebDriverManager");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao baixar ChromeDriver: {ex.Message}");
                        }
                    }

                    var chromeOptions = new ChromeOptions();

                    // Verifica se deve executar em modo headless (sem interface gráfica)
                    bool headless = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHROME_HEADLESS"));

                    if (headless)
                    {
                        Console.WriteLine("Executando Chrome em modo headless");
                        chromeOptions.AddArgument("--headless");
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    else
                    {
                        chromeOptions.AddArgument("--start-maximized");
                    }

                    chromeOptions.AddArgument("--disable-notifications");

                    // Configurações adicionais para lidar com problemas de compatibilidade
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");

                    // Configura o serviço do ChromeDriver com o caminho para a pasta drivers
                    var service = ChromeDriverService.CreateDefaultService(driverPath);
                    service.SuppressInitialDiagnosticInformation = true;

                    Console.WriteLine($"Iniciando o Chrome WebDriver usando o driver em: {driverPath}");
                    _driver = new ChromeDriver(service, chromeOptions);
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                    Console.WriteLine("Chrome WebDriver iniciado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao inicializar o ChromeDriver: {ex.Message}");
                    Console.WriteLine($"Detalhes: {ex.ToString()}");
                    throw new Exception("Não foi possível inicializar o ChromeDriver. Verifique se o Chrome está instalado corretamente e se o chromedriver.exe está na pasta 'drivers'.", ex);
                }
            }

            return _driver;
        }

        /// <summary>
        /// Obtém a instância atual do driver
        /// </summary>
        public static IWebDriver Driver => _driver ?? throw new NullReferenceException("O driver não foi inicializado.");

        /// <summary>
        /// Obtém a instância de espera explícita
        /// </summary>
        public static WebDriverWait Wait => _wait ?? throw new NullReferenceException("O wait não foi inicializado.");

        /// <summary>
        /// Encerra o driver e libera os recursos
        /// </summary>
        public static void EncerrarDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
                _wait = null;
            }
        }
    }
}