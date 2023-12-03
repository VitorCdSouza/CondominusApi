IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AreasComuns] (
    [IdAreaComum] int NOT NULL IDENTITY,
    [NomeAreaComum] nvarchar(max) NULL,
    CONSTRAINT [PK_AreasComuns] PRIMARY KEY ([IdAreaComum])
);
GO

CREATE TABLE [Condominios] (
    [IdCond] int NOT NULL IDENTITY,
    [NomeCond] nvarchar(max) NULL,
    [EnderecoCond] nvarchar(max) NULL,
    CONSTRAINT [PK_Condominios] PRIMARY KEY ([IdCond])
);
GO

CREATE TABLE [Usuarios] (
    [IdUsuario] int NOT NULL IDENTITY,
    [EmailUsuario] nvarchar(max) NULL,
    [PasswordHashUsuario] varbinary(max) NULL,
    [PasswordSaltUsuario] varbinary(max) NULL,
    [DataAcessoUsuario] datetime2 NULL,
    [IdPessoaUsuario] int NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([IdUsuario])
);
GO

CREATE TABLE [Apartamentos] (
    [IdApart] int NOT NULL IDENTITY,
    [TelefoneApart] nvarchar(max) NULL,
    [NumeroApart] nvarchar(max) NULL,
    [CondominioApartIdCond] int NULL,
    [IdCondominioApart] int NOT NULL,
    CONSTRAINT [PK_Apartamentos] PRIMARY KEY ([IdApart]),
    CONSTRAINT [FK_Apartamentos_Condominios_CondominioApartIdCond] FOREIGN KEY ([CondominioApartIdCond]) REFERENCES [Condominios] ([IdCond])
);
GO

CREATE TABLE [Entregas] (
    [IdEnt] int NOT NULL IDENTITY,
    [DestinatarioEnt] nvarchar(max) NULL,
    [CodEnt] nvarchar(max) NULL,
    [DataEntregaEnt] datetime2 NULL,
    [DataRetiradaEnt] datetime2 NULL,
    [ApartamentoEntIdApart] int NULL,
    [IdApartamentoEnt] int NOT NULL,
    CONSTRAINT [PK_Entregas] PRIMARY KEY ([IdEnt]),
    CONSTRAINT [FK_Entregas_Apartamentos_ApartamentoEntIdApart] FOREIGN KEY ([ApartamentoEntIdApart]) REFERENCES [Apartamentos] ([IdApart])
);
GO

CREATE TABLE [Pessoas] (
    [IdPessoa] int NOT NULL IDENTITY,
    [NomePessoa] nvarchar(max) NULL,
    [TelefonePessoa] nvarchar(max) NULL,
    [TipoPessoa] nvarchar(max) NULL,
    [CpfPessoa] nvarchar(max) NULL,
    [ApartamentoPessoaIdApart] int NULL,
    [IdApartamentoPessoa] int NOT NULL,
    [IdUsuarioPessoa] int NOT NULL,
    CONSTRAINT [PK_Pessoas] PRIMARY KEY ([IdPessoa]),
    CONSTRAINT [FK_Pessoas_Apartamentos_ApartamentoPessoaIdApart] FOREIGN KEY ([ApartamentoPessoaIdApart]) REFERENCES [Apartamentos] ([IdApart]),
    CONSTRAINT [FK_Pessoas_Usuarios_IdUsuarioPessoa] FOREIGN KEY ([IdUsuarioPessoa]) REFERENCES [Usuarios] ([IdUsuario]) ON DELETE CASCADE
);
GO

CREATE TABLE [Dependentes] (
    [IdDependente] int NOT NULL IDENTITY,
    [NomeDependente] nvarchar(max) NULL,
    [CpfDependente] nvarchar(max) NULL,
    [PessoaDependenteIdPessoa] int NULL,
    [IdPessoaDependente] int NOT NULL,
    CONSTRAINT [PK_Dependentes] PRIMARY KEY ([IdDependente]),
    CONSTRAINT [FK_Dependentes_Pessoas_PessoaDependenteIdPessoa] FOREIGN KEY ([PessoaDependenteIdPessoa]) REFERENCES [Pessoas] ([IdPessoa])
);
GO

