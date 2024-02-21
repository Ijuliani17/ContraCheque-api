# Credito.ContraCheque.API

# Descrição
Projeto de API .NET voltado para área de Recursos Humanos com as seguintes caracteristicas:
A entidade principal da API é o Funcionário, que possui as seguintes propriedades:

- Id
- Nome
- Sobrenome
- Documento (CPF válido)
- Setor
- Salário bruto
- Data de admissão
- Desconto no plano de saúde (bool)
- Desconto no plano dental (bool)
- Desconto de vale transporte (bool)
  
Os descontos que podem ser aplicados ao salário bruto do funcionário são:

- INSS: varia de 7,5% a 14%, dependendo do salário bruto.
- Imposto de Renda Retido na Fonte (IRRF): varia de 7,5% a 27,5%, dependendo do salário bruto, com um teto máximo para o desconto.
- Plano de saúde: R$ 10 fixos, se o funcionário optar pelo plano.
- Plano dental: R$ 5 fixos, se o funcionário optar pelo plano.
- Vale transporte: 6% do salário bruto, se o funcionário optar pelo vale transporte e se o salário for maior que R$ 1500.
- FGTS: 8% do salário bruto.

O objetivo é gerir internamente essas informações, que atualmente são tratadas por uma consultoria externa, visando economia e confidencialidade dos dados.

# Tecnologias Utilizadas:
 - .Net 8
 - Dapper
 - Mediatr
 - Serilog
 - Sqlite
 - Xunit

Todas as informações de utilização dos endpoints expostos se encontram no swagger.

# Observações:
Os dois dockerfiles são separados para caso a execução seja feita localmente ou por uma ferramenta de CI e CD com as alterações na criação da aplicação de acordo com a pipeline para criação do Artefato da Aplicação.

Os endpoints estão com as decrições de funcionalidades assim como os pontos de atenção que ele solicita.

