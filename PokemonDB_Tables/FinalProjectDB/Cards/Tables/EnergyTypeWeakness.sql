CREATE TABLE [Cards].[EnergyTypeWeakness] (
    [EnergyTypeID]       INT         IDENTITY (1, 1) NOT NULL,
    [WeaknessEnergyType] VARCHAR (1) NOT NULL,
    PRIMARY KEY CLUSTERED ([EnergyTypeID] ASC, [WeaknessEnergyType] ASC),
    FOREIGN KEY ([EnergyTypeID]) REFERENCES [Cards].[EnergyType] ([EnergyTypeID])
);


GO

