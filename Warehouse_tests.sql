USE [Warehouse]
GO

-- TEST.
-- Dodaj PathPoint dla nieistniej¹cego projektu.
-- THROW
INSERT INTO [dbo].[Warehouse_PathPoints]
           ([WarehouseProjectId]
           ,[PathPointId]
           )
     VALUES
           (2
           ,100
           )
GO


-- TEST.
-- Dodaj Path dla nieistniej¹cego projektu.
-- THROW
INSERT INTO [dbo].[Warehouse_Pathes]
           ([WarehouseProjectId]
           ,[PathPointId_IN]
           ,[PathPointId_OUT]
           )
     VALUES
           (2
           ,100
           ,200
           )
GO

-- TEST.
-- Dodaj Path z IN == OUT.
-- THROW
INSERT INTO [dbo].[Warehouse_Pathes]
           ([WarehouseProjectId]
           ,[PathPointId_IN]
           ,[PathPointId_OUT]
           )
     VALUES
           (1
           ,100
           ,100
           )
GO
