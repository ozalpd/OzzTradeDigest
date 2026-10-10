CREATE TABLE IF NOT EXISTS TradeImages(
    Id INTEGER PRIMARY KEY,
	TradeId INTEGER Not Null, 
	ImageURL TEXT Not Null, 
	TradeNoteId INTEGER, 
	UpdatedAt TEXT Not Null 
);
Create Index If Not Exists idx_TradeImages_TradeId on TradeImages(TradeId, UpdatedAt);
Create Index If Not Exists idx_TradeImages_TradeNoteId on TradeImages(TradeNoteId, UpdatedAt);
Create Index If Not Exists idx_TradeImages_UpdatedAt on TradeImages(UpdatedAt DESC, Id);
