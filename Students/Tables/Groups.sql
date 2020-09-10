CREATE TABLE [dbo].[Groups] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [SpecialtyId] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Groups_To_Specialtys] FOREIGN KEY ([SpecialtyId]) REFERENCES [dbo].[Specialtys] ([Id]) ON DELETE SET NULL
);