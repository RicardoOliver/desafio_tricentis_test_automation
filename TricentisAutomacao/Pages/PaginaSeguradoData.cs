using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AventStack.ExtentReports;
using TricentisAutomacao.Utils;

namespace TricentisAutomacao.Pages
{
    /// <summary>
    /// Classe que representa a página de dados do segurado
    /// </summary>
    public class PaginaSeguradoData : PaginaBase
    {
        // Localizadores dos elementos da página
        private readonly By _inputFirstName = By.Id("firstname");
        private readonly By _inputLastName = By.Id("lastname");
        private readonly By _inputDateOfBirth = By.Id("birthdate");
        private readonly By _radioGenderMale = By.XPath("//input[@id='gendermale']/following-sibling::span");
        private readonly By _radioGenderFemale = By.XPath("//input[@id='genderfemale']/following-sibling::span");
        private readonly By _inputStreetAddress = By.Id("streetaddress");
        private readonly By _selectCountry = By.Id("country");
        private readonly By _inputZipCode = By.Id("zipcode");
        private readonly By _inputCity = By.Id("city");
        private readonly By _selectOccupation = By.Id("occupation");
        private readonly By _checkboxSpeeding = By.XPath("//input[@id='speeding']/following-sibling::span");
        private readonly By _checkboxBungeeJumping = By.XPath("//input[@id='bungeejumping']/following-sibling::span");
        private readonly By _checkboxCliffDiving = By.XPath("//input[@id='cliffdiving']/following-sibling::span");
        private readonly By _checkboxSkydiving = By.XPath("//input[@id='skydiving']/following-sibling::span");
        private readonly By _checkboxOther = By.XPath("//input[@id='other']/following-sibling::span");
        private readonly By _inputWebsite = By.Id("website");
        private readonly By _inputPicture = By.Id("picture");
        private readonly By _btnNext = By.Id("nextenterproductdata");

        /// <summary>
        /// Verifica se a página de dados do segurado está carregada
        /// </summary>
        /// <returns>True se a página estiver carregada, False caso contrário</returns>
        public bool PaginaEstaCarregada()
        {
            bool paginaCarregada = ElementoEstaVisivel(_inputFirstName);
            
            if (paginaCarregada)
            {
                GeradorRelatorio.RegistrarPasso(Status.Pass, "Página de dados do segurado carregada com sucesso", true);
            }
            else
            {
                GeradorRelatorio.RegistrarPasso(Status.Fail, "Página de dados do segurado não foi carregada", true);
            }
            
            return paginaCarregada;
        }

        /// <summary>
        /// Preenche o formulário de dados do segurado
        /// </summary>
        /// <param name="dadosSegurado">Dicionário com os dados do segurado</param>
        public void PreencherFormulario(Dictionary<string, string> dadosSegurado)
        {
            // Preenche o nome
            PreencherCampo(_inputFirstName, dadosSegurado["Nome"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Nome preenchido: {dadosSegurado["Nome"]}");

            // Preenche o sobrenome
            PreencherCampo(_inputLastName, dadosSegurado["Sobrenome"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Sobrenome preenchido: {dadosSegurado["Sobrenome"]}");

            // Preenche a data de nascimento
            PreencherCampo(_inputDateOfBirth, dadosSegurado["DataNascimento"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Data de nascimento: {dadosSegurado["DataNascimento"]}");

            // Seleciona o gênero
            if (dadosSegurado["Genero"] == "Masculino")
            {
                ClicarElemento(_radioGenderMale);
                GeradorRelatorio.RegistrarPasso(Status.Info, "Gênero: Masculino");
            }
            else
            {
                ClicarElemento(_radioGenderFemale);
                GeradorRelatorio.RegistrarPasso(Status.Info, "Gênero: Feminino");
            }

            // Preenche o endereço
            PreencherCampo(_inputStreetAddress, dadosSegurado["Endereco"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Endereço: {dadosSegurado["Endereco"]}");

            // Seleciona o país
            SelecionarOpcaoPorTexto(_selectCountry, dadosSegurado["Pais"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"País: {dadosSegurado["Pais"]}");

            // Preenche o CEP
            PreencherCampo(_inputZipCode, dadosSegurado["CEP"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"CEP: {dadosSegurado["CEP"]}");

            // Preenche a cidade
            PreencherCampo(_inputCity, dadosSegurado["Cidade"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Cidade: {dadosSegurado["Cidade"]}");

            // Seleciona a ocupação
            SelecionarOpcaoPorTexto(_selectOccupation, dadosSegurado["Ocupacao"]);
            GeradorRelatorio.RegistrarPasso(Status.Info, $"Ocupação: {dadosSegurado["Ocupacao"]}");

            // Seleciona os hobbies
            if (dadosSegurado.ContainsKey("Hobbies"))
            {
                string[] hobbies = dadosSegurado["Hobbies"].Split(',');
                
                foreach (string hobby in hobbies)
                {
                    switch (hobby.Trim())
                    {
                        case "Speeding":
                            ClicarElemento(_checkboxSpeeding);
                            GeradorRelatorio.RegistrarPasso(Status.Info, "Hobby selecionado: Speeding");
                            break;
                        case "Bungee Jumping":
                            ClicarElemento(_checkboxBungeeJumping);
                            GeradorRelatorio.RegistrarPasso(Status.Info, "Hobby selecionado: Bungee Jumping");
                            break;
                        case "Cliff Diving":
                            ClicarElemento(_checkboxCliffDiving);
                            GeradorRelatorio.RegistrarPasso(Status.Info, "Hobby selecionado: Cliff Diving");
                            break;
                        case "Skydiving":
                            ClicarElemento(_checkboxSkydiving);
                            GeradorRelatorio.RegistrarPasso(Status.Info, "Hobby selecionado: Skydiving");
                            break;
                        case "Other":
                            ClicarElemento(_checkboxOther);
                            GeradorRelatorio.RegistrarPasso(Status.Info, "Hobby selecionado: Other");
                            break;
                    }
                }
            }

            // Preenche o site
            if (dadosSegurado.ContainsKey("Website"))
            {
                PreencherCampo(_inputWebsite, dadosSegurado["Website"]);
                GeradorRelatorio.RegistrarPasso(Status.Info, $"Website: {dadosSegurado["Website"]}");
            }

            GeradorRelatorio.RegistrarPasso(Status.Pass, "Formulário de dados do segurado preenchido com sucesso", true);
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
