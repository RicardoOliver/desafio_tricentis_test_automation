using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO.Compression;

namespace TricentisAutomacao.Utils
{
    /// <summary>
    /// Classe responsável por gerenciar o download e configuração do WebDriver
    /// </summary>
    public static class WebDriverManager
    {
        private static readonly string DriverDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebDrivers");
        
        /// <summary>
        /// Configura o ChromeDriver compatível com a versão do Chrome instalada
        /// </summary>
        /// <returns>Caminho para o executável do ChromeDriver</returns>
        public static async Task<string> SetupChromeDriver()
        {
            // Cria o diretório de drivers se não existir
            if (!Directory.Exists(DriverDirectory))
            {
                Directory.CreateDirectory(DriverDirectory);
            }
            
            // Obtém a versão do Chrome instalada
            string chromeVersion = GetChromeVersion();
            Console.WriteLine($"Versão do Chrome detectada: {chromeVersion}");
            
            // Obtém a versão principal do Chrome (ex: de "136.0.7103.114" para "136")
            string majorVersion = chromeVersion.Split('.')[0];
            
            // Caminho onde o ChromeDriver será salvo
            string driverPath = Path.Combine(DriverDirectory, "chromedriver.exe");
            
            // Verifica se já existe um ChromeDriver compatível
            if (File.Exists(driverPath))
            {
                try
                {
                    // Tenta verificar a versão do ChromeDriver existente
                    string driverVersion = GetChromeDriverVersion(driverPath);
                    Console.WriteLine($"ChromeDriver existente: {driverVersion}");
                    
                    // Se a versão principal for compatível, usa o driver existente
                    if (driverVersion.StartsWith(majorVersion))
                    {
                        Console.WriteLine("ChromeDriver existente é compatível com a versão do Chrome.");
                        return driverPath;
                    }
                    
                    Console.WriteLine("ChromeDriver existente não é compatível. Baixando versão compatível...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao verificar versão do ChromeDriver: {ex.Message}");
                    Console.WriteLine("Baixando nova versão do ChromeDriver...");
                }
            }
            
            // Baixa o ChromeDriver compatível
            try
            {
                await DownloadChromeDriver(majorVersion, driverPath);
                Console.WriteLine($"ChromeDriver baixado com sucesso para: {driverPath}");
                return driverPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao baixar ChromeDriver: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Obtém a versão do Chrome instalada no sistema
        /// </summary>
        private static string GetChromeVersion()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                    if (!File.Exists(chromePath))
                    {
                        chromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                    }
                    
                    if (File.Exists(chromePath))
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(chromePath);
                        return versionInfo.FileVersion ?? "Unknown";
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "google-chrome",
                            Arguments = "--version",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    // Formato típico: "Google Chrome 136.0.7103.114"
                    if (output.Contains("Google Chrome"))
                    {
                        return output.Replace("Google Chrome", "").Trim();
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome",
                            Arguments = "--version",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    if (output.Contains("Google Chrome"))
                    {
                        return output.Replace("Google Chrome", "").Trim();
                    }
                }
                
                // Se não conseguir detectar, retorna uma versão recente
                return "136.0.0.0";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter versão do Chrome: {ex.Message}");
                return "136.0.0.0"; // Versão padrão se não conseguir detectar
            }
        }
        
        /// <summary>
        /// Obtém a versão do ChromeDriver a partir do executável
        /// </summary>
        private static string GetChromeDriverVersion(string driverPath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = driverPath,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            // Formato típico: "ChromeDriver 136.0.7103.94"
            if (output.Contains("ChromeDriver"))
            {
                return output.Replace("ChromeDriver", "").Trim();
            }
            
            return "Unknown";
        }
        
        /// <summary>
        /// Baixa o ChromeDriver compatível com a versão do Chrome
        /// </summary>
        private static async Task DownloadChromeDriver(string majorVersion, string destinationPath)
        {
            // URL para obter a versão mais recente do ChromeDriver para a versão principal do Chrome
            string latestVersionUrl = $"https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{majorVersion}";
            
            using (var httpClient = new HttpClient())
            {
                // Obtém a versão mais recente do ChromeDriver para a versão do Chrome
                string latestVersion = await httpClient.GetStringAsync(latestVersionUrl);
                Console.WriteLine($"Versão mais recente do ChromeDriver para Chrome {majorVersion}: {latestVersion}");
                
                // Determina a plataforma para download
                string platform;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    platform = "win32";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    platform = "linux64";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    platform = RuntimeInformation.OSArchitecture == Architecture.Arm64 ? "mac_arm64" : "mac64";
                }
                else
                {
                    throw new PlatformNotSupportedException("Plataforma não suportada para download do ChromeDriver");
                }
                
                // URL para download do ChromeDriver
                string downloadUrl = $"https://chromedriver.storage.googleapis.com/{latestVersion}/chromedriver_{platform}.zip";
                Console.WriteLine($"URL de download: {downloadUrl}");
                
                // Baixa o arquivo zip do ChromeDriver
                string zipPath = Path.Combine(DriverDirectory, "chromedriver.zip");
                using (var response = await httpClient.GetAsync(downloadUrl))
                {
                    response.EnsureSuccessStatusCode();
                    using (var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                }
                
                // Extrai o arquivo zip
                ZipFile.ExtractToDirectory(zipPath, DriverDirectory, true);
                
                // Remove o arquivo zip após a extração
                File.Delete(zipPath);
                
                // Verifica se o ChromeDriver foi extraído corretamente
                if (!File.Exists(destinationPath))
                {
                    // Em algumas plataformas, o nome do arquivo pode ser diferente
                    string extractedDriver = Directory.GetFiles(DriverDirectory, "chromedriver*").FirstOrDefault();
                    if (extractedDriver != null && extractedDriver != destinationPath)
                    {
                        File.Move(extractedDriver, destinationPath, true);
                    }
                    else
                    {
                        throw new FileNotFoundException("ChromeDriver não foi extraído corretamente");
                    }
                }
                
                // Torna o arquivo executável no Linux/Mac
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var chmodProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "chmod",
                            Arguments = $"+x {destinationPath}",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    
                    chmodProcess.Start();
                    chmodProcess.WaitForExit();
                }
            }
        }
    }
}
