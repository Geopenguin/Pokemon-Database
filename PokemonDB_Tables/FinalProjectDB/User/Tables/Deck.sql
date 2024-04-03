CREATE TABLE [User].[Deck] (
    [DeckID] INT NOT NULL,
    [UserID] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([DeckID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [User].[Users] ([UserID]),
    UNIQUE NONCLUSTERED ([DeckID] ASC, [UserID] ASC)
);


GO

