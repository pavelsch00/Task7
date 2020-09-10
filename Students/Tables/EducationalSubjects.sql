CREATE TABLE [dbo].[EducationalSubjects] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [SubjectName]  NVARCHAR (MAX) NOT NULL,
    [SubjectType]  NVARCHAR (MAX) NOT NULL,
    [ExamainersId] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Groups_To_Examaites] FOREIGN KEY ([ExamainersId]) REFERENCES [dbo].[Examiners] ([Id]) ON DELETE SET NULL
);