#language: pt-BR
Funcionalidade: Preenchimento de formulários no site Tricentis
  Como um usuário do sistema Tricentis
  Eu quero preencher os formulários de seguro de veículo
  Para que eu possa obter uma cotação de seguro

Cenário: Validar campos obrigatórios no formulário de dados do veículo
  Dado que estou na página inicial do Tricentis
  Quando tento preencher o campo de capacidade do cilindro com um valor inválido "0"
  Então devo ver uma mensagem de erro para o campo de capacidade do cilindro
  Quando tento preencher o campo de desempenho do motor com um valor inválido "0"
  Então devo ver uma mensagem de erro para o campo de desempenho do motor

Cenário: Preencher formulário de dados do veículo
  Dado que estou na página inicial do Tricentis
  Quando preencho o formulário de dados do veículo corretamente
  E clico no botão Next na página de dados do veículo
  Então devo ser direcionado para a página de dados do segurado

Cenário: Preencher formulário de dados do segurado
  Dado que estou na página de dados do segurado
  Quando preencho o formulário de dados do segurado corretamente
  E clico no botão Next na página de dados do segurado
  Então devo ser direcionado para a próxima página
