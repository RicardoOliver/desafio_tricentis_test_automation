using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TricentisAutomacao.Utils;

namespace TricentisAutomacao
{
    /// <summary>
    /// Classe auxiliar para execução dos testes via NUnit
    /// </summary>
    [TestFixture]
    public class RunTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            // Inicializa o relatório
            GeradorRelatorio.InicializarRelatorio();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Finaliza o relatório
            GeradorRelatorio.FinalizarRelatorio();
        }

        // Não possui método Main, apenas métodos de teste que serão executados pelo NUnit
    }
}
