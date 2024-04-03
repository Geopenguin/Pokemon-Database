CREATE TABLE [User].[WishList] (
    [UserID]         INT                IDENTITY (1, 1) NOT NULL,
    [CardID]         INT                NOT NULL,
    [WishListedDate] DATETIMEOFFSET (7) DEFAULT (sysdatetimeoffset()) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC, [CardID] ASC),
    FOREIGN KEY ([CardID]) REFERENCES [Cards].[Card] ([CardID]),
    FOREIGN KEY ([UserID]) REFERENCES [User].[Users] ([UserID])
);


GO

