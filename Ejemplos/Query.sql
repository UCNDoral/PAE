create database Practica

go
use Practica

CREATE TABLE Productos
(
	Id int identity (1,1) primary key,
	Nombre nvarchar (100),
	Descripcion nvarchar(100),
	Precio float,
	Marca nvarchar (100),
	Stock int
)

INSERT INTO Productos VALUES
('Aceite','Nose', 50.5, 'Caballo Negro', 60)

SELECT * FROM Productos;