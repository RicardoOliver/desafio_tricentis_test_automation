using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TricentisAutomacao.Utils;

namespace TricentisAutomacao.Pages
{
    /// <summary>
    /// Classe base para todas as páginas do sistema
    /// </summary>
    public class PaginaBase
    {
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;

        /// <summary>
        /// Construtor da classe base
        /// </summary>
        public PaginaBase()
        {
            _driver = ConfiguracaoDriver.Driver;
            _wait = ConfiguracaoDriver.Wait;
        }

        /// <summary>
        /// Navega para a URL especificada
        /// </summary>
        /// <param name="url">URL de destino</param>
        public void Navegar(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Aguarda até que o elemento esteja visível e clica nele
        /// </summary>
        /// <param name="localizador">Localizador do elemento</param>
        protected void ClicarElemento(By localizador)
        {
            try
            {
                var elemento = _wait.Until(ExpectedConditions.ElementToBeClickable(localizador));
                elemento.Click();
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível clicar no elemento {localizador}: {ex.Message}");
            }
        }

        /// <summary>
        /// Preenche um campo de texto com o valor especificado
        /// </summary>
        /// <param name="localizador">Localizador do elemento</param>
        /// <param name="texto">Texto a ser inserido</param>
        protected void PreencherCampo(By localizador, string texto)
        {
            try
            {
                var elemento = _wait.Until(ExpectedConditions.ElementToBeClickable(localizador));
                elemento.Clear();
                elemento.SendKeys(texto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível preencher o campo {localizador}: {ex.Message}");
            }
        }

        /// <summary>
        /// Seleciona uma opção em um dropdown pelo texto visível
        /// </summary>
        /// <param name="localizador">Localizador do elemento select</param>
        /// <param name="texto">Texto da opção a ser selecionada</param>
        protected void SelecionarOpcaoPorTexto(By localizador, string texto)
        {
            try
            {
                var elemento = _wait.Until(ExpectedConditions.ElementToBeClickable(localizador));
                var selectElement = new SelectElement(elemento);
                selectElement.SelectByText(texto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível selecionar a opção {texto} no dropdown {localizador}: {ex.Message}");
            }
        }

        /// <summary>
        /// Seleciona uma opção em um dropdown pelo valor
        /// </summary>
        /// <param name="localizador">Localizador do elemento select</param>
        /// <param name="valor">Valor da opção a ser selecionada</param>
        protected void SelecionarOpcaoPorValor(By localizador, string valor)
        {
            try
            {
                var elemento = _wait.Until(ExpectedConditions.ElementToBeClickable(localizador));
                var selectElement = new SelectElement(elemento);
                selectElement.SelectByValue(valor);
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível selecionar o valor {valor} no dropdown {localizador}: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se um elemento está visível na página
        /// </summary>
        /// <param name="localizador">Localizador do elemento</param>
        /// <returns>True se o elemento estiver visível, False caso contrário</returns>
        protected bool ElementoEstaVisivel(By localizador)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(localizador)).Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém o texto de um elemento
        /// </summary>
        /// <param name="localizador">Localizador do elemento</param>
        /// <returns>Texto do elemento</returns>
        protected string ObterTextoElemento(By localizador)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(localizador)).Text;
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível obter o texto do elemento {localizador}: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se um elemento contém uma classe CSS específica
        /// </summary>
        /// <param name="localizador">Localizador do elemento</param>
        /// <param name="classe">Nome da classe CSS</param>
        /// <returns>True se o elemento contiver a classe, False caso contrário</returns>
        protected bool ElementoContemClasse(By localizador, string classe)
        {
            try
            {
                var elemento = _wait.Until(ExpectedConditions.ElementExists(localizador));
                string classes = elemento.GetAttribute("class");
                return classes.Contains(classe);
            }
            catch
            {
                return false;
            }
        }
    }
}
