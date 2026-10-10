CREATE TABLE IF NOT EXISTS TradeNotes(
    Id INTEGER PRIMARY KEY,
	TradeId INTEGER Not Null, 
	Category INTEGER Not Null, 
	Adherence INTEGER, 
	Content TEXT Not Null, 
	UpdatedAt TEXT Not Null 
);
Create Index If Not Exists idx_TradeNotes_UpdatedAt on TradeNotes(UpdatedAt DESC, Id);
