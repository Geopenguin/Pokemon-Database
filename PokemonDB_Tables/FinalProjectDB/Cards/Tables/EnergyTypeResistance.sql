CREATE TABLE [Cards].[EnergyTypeResistance] (
    [EnergyTypeID]         INT         IDENTITY (1, 1) NOT NULL,
    [ResistanceEnergyType] VARCHAR (1) NOT NULL,
    PRIMARY KEY CLUSTERED ([EnergyTypeID] ASC, [ResistanceEnergyType] ASC),
    FOREIGN KEY ([EnergyTypeID]) REFERENCES [Cards].[EnergyType] ([EnergyTypeID])
);


GO

