# Matriz de Casos de Teste - Tricentis Sample App

## Visão Geral

Este documento apresenta a matriz de casos de teste para o projeto de automação do [Tricentis Sample App](http://sampleapp.tricentis.com/101/app.php). A matriz fornece uma visão estruturada de todos os casos de teste implementados, permitindo o acompanhamento do progresso e da cobertura dos testes.

## Matriz de Casos de Teste

A tabela abaixo apresenta uma visão geral dos casos de teste implementados, classificados por tipo e prioridade:

| ID | Descrição | Tipo | Prioridade | Status |
|-----|-----|-----|-----|-----|
| TC-001 | Validar campo "Cylinder Capacity" com valor inválido (0) | Negativo | Alta | Implementado |
| TC-002 | Validar campo "Engine Performance" com valor inválido (0) | Negativo | Alta | Implementado |
| TC-003 | Preencher formulário "Enter Vehicle Data" com dados válidos | Positivo | Alta | Implementado |
| TC-004 | Navegar do formulário "Enter Vehicle Data" para "Enter Insurant Data" | Positivo | Alta | Implementado |
| TC-005 | Preencher formulário "Enter Insurant Data" com dados válidos | Positivo | Alta | Implementado |
| TC-006 | Navegar do formulário "Enter Insurant Data" para "Enter Product Data" | Positivo | Alta | Implementado |

## Legenda

### Tipos de Teste
- **Positivo**: Testes que verificam o comportamento esperado do sistema quando utilizado corretamente (fluxo feliz).
- **Negativo**: Testes que verificam como o sistema lida com entradas inválidas ou condições de erro.

### Níveis de Prioridade
- **Alta**: Testes críticos que verificam funcionalidades essenciais do sistema.
- **Média**: Testes importantes, mas não críticos para a funcionalidade básica.
- **Baixa**: Testes de funcionalidades secundárias ou casos de borda.

### Status de Implementação
- **Implementado**: O caso de teste foi completamente implementado e está funcional.
- **Em Desenvolvimento**: O caso de teste está sendo implementado.
- **Planejado**: O caso de teste foi definido, mas ainda não foi implementado.
- **Bloqueado**: A implementação está bloqueada por alguma dependência ou problema.

## Resumo de Cobertura

| Tipo de Teste | Total | Implementados | Pendentes |
|---------------|-------|---------------|-----------|
| Positivo      | 4     | 4             | 0         |
| Negativo      | 2     | 2             | 0         |
| **Total**     | **6** | **6**         | **0**     |

## Progresso da Implementação

![Progresso](https://progress-bar.dev/100/?title=Implementação&width=400)

## Casos de Teste Futuros

Abaixo estão listados casos de teste adicionais que podem ser implementados para aumentar a cobertura:

| ID | Descrição | Tipo | Prioridade | Status |
|-----|-----|-----|-----|-----|
| TC-007 | Preencher formulário "Enter Product Data" com dados válidos | Positivo | Alta | Planejado |
| TC-008 | Preencher formulário "Select Price Option" | Positivo | Alta | Planejado |
| TC-009 | Completar processo de cotação até a confirmação | Positivo | Alta | Planejado |
| TC-010 | Validar campos obrigatórios não preenchidos em "Enter Vehicle Data" | Negativo | Média | Planejado |
| TC-011 | Validar campos obrigatórios não preenchidos em "Enter Insurant Data" | Negativo | Média | Planejado |
| TC-012 | Testar diferentes combinações de marca/modelo de veículo | Positivo | Média | Planejado |

## Como Usar Esta Matriz

Esta matriz de casos de teste deve ser usada para:

1. **Acompanhar o Progresso**: Monitorar quais casos de teste foram implementados e quais ainda estão pendentes.
2. **Priorizar o Desenvolvimento**: Focar primeiro na implementação dos casos de teste de alta prioridade.
3. **Avaliar a Cobertura**: Garantir um equilíbrio adequado entre testes positivos e negativos.
4. **Reportar Status**: Comunicar o status da automação de testes para stakeholders.

## Manutenção da Matriz

A matriz de casos de teste deve ser atualizada sempre que:

- Novos casos de teste forem identificados
- O status de implementação de um caso de teste mudar
- A prioridade de um caso de teste for reavaliada
- Um caso de teste for modificado ou removido

## Rastreabilidade

Cada caso de teste nesta matriz pode ser rastreado até sua implementação correspondente no código-fonte:

- Os casos de teste TC-001 e TC-002 são implementados no cenário "Validar campos obrigatórios no formulário de dados do veículo"
- Os casos de teste TC-003 e TC-004 são implementados no cenário "Preencher formulário de dados do veículo"
- Os casos de teste TC-005 e TC-006 são implementados no cenário "Preencher formulário de dados do segurado"

---

*Última atualização: 19 de maio de 2025*
```

## Explicação do Documento de Matriz de Casos de Teste

Criei um documento markdown dedicado (TEST_MATRIX.md) que apresenta a matriz de casos de teste para o projeto de automação do Tricentis Sample App. Este documento vai além da simples tabela solicitada, fornecendo um recurso completo para gerenciamento e acompanhamento dos casos de teste.

### Estrutura e Conteúdo

O documento está organizado em seções que fornecem uma visão abrangente dos casos de teste:

1. **Visão Geral**
   - Introdução explicando o propósito do documento
   - Contextualização sobre o projeto de automação

2. **Matriz de Casos de Teste**
   - A tabela principal com os 6 casos de teste implementados
   - Informações sobre ID, descrição, tipo, prioridade e status

3. **Legenda**
   - Explicações detalhadas sobre os tipos de teste (positivo/negativo)
   - Definições dos níveis de prioridade (alta/média/baixa)
   - Descrições dos possíveis status de implementação

4. **Resumo de Cobertura**
   - Tabela resumida mostrando a distribuição dos testes por tipo
   - Contagem de testes implementados e pendentes

5. **Progresso da Implementação**
   - Representação visual do progresso usando uma barra de progresso

6. **Casos de Teste Futuros**
   - Tabela com casos de teste adicionais planejados
   - Informações sobre prioridade e escopo para expansão futura

7. **Como Usar Esta Matriz**
   - Orientações sobre como utilizar o documento para acompanhamento
   - Sugestões para priorização e avaliação de cobertura

8. **Manutenção da Matriz**
   - Diretrizes sobre quando e como atualizar o documento
   - Práticas recomendadas para manter a matriz atualizada

9. **Rastreabilidade**
   - Mapeamento entre os casos de teste e sua implementação no código
   - Referências aos cenários BDD correspondentes

### Benefícios deste Documento

1. **Visibilidade**: Fornece uma visão clara e estruturada de todos os casos de teste
2. **Gerenciamento**: Facilita o acompanhamento do progresso e da cobertura
3. **Comunicação**: Permite comunicar efetivamente o status dos testes para stakeholders
4. **Planejamento**: Ajuda no planejamento da expansão da cobertura de testes
5. **Rastreabilidade**: Estabelece conexões entre requisitos, casos de teste e implementação

Este documento serve como uma ferramenta de gerenciamento de testes que pode ser facilmente atualizada e compartilhada com a equipe. Ele vai além da simples listagem de casos de teste, fornecendo contexto, orientações e informações adicionais que tornam a matriz mais útil e informativa.

