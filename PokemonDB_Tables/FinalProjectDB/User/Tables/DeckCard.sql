CREATE TABLE [User].[DeckCard] (
    [DeckID] INT NOT NULL,
    [CardID] INT NOT NULL,
    [UserID] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([DeckID] ASC, [CardID] ASC),
    FOREIGN KEY ([DeckID]) REFERENCES [User].[Deck] ([DeckID]),
    FOREIGN KEY ([UserID], [CardID]) REFERENCES [User].[UserCards] ([UserID], [CardID])
);


GO

