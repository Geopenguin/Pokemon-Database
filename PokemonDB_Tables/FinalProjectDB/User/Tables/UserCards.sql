CREATE TABLE [User].[UserCards] (
    [UserID]    INT                NOT NULL,
    [CardID]    INT                NOT NULL,
    [DateAdded] DATETIMEOFFSET (7) DEFAULT (sysdatetimeoffset()) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC, [CardID] ASC),
    FOREIGN KEY ([CardID]) REFERENCES [Cards].[Card] ([CardID]),
    FOREIGN KEY ([UserID]) REFERENCES [User].[Users] ([UserID])
);


GO

