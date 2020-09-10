CREATE TABLE [dbo].[SessionEducationalSubjects] (
    [Id]                   INT  IDENTITY (1, 1) NOT NULL,
    [SessionId]            INT  NULL,
    [EducationalSubjectId] INT  NULL,
    [Date]                 DATE NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SessionEducationalSubjectsList_To_Session] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Sessions] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_EducationalSubjectsList_To_EducationalSubjects] FOREIGN KEY ([EducationalSubjectId]) REFERENCES [dbo].[EducationalSubjects] ([Id]) ON DELETE SET NULL
);