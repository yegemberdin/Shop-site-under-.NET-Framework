CREATE TABLE [dbo].[OrderDetails] (
    [OrderDetailId] INT  IDENTITY (1, 1) NOT NULL,
    [OrderId]       INT  NOT NULL,
    [ProductId]     INT  NOT NULL,
    [Quantity]      INT  NOT NULL,
    [Total]         REAL NOT NULL,
    CONSTRAINT [PK_dbo.OrderDetails] PRIMARY KEY CLUSTERED ([OrderDetailId] ASC),
    CONSTRAINT [FK_dbo.OrderDetails_dbo.Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OrderId]
    ON [dbo].[OrderDetails]([OrderId] ASC);


