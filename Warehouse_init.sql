





-- INIT DB
--DELETE
--DELETE [Warehouse_Projects]

-- Aktywny Project Magazynu.
INSERT INTO [dbo].[Warehouse_Projects]
           ([WarehouseProjectId]
           ,[IsActive])
     VALUES
           (1
           ,1)
GO

-- #1 Punkt Œcie¿ki.
INSERT INTO [dbo].[Warehouse_PathPoints]
           ([WarehouseProjectId]
           ,[PathPointId]
           )
     VALUES
           (1
           ,100
           )
GO
INSERT INTO [dbo].[Warehouse_PathPoints]
           ([WarehouseProjectId]
           ,[PathPointId])
     VALUES
           (1
           ,200)
GO

-- #2 Œcie¿ki
INSERT INTO [dbo].[Warehouse_Pathes]
           ([WarehouseProjectId]
           ,[PathPointId_IN]
           ,[PathPointId_OUT])
     VALUES
           (1
           ,100
           ,200)
GO

-- #3 Segment
INSERT INTO [dbo].[Warehouse_Segments]
           ([WarehouseProjectId]
           ,[SegmentId]
           ,[PathPointId_IN]
           ,[PathPointId_OUT])
     VALUES
           (1
           ,1
           ,100
           ,200)
GO

INSERT INTO [dbo].[Warehouse_Segments]
           ([WarehouseProjectId]
           ,[SegmentId]
           ,[PathPointId_IN]
           ,[PathPointId_OUT])
     VALUES
           (1
           ,2
           ,100
           ,200)
GO



-- Lokalizacje / Lokacje
INSERT INTO [dbo].[Warehouse_Locations]
           ([WarehouseProjectId]
           ,[LocationId]
           ,[SegmentId]
           ,[FirstLocaltion]
           ,[LastLocation])
     VALUES
           (1
           ,1
           ,1			
           ,'AA-01-01'
           ,'AA-09-99')
INSERT INTO [dbo].[Warehouse_Locations]
           ([WarehouseProjectId]
           ,[LocationId]
           ,[SegmentId]
           ,[FirstLocaltion]
           ,[LastLocation])
     VALUES
           (1
           ,2
           ,1			
           ,'PP-01-01'
           ,'PP-09-99')
INSERT INTO [dbo].[Warehouse_Locations]
           ([WarehouseProjectId]
           ,[LocationId]
           ,[SegmentId]
           ,[FirstLocaltion]
           ,[LastLocation])
     VALUES
           (1
           ,3
           ,1			
           ,'ZZ-01-01'
           ,'ZZ-09-99')
GO