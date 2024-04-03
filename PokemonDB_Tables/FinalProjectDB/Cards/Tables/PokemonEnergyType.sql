CREATE TABLE [Cards].[PokemonEnergyType] (
    [PokemonEnergyTypeID] INT IDENTITY (1, 1) NOT NULL,
    [CardID]              INT NOT NULL,
    [EnergyTypeID]        INT NOT NULL,
    PRIMARY KEY CLUSTERED ([PokemonEnergyTypeID] ASC),
    FOREIGN KEY ([EnergyTypeID]) REFERENCES [Cards].[EnergyType] ([EnergyTypeID]),
    CONSTRAINT [FK_CardID] FOREIGN KEY ([CardID]) REFERENCES [Cards].[Card] ([CardID]),
    UNIQUE NONCLUSTERED ([CardID] ASC, [EnergyTypeID] ASC)
);


GO

