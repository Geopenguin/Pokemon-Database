CREATE TABLE [Cards].[EnergyType] (
    [EnergyTypeID] INT         IDENTITY (1, 1) NOT NULL,
    [EnergyName]   VARCHAR (1) NULL,
    PRIMARY KEY CLUSTERED ([EnergyTypeID] ASC),
    UNIQUE NONCLUSTERED ([EnergyName] ASC)
);


GO

