﻿CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
BEGIN
	SET nocount on;
	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable
	from dbo.Product
	order by ProductName;
END

