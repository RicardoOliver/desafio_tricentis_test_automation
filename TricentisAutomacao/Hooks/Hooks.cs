using System;
using System.IO;
using AventStack.ExtentReports;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TricentisAutomacao.Utils;

namespace TricentisAutomacao.Hooks
{
    [Binding]
    public class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine("Iniciando execução dos testes...");
            
            // Inicializa o relatório
            GeradorRelatorio.InicializarRelatorio();
            Console.WriteLine("Relatório inicializado com sucesso.");
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine($"Iniciando cenário: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // Inicializa o driver
                ConfiguracaoDriver.InicializarDriver();
                
                // Cria um novo cenário no relatório
                GeradorRelatorio.CriarCenario(scenarioContext.ScenarioInfo.Title);
                GeradorRelatorio.RegistrarPasso(Status.Info, $"Iniciando cenário: {scenarioContext.ScenarioInfo.Title}");
                
                Console.WriteLine("Driver e cenário inicializados com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao inicializar cenário: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            try
            {
                // Registra o resultado do passo no relatório
                var stepInfo = scenarioContext.StepContext.StepInfo;
                var stepText = $"{stepInfo.StepDefinitionType} {stepInfo.Text}";
                
                switch (scenarioContext.StepContext.Status)
                {
                    case ScenarioExecutionStatus.OK:
                        GeradorRelatorio.RegistrarPasso(Status.Pass, $"Passo concluído: {stepText}");
                        break;
                    case ScenarioExecutionStatus.StepDefinitionPending:
                        GeradorRelatorio.RegistrarPasso(Status.Warning, $"Passo pendente: {stepText}");
                        break;
                    case ScenarioExecutionStatus.UndefinedStep:
                        GeradorRelatorio.RegistrarPasso(Status.Warning, $"Passo não definido: {stepText}");
                        break;
                    case ScenarioExecutionStatus.BindingError:
                    case ScenarioExecutionStatus.TestError:
                        var error = scenarioContext.TestError;
                        GeradorRelatorio.RegistrarPasso(Status.Fail, $"Erro no passo: {stepText}. Erro: {error?.Message}");
                        // Captura tela em caso de erro
                        GeradorRelatorio.CapturarTela();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao processar passo: {ex.Message}");
            }
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            try
            {
                Console.WriteLine($"Finalizando cenário: {scenarioContext.ScenarioInfo.Title}");
                
                // Captura uma evidência final do cenário
                string evidencia = GeradorRelatorio.CapturarTela();
                if (!string.IsNullOrEmpty(evidencia))
                {
                    GeradorRelatorio.RegistrarPasso(
                        scenarioContext.TestError == null ? Status.Pass : Status.Fail,
                        $"Finalização do cenário: {scenarioContext.ScenarioInfo.Title}",
                        true);
                }
                
                // Encerra o driver
                ConfiguracaoDriver.EncerrarDriver();
                Console.WriteLine("Driver encerrado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao finalizar cenário: {ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                Console.WriteLine("Finalizando execução dos testes...");
                
                // Finaliza o relatório
                GeradorRelatorio.FinalizarRelatorio();
                Console.WriteLine("Relatório finalizado com sucesso.");
                
                // Exibe o caminho dos relatórios e evidências
                string diretorioBase = AppDomain.CurrentDomain.BaseDirectory;
                string diretorioRelatorio = Path.Combine(diretorioBase, "Relatorios");
                string diretorioEvidencias = Path.Combine(diretorioBase, "Evidencias");
                
                Console.WriteLine($"Relatórios disponíveis em: {diretorioRelatorio}");
                Console.WriteLine($"Evidências disponíveis em: {diretorioEvidencias}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao finalizar execução dos testes: {ex.Message}");
            }
        }
    }
}
