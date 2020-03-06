CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int output,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Tax money,
	@Total money

As
Begin
	set nocount on;
	insert into dbo.Sale(CashierId, SaleDate, SubTotal, Tax, Total)
	values(@CashierId, @SaleDate, @SubTotal, @Tax, @Total);
	select @Id = @@IDENTITY;
End

--	select @Id = @@IDENTITY;
--This previous line didn't succeed in returning back the id to the SaleDBModel, so another stored procedure
--called spSale_Lookup got created