CREATE TABLE [Notificacoes] (
    [IdNotificacao] int NOT NULL IDENTITY,
    [AssuntoNotificacao] nvarchar(max) NULL,
    [MensagemNotificacao] nvarchar(max) NULL,
    [DataEnvioNotificacao] datetime2 NOT NULL,
    [PessoaNotificacaoIdPessoa] int NULL,
    [IdPessoaNotificacao] int NOT NULL,
    CONSTRAINT [PK_Notificacoes] PRIMARY KEY ([IdNotificacao]),
    CONSTRAINT [FK_Notificacoes_Pessoas_PessoaNotificacaoIdPessoa] FOREIGN KEY ([PessoaNotificacaoIdPessoa]) REFERENCES [Pessoas] ([IdPessoa])
);
GO

CREATE TABLE [PessoasAreasComuns] (
    [IdPessoaPessArea] int NOT NULL,
    [IdAreaComumPessArea] int NOT NULL,
    [IdPessArea] int NOT NULL,
    [dataHoraInicioPessArea] datetime2 NOT NULL,
    [dataHoraFimPessArea] datetime2 NOT NULL,
    [PessoaPessAreaIdPessoa] int NULL,
    [AreaComumPessAreaIdAreaComum] int NULL,
    CONSTRAINT [PK_PessoasAreasComuns] PRIMARY KEY ([IdPessoaPessArea], [IdAreaComumPessArea]),
    CONSTRAINT [FK_PessoasAreasComuns_AreasComuns_AreaComumPessAreaIdAreaComum] FOREIGN KEY ([AreaComumPessAreaIdAreaComum]) REFERENCES [AreasComuns] ([IdAreaComum]),
    CONSTRAINT [FK_PessoasAreasComuns_Pessoas_PessoaPessAreaIdPessoa] FOREIGN KEY ([PessoaPessAreaIdPessoa]) REFERENCES [Pessoas] ([IdPessoa])
);
GO

