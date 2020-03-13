CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
begin
	set nocount on;
	SELECT [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	from dbo.Inventory 
end