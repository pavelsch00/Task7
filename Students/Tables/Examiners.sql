CREATE TABLE [dbo].[Examiners] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [FullName] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);