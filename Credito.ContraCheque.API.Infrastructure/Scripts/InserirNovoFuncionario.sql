    INSERT INTO Funcionarios 
    (Id
    , Nome
    , Sobrenome
    , Documento, Setor
    , salarioBruto
    , DataAdmissao
    , TemDescontoPlanoSaude
    , TemDescontoPlanoDental
    , TemDescontoValeTransporte)
    VALUES (
    @ExternalId,
    @Nome,
    @Sobrenome, 
    @Documento, 
    @Setor,
    @salarioBruto, 
    @DataAdmissao,
    @TemDescontoPlanoSaude, 
    @TemDescontoPlanoDental, 
    @TemDescontoVT);