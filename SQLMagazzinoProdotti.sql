CREATE DATABASE Magazzino

CREATE TABLE Prodotti(
ID int NOT NULL IDENTITY(1,1),
CodiceProdotto nvarchar(10) UNIQUE,
Categoria nvarchar(20) CHECK (Categoria IN ('Alimentari', 'Sanitari', 'Cancelleria')),
Descrizione nvarchar(500),
PrezzoUnitario numeric(10,2),
Quantit‡Disponibile int,
CONSTRAINT PK_Prodotti PRIMARY KEY (ID)
)

