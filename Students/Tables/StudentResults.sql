CREATE TABLE [dbo].[StudentResults] (
    [Id]                          INT            IDENTITY (1, 1) NOT NULL,
    [StudentId]                   INT            NULL,
    [SessionEducationalSubjectId] INT            NULL,
    [Mark]                        NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StudentResults_To_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_StudentResults_To_SessionEducationalSubjects] FOREIGN KEY ([SessionEducationalSubjectId]) REFERENCES [dbo].[EducationalSubjects] ([Id])
);