alter table [dbo].[Product_PriceForSize] add 
	[StandardPriceType] int null,
	[MeasureDimensionId] int null

alter table [dbo].[Product_PriceForSize] alter column [MinimumWidthManageable] decimal(18,4)
alter table [dbo].[Product_PriceForSize] alter column [MaximumWidthManageable] decimal(18,4)
alter table [dbo].[Product_PriceForSize] alter column [MinimumHeightManageable] decimal(18,4)
alter table [dbo].[Product_PriceForSize] alter column [MaximumHeightManageable] decimal(18,4)
alter table [dbo].[Product_PriceForSize] alter column [MinimumDepthManageable] decimal(18,4)
alter table [dbo].[Product_PriceForSize] alter column [MaximumDepthManageable] decimal(18,4)