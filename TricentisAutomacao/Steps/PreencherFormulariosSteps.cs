using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TricentisAutomacao.Pages;
using TricentisAutomacao.Utils;
using AventStack.ExtentReports;

namespace TricentisAutomacao.Steps
{
    [Binding]
    public class PreencherFormulariosSteps
    {
        private readonly PaginaVeiculoData _paginaVeiculoData;
        private readonly PaginaSeguradoData _paginaSeguradoData;
        private bool _validacaoCapacidadeCilindro;
        private bool _validacaoDesempenhoMotor;

        public PreencherFormulariosSteps()
        {
            _paginaVeiculoData = new PaginaVeiculoData();
            _paginaSeguradoData = new PaginaSeguradoData();
        }

        [Given(@"que estou na página inicial do Tricentis")]
        public void DadoQueEstouNaPaginaInicialDoTricentis()
        {
            _paginaVeiculoData.AbrirPagina();
        }

        [When(@"tento preencher o campo de capacidade do cilindro com um valor inválido ""(.*)""")]
        public void QuandoTentoPreencherOCampoDeCapacidadeDoCilindroComUmValorInvalido(string valor)
        {
            _validacaoCapacidadeCilindro = !_paginaVeiculoData.ValidarCapacidadeCilindro(valor);
        }

        [Then(@"devo ver uma mensagem de erro para o campo de capacidade do cilindro")]
        public void EntaoDevoVerUmaMensagemDeErroParaOCampoDeCapacidadeDoCilindro()
        {
            Assert.IsTrue(_validacaoCapacidadeCilindro, "Não foi exibida mensagem de erro para o campo de capacidade do cilindro");
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Validação do campo de capacidade do cilindro realizada com sucesso", true);
        }

        [When(@"tento preencher o campo de desempenho do motor com um valor inválido ""(.*)""")]
        public void QuandoTentoPreencherOCampoDeDesempenhoDoMotorComUmValorInvalido(string valor)
        {
            _validacaoDesempenhoMotor = !_paginaVeiculoData.ValidarDesempenhoMotor(valor);
        }

        [Then(@"devo ver uma mensagem de erro para o campo de desempenho do motor")]
        public void EntaoDevoVerUmaMensagemDeErroParaOCampoDeDesempenhoDoMotor()
        {
            Assert.IsTrue(_validacaoDesempenhoMotor, "Não foi exibida mensagem de erro para o campo de desempenho do motor");
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Validação do campo de desempenho do motor realizada com sucesso", true);
        }

        [When(@"preencho o formulário de dados do veículo corretamente")]
        public void QuandoPreenchoOFormularioDeDadosDoVeiculoCorretamente()
        {
            var dadosVeiculo = new Dictionary<string, string>
            {
                { "Marca", "BMW" },
                { "Modelo", "Scooter" },
                { "CapacidadeCilindro", "125" },
                { "DesempenhoMotor", "100" },
                { "DataFabricacao", "01/01/2022" },
                { "NumeroAssentos", "2" },
                { "DirecaoDireita", "Não" },
                { "TipoCombustivel", "Petrol" },
                { "PrecoLista", "10000" },
                { "NumeroPlaca", "ABC1234" },
                { "QuilometragemAnual", "10000" }
            };

            _paginaVeiculoData.PreencherFormulario(dadosVeiculo);
        }

        [When(@"clico no botão Next na página de dados do veículo")]
        public void QuandoClicoNoBotaoNextNaPaginaDeDadosDoVeiculo()
        {
            _paginaVeiculoData.ClicarBotaoNext();
        }

        [Then(@"devo ser direcionado para a página de dados do segurado")]
        public void EntaoDevoSerDirecionadoParaAPaginaDeDadosDoSegurado()
        {
            bool paginaCarregada = _paginaSeguradoData.PaginaEstaCarregada();
            Assert.IsTrue(paginaCarregada, "A página de dados do segurado não foi carregada");
        }

        [Given(@"que estou na página de dados do segurado")]
        public void DadoQueEstouNaPaginaDeDadosDoSegurado()
        {
            // Primeiro abre a página inicial
            _paginaVeiculoData.AbrirPagina();
            
            // Preenche o formulário de dados do veículo
            var dadosVeiculo = new Dictionary<string, string>
            {
                { "Marca", "BMW" },
                { "Modelo", "Scooter" },
                { "CapacidadeCilindro", "125" },
                { "DesempenhoMotor", "100" },
                { "DataFabricacao", "01/01/2022" },
                { "NumeroAssentos", "2" },
                { "DirecaoDireita", "Não" },
                { "TipoCombustivel", "Petrol" },
                { "PrecoLista", "10000" },
                { "NumeroPlaca", "ABC1234" },
                { "QuilometragemAnual", "10000" }
            };
            
            _paginaVeiculoData.PreencherFormulario(dadosVeiculo);
            
            // Clica no botão Next
            _paginaVeiculoData.ClicarBotaoNext();
            
            // Verifica se a página de dados do segurado foi carregada
            bool paginaCarregada = _paginaSeguradoData.PaginaEstaCarregada();
            Assert.IsTrue(paginaCarregada, "A página de dados do segurado não foi carregada");
        }

        [When(@"preencho o formulário de dados do segurado corretamente")]
        public void QuandoPreenchoOFormularioDeDadosDoSeguradoCorretamente()
        {
            var dadosSegurado = new Dictionary<string, string>
            {
                { "Nome", "João" },
                { "Sobrenome", "Silva" },
                { "DataNascimento", "01/01/1990" },
                { "Genero", "Masculino" },
                { "Endereco", "Rua Exemplo, 123" },
                { "Pais", "Brazil" },
                { "CEP", "12345" },
                { "Cidade", "São Paulo" },
                { "Ocupacao", "Employee" },
                { "Hobbies", "Speeding,Skydiving" },
                { "Website", "www.exemplo.com.br" }
            };

            _paginaSeguradoData.PreencherFormulario(dadosSegurado);
        }

        [When(@"clico no botão Next na página de dados do segurado")]
        public void QuandoClicoNoBotaoNextNaPaginaDeDadosDoSegurado()
        {
            _paginaSeguradoData.ClicarBotaoNext();
        }

        [Then(@"devo ser direcionado para a próxima página")]
        public void EntaoDevoSerDirecionadoParaAProximaPagina()
        {
            // Verifica se o título da página contém "Enter Product Data"
            string tituloAtual = ConfiguracaoDriver.Driver.Title;
            Assert.IsTrue(tituloAtual.Contains("Enter Product Data"), $"A página não foi redirecionada corretamente. Título atual: {tituloAtual}");
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Redirecionado para a página de dados do produto com sucesso", true);
        }
    }
}
