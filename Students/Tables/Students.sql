CREATE TABLE [dbo].[Students] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [FullName]    NVARCHAR (MAX) NULL,
    [Gender]      NVARCHAR (MAX) NULL,
    [DateOfBirth] DATE           NULL,
    [GroupId]     INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Student_To_Groups] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE SET NULL
);