CREATE TABLE [PessoaNotis] (
    [IdPessoaPessoaNoti] int NOT NULL,
    [IdNotificacaoPessoaNoti] int NOT NULL,
    [IdPessoaNoti] int NOT NULL,
    [PessoaPessoaNotiIdPessoa] int NULL,
    [NotificacaoPessoaNotiIdNotificacao] int NULL,
    CONSTRAINT [PK_PessoaNotis] PRIMARY KEY ([IdPessoaPessoaNoti], [IdNotificacaoPessoaNoti]),
    CONSTRAINT [FK_PessoaNotis_Notificacoes_NotificacaoPessoaNotiIdNotificacao] FOREIGN KEY ([NotificacaoPessoaNotiIdNotificacao]) REFERENCES [Notificacoes] ([IdNotificacao]),
    CONSTRAINT [FK_PessoaNotis_Pessoas_PessoaPessoaNotiIdPessoa] FOREIGN KEY ([PessoaPessoaNotiIdPessoa]) REFERENCES [Pessoas] ([IdPessoa])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdApart', N'CondominioApartIdCond', N'IdCondominioApart', N'NumeroApart', N'TelefoneApart') AND [object_id] = OBJECT_ID(N'[Apartamentos]'))
    SET IDENTITY_INSERT [Apartamentos] ON;
INSERT INTO [Apartamentos] ([IdApart], [CondominioApartIdCond], [IdCondominioApart], [NumeroApart], [TelefoneApart])
VALUES (1, NULL, 1, N'A001', N'11912345678'),
(2, NULL, 1, N'B002', N'11912345678'),
(3, NULL, 1, N'C003', N'11887654321');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdApart', N'CondominioApartIdCond', N'IdCondominioApart', N'NumeroApart', N'TelefoneApart') AND [object_id] = OBJECT_ID(N'[Apartamentos]'))
    SET IDENTITY_INSERT [Apartamentos] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdAreaComum', N'NomeAreaComum') AND [object_id] = OBJECT_ID(N'[AreasComuns]'))
    SET IDENTITY_INSERT [AreasComuns] ON;
INSERT INTO [AreasComuns] ([IdAreaComum], [NomeAreaComum])
VALUES (1, N'Churrasqueira'),
(2, N'Salão de Jogos'),
(3, N'Quadra');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdAreaComum', N'NomeAreaComum') AND [object_id] = OBJECT_ID(N'[AreasComuns]'))
    SET IDENTITY_INSERT [AreasComuns] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdCond', N'EnderecoCond', N'NomeCond') AND [object_id] = OBJECT_ID(N'[Condominios]'))
    SET IDENTITY_INSERT [Condominios] ON;
INSERT INTO [Condominios] ([IdCond], [EnderecoCond], [NomeCond])
VALUES (1, N'Rua Guaranésia, 1070', N'Vila Nova Maria'),
(2, N'Rua Paulo Andrighetti, 1573', N'Condomínio Aquarella Pari Colore'),
(3, N'Rua Paulo Andrighetti, 449', N'Condomínio Edifício Antônio Walter Santiago');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdCond', N'EnderecoCond', N'NomeCond') AND [object_id] = OBJECT_ID(N'[Condominios]'))
    SET IDENTITY_INSERT [Condominios] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdDependente', N'CpfDependente', N'IdPessoaDependente', N'NomeDependente', N'PessoaDependenteIdPessoa') AND [object_id] = OBJECT_ID(N'[Dependentes]'))
    SET IDENTITY_INSERT [Dependentes] ON;
INSERT INTO [Dependentes] ([IdDependente], [CpfDependente], [IdPessoaDependente], [NomeDependente], [PessoaDependenteIdPessoa])
VALUES (1, N'11242100083', 1, N'João Gomes', NULL),
(2, N'30777454025', 1, N'Maria Silva', NULL),
(3, N'53086593032', 2, N'Carlos Oliveira', NULL),
(4, N'54710630070', 3, N'Ana Souza', NULL),
(5, N'03940474002', 3, N'Pedro Santos', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdDependente', N'CpfDependente', N'IdPessoaDependente', N'NomeDependente', N'PessoaDependenteIdPessoa') AND [object_id] = OBJECT_ID(N'[Dependentes]'))
    SET IDENTITY_INSERT [Dependentes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdEnt', N'ApartamentoEntIdApart', N'CodEnt', N'DataEntregaEnt', N'DataRetiradaEnt', N'DestinatarioEnt', N'IdApartamentoEnt') AND [object_id] = OBJECT_ID(N'[Entregas]'))
    SET IDENTITY_INSERT [Entregas] ON;
INSERT INTO [Entregas] ([IdEnt], [ApartamentoEntIdApart], [CodEnt], [DataEntregaEnt], [DataRetiradaEnt], [DestinatarioEnt], [IdApartamentoEnt])
VALUES (1, NULL, N'NBR1354897', '2023-12-03T12:00:31.7957636-03:00', '2023-12-04T12:00:31.7957651-03:00', N'Joao Guilherme', 1),
(2, NULL, N'NBR2468135', '2023-12-03T12:00:31.7957658-03:00', '2023-12-05T12:00:31.7957659-03:00', N'Maria Joaquina', 2),
(3, NULL, N'NBR3581415', '2023-12-03T12:00:31.7957660-03:00', '2023-12-04T12:00:31.7957661-03:00', N'Ana Clara', 3);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdEnt', N'ApartamentoEntIdApart', N'CodEnt', N'DataEntregaEnt', N'DataRetiradaEnt', N'DestinatarioEnt', N'IdApartamentoEnt') AND [object_id] = OBJECT_ID(N'[Entregas]'))
    SET IDENTITY_INSERT [Entregas] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdNotificacao', N'AssuntoNotificacao', N'DataEnvioNotificacao', N'IdPessoaNotificacao', N'MensagemNotificacao', N'PessoaNotificacaoIdPessoa') AND [object_id] = OBJECT_ID(N'[Notificacoes]'))
    SET IDENTITY_INSERT [Notificacoes] ON;
INSERT INTO [Notificacoes] ([IdNotificacao], [AssuntoNotificacao], [DataEnvioNotificacao], [IdPessoaNotificacao], [MensagemNotificacao], [PessoaNotificacaoIdPessoa])
VALUES (1, N'Manutenção elétrica', '2023-12-06T09:13:22.0000000', 0, N'Haverá manutencão no quadro de força do prédio, dia: 20/12 as 14 horas', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdNotificacao', N'AssuntoNotificacao', N'DataEnvioNotificacao', N'IdPessoaNotificacao', N'MensagemNotificacao', N'PessoaNotificacaoIdPessoa') AND [object_id] = OBJECT_ID(N'[Notificacoes]'))
    SET IDENTITY_INSERT [Notificacoes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdNotificacaoPessoaNoti', N'IdPessoaPessoaNoti', N'IdPessoaNoti', N'NotificacaoPessoaNotiIdNotificacao', N'PessoaPessoaNotiIdPessoa') AND [object_id] = OBJECT_ID(N'[PessoaNotis]'))
    SET IDENTITY_INSERT [PessoaNotis] ON;
INSERT INTO [PessoaNotis] ([IdNotificacaoPessoaNoti], [IdPessoaPessoaNoti], [IdPessoaNoti], [NotificacaoPessoaNotiIdNotificacao], [PessoaPessoaNotiIdPessoa])
VALUES (1, 1, 1, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdNotificacaoPessoaNoti', N'IdPessoaPessoaNoti', N'IdPessoaNoti', N'NotificacaoPessoaNotiIdNotificacao', N'PessoaPessoaNotiIdPessoa') AND [object_id] = OBJECT_ID(N'[PessoaNotis]'))
    SET IDENTITY_INSERT [PessoaNotis] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdPessoa', N'ApartamentoPessoaIdApart', N'CpfPessoa', N'IdApartamentoPessoa', N'IdUsuarioPessoa', N'NomePessoa', N'TelefonePessoa', N'TipoPessoa') AND [object_id] = OBJECT_ID(N'[Pessoas]'))
    SET IDENTITY_INSERT [Pessoas] ON;
INSERT INTO [Pessoas] ([IdPessoa], [ApartamentoPessoaIdApart], [CpfPessoa], [IdApartamentoPessoa], [IdUsuarioPessoa], [NomePessoa], [TelefonePessoa], [TipoPessoa])
VALUES (1, NULL, N'56751898901', 1, 0, N'João Gomes', N'11924316523', N'Sindico'),
(2, NULL, N'89674156892', 2, 0, N'Maria Oliveira', N'1198254351', N'Morador'),
(3, NULL, N'32569874561', 3, 0, N'João Viana', N'11984512345', N'Morador');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdPessoa', N'ApartamentoPessoaIdApart', N'CpfPessoa', N'IdApartamentoPessoa', N'IdUsuarioPessoa', N'NomePessoa', N'TelefonePessoa', N'TipoPessoa') AND [object_id] = OBJECT_ID(N'[Pessoas]'))
    SET IDENTITY_INSERT [Pessoas] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdAreaComumPessArea', N'IdPessoaPessArea', N'AreaComumPessAreaIdAreaComum', N'IdPessArea', N'PessoaPessAreaIdPessoa', N'dataHoraFimPessArea', N'dataHoraInicioPessArea') AND [object_id] = OBJECT_ID(N'[PessoasAreasComuns]'))
    SET IDENTITY_INSERT [PessoasAreasComuns] ON;
INSERT INTO [PessoasAreasComuns] ([IdAreaComumPessArea], [IdPessoaPessArea], [AreaComumPessAreaIdAreaComum], [IdPessArea], [PessoaPessAreaIdPessoa], [dataHoraFimPessArea], [dataHoraInicioPessArea])
VALUES (1, 1, NULL, 1, NULL, '2023-12-05T18:00:00.0000000', '2023-12-05T15:00:00.0000000'),
(1, 2, NULL, 2, NULL, '2023-12-06T16:00:00.0000000', '2023-12-06T13:00:00.0000000'),
(2, 2, NULL, 2, NULL, '2023-12-16T21:00:00.0000000', '2023-12-16T18:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdAreaComumPessArea', N'IdPessoaPessArea', N'AreaComumPessAreaIdAreaComum', N'IdPessArea', N'PessoaPessAreaIdPessoa', N'dataHoraFimPessArea', N'dataHoraInicioPessArea') AND [object_id] = OBJECT_ID(N'[PessoasAreasComuns]'))
    SET IDENTITY_INSERT [PessoasAreasComuns] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdUsuario', N'DataAcessoUsuario', N'EmailUsuario', N'IdPessoaUsuario', N'PasswordHashUsuario', N'PasswordSaltUsuario') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([IdUsuario], [DataAcessoUsuario], [EmailUsuario], [IdPessoaUsuario], [PasswordHashUsuario], [PasswordSaltUsuario])
VALUES (1, NULL, N'admin@gmail.com', 1, 0x20574EAB626CC9244E9D28FEAAED69BDC42ED185B6CB6D800A3EE126E3FA71E1E338A545F21390627D645BA3AEBCA73CD310EC7000F55063302D4ABE1B87A30B, 0x805E0C0D51DCB520DBD172E2C5489B15731A6F7F9509384E8FEDF18C568B4ACE4608F97A4E9B5ED4E46F8E7EFAF75103713044567820F6545517C275F0277BDE1CB927D3CAC4B7CB331AEFD8122E5C555574553344FF7142A06C450FD2E3372729BC6A48E6CDBBEF94D214589B304381E908EBE22CCD5EAD54903A897C8B4A8C);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdUsuario', N'DataAcessoUsuario', N'EmailUsuario', N'IdPessoaUsuario', N'PasswordHashUsuario', N'PasswordSaltUsuario') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] OFF;
GO

CREATE INDEX [IX_Apartamentos_CondominioApartIdCond] ON [Apartamentos] ([CondominioApartIdCond]);
GO

CREATE INDEX [IX_Dependentes_PessoaDependenteIdPessoa] ON [Dependentes] ([PessoaDependenteIdPessoa]);
GO

CREATE INDEX [IX_Entregas_ApartamentoEntIdApart] ON [Entregas] ([ApartamentoEntIdApart]);
GO

CREATE INDEX [IX_Notificacoes_PessoaNotificacaoIdPessoa] ON [Notificacoes] ([PessoaNotificacaoIdPessoa]);
GO

CREATE INDEX [IX_PessoaNotis_NotificacaoPessoaNotiIdNotificacao] ON [PessoaNotis] ([NotificacaoPessoaNotiIdNotificacao]);
GO

CREATE INDEX [IX_PessoaNotis_PessoaPessoaNotiIdPessoa] ON [PessoaNotis] ([PessoaPessoaNotiIdPessoa]);
GO

CREATE INDEX [IX_Pessoas_ApartamentoPessoaIdApart] ON [Pessoas] ([ApartamentoPessoaIdApart]);
GO

CREATE UNIQUE INDEX [IX_Pessoas_IdUsuarioPessoa] ON [Pessoas] ([IdUsuarioPessoa]);
GO

CREATE INDEX [IX_PessoasAreasComuns_AreaComumPessAreaIdAreaComum] ON [PessoasAreasComuns] ([AreaComumPessAreaIdAreaComum]);
GO

CREATE INDEX [IX_PessoasAreasComuns_PessoaPessAreaIdPessoa] ON [PessoasAreasComuns] ([PessoaPessAreaIdPessoa]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231203150031_Models', N'7.0.4');
GO

COMMIT;
GO

