CREATE TABLE [Cards].[Card] (
    [CardID]              INT                IDENTITY (1, 1) NOT NULL,
    [CardTypeID]          INT                NOT NULL,
    [RarityID]            INT                NOT NULL,
    [PokemonEnergyTypeID] INT                NOT NULL,
    [IllustratorID]       INT                NOT NULL,
    [CardName]            VARCHAR (1)        NOT NULL,
    [CardNumber]          INT                NOT NULL,
    [CreatedOn]           DATETIMEOFFSET (7) DEFAULT (sysdatetimeoffset()) NOT NULL,
    PRIMARY KEY CLUSTERED ([CardID] ASC),
    FOREIGN KEY ([CardTypeID]) REFERENCES [Cards].[CardType] ([CardTypeID]),
    FOREIGN KEY ([IllustratorID]) REFERENCES [Cards].[Illustrators] ([IllustratorID]),
    FOREIGN KEY ([RarityID]) REFERENCES [Cards].[Rarity] ([RarityID]),
    CONSTRAINT [FK_PokemonEnergyTypeID] FOREIGN KEY ([PokemonEnergyTypeID]) REFERENCES [Cards].[PokemonEnergyType] ([PokemonEnergyTypeID])
);


GO

