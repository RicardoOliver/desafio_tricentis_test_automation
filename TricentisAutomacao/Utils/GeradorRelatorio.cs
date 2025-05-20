using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using System;
using System.IO;

namespace TricentisAutomacao.Utils
{
    /// <summary>
    /// Classe responsável pela geração de relatórios e captura de evidências
    /// </summary>
    public class GeradorRelatorio
    {
        private static ExtentReports? _extent;
        private static ExtentTest? _cenarioAtual;
        
        // Alterado para usar caminhos relativos ao diretório do projeto
        private static string _diretorioBase = AppDomain.CurrentDomain.BaseDirectory;
        private static string _diretorioRelatorio = Path.Combine(_diretorioBase, "Relatorios");
        private static string _diretorioEvidencias = Path.Combine(_diretorioBase, "Evidencias");

        /// <summary>
        /// Inicializa o relatório ExtentReports
        /// </summary>
        public static void InicializarRelatorio()
        {
            if (_extent == null)
            {
                try
                {
                    Console.WriteLine("Inicializando sistema de relatórios...");
                    
                    // Cria diretórios se não existirem
                    if (!Directory.Exists(_diretorioRelatorio))
                    {
                        Console.WriteLine($"Criando diretório de relatórios: {_diretorioRelatorio}");
                        Directory.CreateDirectory(_diretorioRelatorio);
                    }

                    if (!Directory.Exists(_diretorioEvidencias))
                    {
                        Console.WriteLine($"Criando diretório de evidências: {_diretorioEvidencias}");
                        Directory.CreateDirectory(_diretorioEvidencias);
                    }

                    // Configura o relatório HTML com caminho absoluto
                    string caminhoRelatorio = Path.Combine(_diretorioRelatorio, $"Relatorio_{DateTime.Now:yyyyMMdd_HHmmss}.html");
                    Console.WriteLine($"Configurando relatório em: {caminhoRelatorio}");
                    
                    var reporter = new ExtentHtmlReporter(caminhoRelatorio);
                    reporter.Config.Theme = Theme.Dark;
                    reporter.Config.DocumentTitle = "Relatório de Testes - Tricentis";
                    reporter.Config.ReportName = "Automação de Testes - Formulários Tricentis";
                    reporter.Config.EnableTimeline = true;

                    _extent = new ExtentReports();
                    _extent.AttachReporter(reporter);
                    _extent.AddSystemInfo("Ambiente", "Teste");
                    _extent.AddSystemInfo("Navegador", "Chrome");
                    _extent.AddSystemInfo("Sistema Operacional", Environment.OSVersion.ToString());
                    
                    Console.WriteLine("Sistema de relatórios inicializado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERRO ao inicializar relatório: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }
        }

        /// <summary>
        /// Cria um novo cenário no relatório
        /// </summary>
        /// <param name="nomeCenario">Nome do cenário</param>
        public static void CriarCenario(string nomeCenario)
        {
            try
            {
                if (_extent != null)
                {
                    Console.WriteLine($"Criando cenário no relatório: {nomeCenario}");
                    _cenarioAtual = _extent.CreateTest(nomeCenario);
                }
                else
                {
                    Console.WriteLine("AVISO: Tentativa de criar cenário, mas o relatório não foi inicializado.");
                    InicializarRelatorio();
                    if (_extent != null)
                    {
                        _cenarioAtual = _extent.CreateTest(nomeCenario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao criar cenário: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra um passo no cenário atual
        /// </summary>
        /// <param name="status">Status do passo</param>
        /// <param name="descricao">Descrição do passo</param>
        /// <param name="capturarTela">Indica se deve capturar a tela</param>
        public static void RegistrarPasso(Status status, string descricao, bool capturarTela = false)
        {
            try
            {
                if (_cenarioAtual != null)
                {
                    if (capturarTela)
                    {
                        string caminhoEvidencia = CapturarTela();
                        if (!string.IsNullOrEmpty(caminhoEvidencia))
                        {
                            // Usa caminho relativo para a imagem no relatório
                            string nomeArquivo = Path.GetFileName(caminhoEvidencia);
                            _cenarioAtual.Log(status, descricao + $" <br><img src='../Evidencias/{nomeArquivo}' width='800px'>");
                            Console.WriteLine($"Passo registrado com evidência: {descricao}");
                        }
                        else
                        {
                            _cenarioAtual.Log(status, descricao + " (Falha ao capturar evidência)");
                            Console.WriteLine($"Passo registrado sem evidência (falha): {descricao}");
                        }
                    }
                    else
                    {
                        _cenarioAtual.Log(status, descricao);
                        Console.WriteLine($"Passo registrado: {descricao}");
                    }
                }
                else
                {
                    Console.WriteLine($"AVISO: Tentativa de registrar passo, mas nenhum cenário foi criado: {descricao}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao registrar passo: {ex.Message}");
            }
        }

        /// <summary>
        /// Captura a tela atual e salva como evidência
        /// </summary>
        /// <returns>Caminho do arquivo de evidência</returns>
        public static string CapturarTela()
        {
            try
            {
                // Verifica se o diretório existe, se não, cria
                if (!Directory.Exists(_diretorioEvidencias))
                {
                    Directory.CreateDirectory(_diretorioEvidencias);
                }

                string nomeArquivo = $"Evidencia_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string caminhoCompleto = Path.Combine(_diretorioEvidencias, nomeArquivo);

                var driver = ConfiguracaoDriver.Driver;
                if (driver != null)
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile(caminhoCompleto, ScreenshotImageFormat.Png);
                    Console.WriteLine($"Screenshot capturado: {caminhoCompleto}");
                    return caminhoCompleto;
                }
                else
                {
                    Console.WriteLine("ERRO: Driver não inicializado ao tentar capturar tela");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao capturar tela: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Finaliza o relatório e gera o arquivo HTML
        /// </summary>
        public static void FinalizarRelatorio()
        {
            try
            {
                if (_extent != null)
                {
                    Console.WriteLine("Finalizando relatório...");
                    _extent.Flush();
                    Console.WriteLine($"Relatório gerado com sucesso em: {_diretorioRelatorio}");
                    
                    // Exibe os arquivos gerados
                    if (Directory.Exists(_diretorioRelatorio))
                    {
                        var arquivos = Directory.GetFiles(_diretorioRelatorio, "*.html");
                        Console.WriteLine($"Arquivos de relatório gerados ({arquivos.Length}):");
                        foreach (var arquivo in arquivos)
                        {
                            Console.WriteLine($" - {arquivo}");
                        }
                    }
                    
                    if (Directory.Exists(_diretorioEvidencias))
                    {
                        var arquivos = Directory.GetFiles(_diretorioEvidencias, "*.png");
                        Console.WriteLine($"Arquivos de evidência gerados ({arquivos.Length}):");
                        foreach (var arquivo in arquivos)
                        {
                            Console.WriteLine($" - {arquivo}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("AVISO: Tentativa de finalizar relatório, mas o relatório não foi inicializado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao finalizar relatório: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
