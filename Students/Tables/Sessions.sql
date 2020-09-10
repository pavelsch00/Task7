CREATE TABLE [dbo].[Sessions] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [GroupId]       INT NULL,
    [SessionNumber] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GroupResults_To_Groups] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE SET NULL
);