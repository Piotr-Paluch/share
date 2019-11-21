





-- STACJA -- LOKALIZACJA

--DROP TABLE [dbo].[Warehouse_Projects]
CREATE TABLE [dbo].[Warehouse_Projects]
(
	-- KEY
	[WarehouseProjectId] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(0),
	CONSTRAINT PK_Warehouse_Projects PRIMARY KEY NONCLUSTERED ([WarehouseProjectId])
)

--DROP TABLE [dbo].[Warehouse_PathPoints]
CREATE TABLE [dbo].[Warehouse_PathPoints]
(
	-- KEY
	[WarehouseProjectId] INT NOT NULL,
	[PathPointId] INT NOT NULL,
	-- KEY

	--[Side] CHAR NOT NULL,

	-- FK
	CONSTRAINT FK_Warehouse_Projects FOREIGN KEY ([WarehouseProjectId]) REFERENCES [Warehouse_Projects]([WarehouseProjectId]),
	-- PK
	CONSTRAINT PK_Warehouse_PathPoints PRIMARY KEY NONCLUSTERED ([WarehouseProjectId], [PathPointId])
)

--DROP TABLE [dbo].[Warehouse_Pathes]
CREATE TABLE [dbo].[Warehouse_Pathes]
(
  -- KEY
  [WarehouseProjectId] INT NOT NULL,
  [PathPointId_IN] INT NOT NULL,
  [PathPointId_OUT] INT NOT NULL,
  -- KEY


  [Side] CHAR NOT NULL,
  [OrderIndex] INT NOT NULL,



	--CONSTRAINT FK_Project FOREIGN KEY ([WarehouseProjectId]) REFERENCES [Warehouse_Projects]([WarehouseProjectId]),

	CONSTRAINT FK_PP_IN   FOREIGN KEY ([WarehouseProjectId],[PathPointId_IN]) REFERENCES [Warehouse_PathPoints]([WarehouseProjectId],[PathPointId]),
	CONSTRAINT FK_PP_OUT  FOREIGN KEY ([WarehouseProjectId],[PathPointId_OUT]) REFERENCES [Warehouse_PathPoints]([WarehouseProjectId],[PathPointId]),

	CONSTRAINT PK_Warehouse_Pathes PRIMARY KEY NONCLUSTERED ([WarehouseProjectId], [PathPointId_IN], [PathPointId_OUT])
)

-- Walidacja dodania po³¹czenia z IN == OUT
--DROP TRIGGER ValidatorTrigger_Pathes;
CREATE TRIGGER ValidatorTrigger_Pathes on [Warehouse_Pathes]
FOR INSERT, UPDATE AS
BEGIN
    IF EXISTS (SELECT TOP 1 1 from inserted i WHERE i.PathPointId_IN = i.PathPointId_OUT)
	BEGIN
		PRINT 'PathPointId_IN == PathPointId_OUT';
        ROLLBACK TRANSACTION;
	END;
END;








--DROP TABLE [dbo].[Warehouse_Pathes]
CREATE TABLE [dbo].[Warehouse_Segments]
(
  -- KEY
  [WarehouseProjectId] INT NOT NULL,
  [SegmentId] INT NOT NULL,


  [PathPointId_IN] INT NOT NULL,
  [PathPointId_OUT] INT NOT NULL,  
  -- KEY


    --[Side] CHAR NOT NULL,
	--CONSTRAINT FK_Project FOREIGN KEY ([WarehouseProjectId]) REFERENCES [Warehouse_Projects]([WarehouseProjectId]),
	CONSTRAINT FK_Warehouse_Pathes FOREIGN KEY ([WarehouseProjectId],[PathPointId_IN],[PathPointId_OUT]) REFERENCES [Warehouse_Pathes]([WarehouseProjectId],[PathPointId_IN],[PathPointId_OUT]),
	CONSTRAINT PK_Warehouse_Segments PRIMARY KEY NONCLUSTERED ([WarehouseProjectId], [SegmentId])
)

--DROP TABLE [dbo].[Warehouse_Locations]
CREATE TABLE [dbo].[Warehouse_Locations]
(
	-- KEY
	[WarehouseProjectId] INT NOT NULL,
	[LocationId] INT NOT NULL,
	--FK
	[SegmentId] INT NOT NULL,
	-- 
	[FirstLocaltion] CHAR(8) NOT NULL,
	[LastLocation] CHAR(8) NOT NULL,  
	
	--[LocationSymbol] CHAR(2) NOT NULL,
	--[Shelf] TINYINT NOT NULL,
	--[Position] TINYINT NOT NULL,

	UNIQUE ([FirstLocaltion]),
	UNIQUE ([LastLocation]),

	CONSTRAINT FK_Warehouse_Segments FOREIGN KEY ([WarehouseProjectId],[SegmentId]) REFERENCES [Warehouse_Segments]([WarehouseProjectId],[SegmentId]),
	CONSTRAINT PK_Locations PRIMARY KEY NONCLUSTERED ([WarehouseProjectId], [LocationId])

)