CREATE TABLE IF NOT EXISTS ExitOrders(
    Id INTEGER PRIMARY KEY,
	TradeId INTEGER Not Null, 
	ExitMode INTEGER Not Null, 
	OrderType INTEGER Not Null, 
	OrderPrice TEXT Not Null, 
	FilledPrice TEXT, 
	OrderQuantity TEXT, 
	FilledQuantity TEXT, 
	OrderValue INTEGER, 
	FilledValue INTEGER, 
	Notes TEXT, 
	CancellationTime TEXT, 
	FilledTime TEXT, 
	UpdatedAt TEXT Not Null 
);
Create Index If Not Exists idx_ExitOrders_FilledTime on ExitOrders(FilledTime);
Create Index If Not Exists idx_ExitOrders_UpdatedAt on ExitOrders(UpdatedAt DESC, Id);
