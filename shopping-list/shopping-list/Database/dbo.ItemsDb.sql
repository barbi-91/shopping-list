CREATE TABLE [dbo].[ItemsDb] (
    [Id]		INT			IDENTITY (1, 1) NOT NULL,
    [Item]		NCHAR (53)	NULL,
	[Amount]	INT			NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

