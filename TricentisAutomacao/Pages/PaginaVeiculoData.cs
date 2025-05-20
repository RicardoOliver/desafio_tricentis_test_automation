using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AventStack.ExtentReports;
using TricentisAutomacao.Utils;

namespace TricentisAutomacao.Pages
{
    /// <summary>
    /// Classe que representa a página de dados do veículo
    /// </summary>
    public class PaginaVeiculoData : PaginaBase
    {
        // Localizadores dos elementos da página
        private readonly By _selectMake = By.Id("make");
        private readonly By _selectModel = By.Id("model");
        private readonly By _inputCylinderCapacity = By.Id("cylindercapacity");
        private readonly By _inputEnginePerformance = By.Id("engineperformance");
        private readonly By _inputDateOfManufacture = By.Id("dateofmanufacture");
        private readonly By _selectNumberOfSeats = By.Id("numberofseats");
        private readonly By _radioRightHandDriveYes = By.XPath("//input[@id='righthanddriveyes']/following-sibling::span");
        private readonly By _radioRightHandDriveNo = By.XPath("//input[@id='righthanddriveno']/following-sibling::span");
        private readonly By _selectNumberOfSeatsMotorcycle = By.Id("numberofseatsmotorcycle");
        private readonly By _selectFuelType = By.Id("fuel");
        private readonly By _inputPayload = By.Id("payload");
        private readonly By _inputTotalWeight = By.Id("totalweight");
        private readonly By _inputListPrice = By.Id("listprice");
        private readonly By _inputLicensePlateNumber = By.Id("licenseplatenumber");
        private readonly By _inputAnnualMileage = By.Id("annualmileage");
        private readonly By _btnNext = By.Id("nextenterinsurantdata");
        
        // Mensagens de erro
        private readonly By _msgErroCylinderCapacity = By.XPath("//input[@id='cylindercapacity']/following-sibling::span[@class='error']");
        private readonly By _msgErroEnginePerformance = By.XPath("//input[@id='engineperformance']/following-sibling::span[@class='error']");

        /// <summary>
        /// Abre a página inicial do aplicativo
        /// </summary>
        public void AbrirPagina()
        {
            Navegar("http://sampleapp.tricentis.com/101/app.php");
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Página inicial aberta com sucesso", true);
        }

        /// <summary>
        /// Preenche o formulário de dados do veículo
        /// </summary>
        /// <param name="dadosVeiculo">Dicionário com os dados do veículo</param>
        public void PreencherFormulario(Dictionary<string, string> dadosVeiculo)
        {
            // Seleciona a marca do veículo
            SelecionarOpcaoPorTexto(_selectMake, dadosVeiculo["Marca"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Marca selecionada: {dadosVeiculo["Marca"]}");

            // Seleciona o modelo do veículo
            SelecionarOpcaoPorTexto(_selectModel, dadosVeiculo["Modelo"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Modelo selecionado: {dadosVeiculo["Modelo"]}");

            // Preenche a capacidade do cilindro
            PreencherCampo(_inputCylinderCapacity, dadosVeiculo["CapacidadeCilindro"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Capacidade do cilindro: {dadosVeiculo["CapacidadeCilindro"]}");

            // Preenche o desempenho do motor
            PreencherCampo(_inputEnginePerformance, dadosVeiculo["DesempenhoMotor"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Desempenho do motor: {dadosVeiculo["DesempenhoMotor"]}");

            // Preenche a data de fabricação
            PreencherCampo(_inputDateOfManufacture, dadosVeiculo["DataFabricacao"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Data de fabricação: {dadosVeiculo["DataFabricacao"]}");

            // Seleciona o número de assentos
            SelecionarOpcaoPorTexto(_selectNumberOfSeats, dadosVeiculo["NumeroAssentos"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Número de assentos: {dadosVeiculo["NumeroAssentos"]}");

            // Seleciona se é direção à direita
            if (dadosVeiculo["DirecaoDireita"] == "Sim")
            {
                ClicarElemento(_radioRightHandDriveYes);
                GeradorRelatorio.RegistrarPasso(Status.Info, "Direção à direita: Sim");
            }
            else
            {
                ClicarElemento(_radioRightHandDriveNo);
                GeradorRelatorio.RegistrarPasso(Status.Info, "Direção à direita: Não");
            }

            // Seleciona o tipo de combustível
            SelecionarOpcaoPorTexto(_selectFuelType, dadosVeiculo["TipoCombustivel"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Tipo de combustível: {dadosVeiculo["TipoCombustivel"]}");

            // Preenche o preço de lista
            PreencherCampo(_inputListPrice, dadosVeiculo["PrecoLista"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Preço de lista: {dadosVeiculo["PrecoLista"]}");

            // Preenche o número da placa
            PreencherCampo(_inputLicensePlateNumber, dadosVeiculo["NumeroPlaca"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Número da placa: {dadosVeiculo["NumeroPlaca"]}");

            // Preenche a quilometragem anual
            PreencherCampo(_inputAnnualMileage, dadosVeiculo["QuilometragemAnual"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Quilometragem anual: {dadosVeiculo["QuilometragemAnual"]}");

            GeradorRelatorio.RegistrarPasso(Status.Pass, "Formulário de dados do veículo preenchido com sucesso", true);
        }

        /// <summary>
        /// Valida o campo de capacidade do cilindro
        /// </summary>
        /// <param name="valor">Valor a ser validado</param>
        /// <returns>True se o campo for válido, False caso contrário</returns>
        public bool ValidarCapacidadeCilindro(string valor)
        {
            PreencherCampo(_inputCylinderCapacity, valor);
            
            // Clica em outro campo para acionar a validação
            ClicarElemento(_selectMake);
            
            bool temErro = ElementoEstaVisivel(_msgErroCylinderCapacity);
            
            if (temErro)
            {
                string mensagemErro = ObterTextoElemento(_msgErroCylinderCapacity);
                GeradorRelatorio.RegistrarPasso(Status.Warning, $"Validação do campo Capacidade do Cilindro: {mensagemErro}", true);
                return false;
            }
            
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Campo Capacidade do Cilindro validado com sucesso", true);
            return true;
        }

        /// <summary>
        /// Valida o campo de desempenho do motor
        /// </summary>
        /// <param name="valor">Valor a ser validado</param>
        /// <returns>True se o campo for válido, False caso contrário</returns>
        public bool ValidarDesempenhoMotor(string valor)
        {
            PreencherCampo(_inputEnginePerformance, valor);
            
            // Clica em outro campo para acionar a validação
            ClicarElemento(_selectMake);
            
            bool temErro = ElementoEstaVisivel(_msgErroEnginePerformance);
            
            if (temErro)
            {
                string mensagemErro = ObterTextoElemento(_msgErroEnginePerformance);
                GeradorRelatorio.RegistrarPasso(Status.Warning, $"Validação do campo Desempenho do Motor: {mensagemErro}", true);
                return false;
            }
            
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Campo Desempenho do Motor validado com sucesso", true);
            return true;
        }

        /// <summary>
        /// Clica no botão Next para avançar para a próxima página
        /// </summary>
        public void ClicarBotaoNext()
        {
            ClicarElemento(_btnNext);
            GeradorRelatorio.RegistrarPasso(Status.Pass, "Botão Next clicado com sucesso", true);
        }
    }
}
