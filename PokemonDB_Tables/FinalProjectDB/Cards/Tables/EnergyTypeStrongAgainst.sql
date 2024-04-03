CREATE TABLE [Cards].[EnergyTypeStrongAgainst] (
    [EnergyTypeID]            INT         IDENTITY (1, 1) NOT NULL,
    [StrongAgainstEnergyType] VARCHAR (1) NOT NULL,
    PRIMARY KEY CLUSTERED ([EnergyTypeID] ASC, [StrongAgainstEnergyType] ASC),
    FOREIGN KEY ([EnergyTypeID]) REFERENCES [Cards].[EnergyType] ([EnergyTypeID])
);


GO

