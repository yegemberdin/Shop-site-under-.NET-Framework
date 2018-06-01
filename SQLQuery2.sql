CREATE TABLE [dbo].[Products] (
    [ProductId]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [Price]           REAL           NOT NULL,
    [DiscountedPrice] REAL           NOT NULL,
    [Stock]           INT            NOT NULL,
    [Purchases]       INT            NOT NULL,
    [Picture]         NVARCHAR (MAX) NULL,
    [Type]            NVARCHAR (MAX) NULL,
    [DateCreated]     DATETIME       NOT NULL,
    [Featured]        BIT            NOT NULL,
    CONSTRAINT [PK_dbo.Products] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

