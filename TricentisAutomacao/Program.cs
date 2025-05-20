using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TricentisAutomacao
{
    /// <summary>
    /// Classe principal para execução do projeto
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Método principal - ponto de entrada da aplicação
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Iniciando execução dos testes automatizados ===");

            try
            {
                // Obtém o diretório do executável
                string diretorioBase = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine($"Diretório base: {diretorioBase}");

                // Caminho para o relatório e evidências
                string diretorioRelatorio = Path.Combine(diretorioBase, "Relatorios");
                string diretorioEvidencias = Path.Combine(diretorioBase, "Evidencias");

                // Cria os diretórios se não existirem
                if (!Directory.Exists(diretorioRelatorio))
                {
                    Directory.CreateDirectory(diretorioRelatorio);
                    Console.WriteLine($"Diretório de relatórios criado: {diretorioRelatorio}");
                }

                if (!Directory.Exists(diretorioEvidencias))
                {
                    Directory.CreateDirectory(diretorioEvidencias);
                    Console.WriteLine($"Diretório de evidências criado: {diretorioEvidencias}");
                }

                // Caminho do assembly atual
                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                Console.WriteLine($"Executando testes do assembly: {assemblyPath}");

                // Executa os testes usando o NUnit Console Runner
                var processo = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"test \"{assemblyPath}\" --no-build --verbosity normal",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = false
                    }
                };

                Console.WriteLine("Iniciando execução dos testes...");
                processo.Start();

                // Lê a saída em tempo real
                while (!processo.StandardOutput.EndOfStream)
                {
                    string line = processo.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                }

                // Lê possíveis erros
                string errors = processo.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine("ERROS:");
                    Console.WriteLine(errors);
                }

                processo.WaitForExit();
                Console.WriteLine($"Testes concluídos com código de saída: {processo.ExitCode}");

                // Verifica se foram gerados relatórios
                if (Directory.Exists(diretorioRelatorio))
                {
                    string[] arquivosRelatorio = Directory.GetFiles(diretorioRelatorio, "*.html");
                    if (arquivosRelatorio.Length > 0)
                    {
                        // Ordena os arquivos por data de criação (mais recente primeiro)
                        Array.Sort(arquivosRelatorio, (a, b) => File.GetCreationTime(b).CompareTo(File.GetCreationTime(a)));

                        Console.WriteLine($"Relatório gerado: {arquivosRelatorio[0]}");

                        // Tenta abrir o relatório no navegador padrão
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = arquivosRelatorio[0],
                                UseShellExecute = true
                            });
                            Console.WriteLine("Relatório aberto no navegador padrão.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Não foi possível abrir o relatório automaticamente: {ex.Message}");
                            Console.WriteLine($"Você pode abrir manualmente o arquivo: {arquivosRelatorio[0]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhum arquivo de relatório foi gerado.");
                    }
                }
                else
                {
                    Console.WriteLine($"O diretório de relatórios não existe: {diretorioRelatorio}");
                }

                // Verifica se foram geradas evidências
                if (Directory.Exists(diretorioEvidencias))
                {
                    string[] arquivosEvidencia = Directory.GetFiles(diretorioEvidencias, "*.png");
                    Console.WriteLine($"Evidências geradas: {arquivosEvidencia.Length} arquivos");

                    if (arquivosEvidencia.Length > 0)
                    {
                        Console.WriteLine("Exemplos de evidências:");
                        for (int i = 0; i < Math.Min(3, arquivosEvidencia.Length); i++)
                        {
                            Console.WriteLine($" - {arquivosEvidencia[i]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma evidência foi gerada.");
                    }
                }
                else
                {
                    Console.WriteLine($"O diretório de evidências não existe: {diretorioEvidencias}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao executar os testes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== Execução concluída ===");
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
