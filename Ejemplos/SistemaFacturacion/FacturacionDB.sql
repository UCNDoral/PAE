CREATE DATABASE FacturacionDB;
GO

USE FacturacionDB;
GO

-- Tabla de clientes
CREATE TABLE Clientes (
    IdCliente INT PRIMARY KEY IDENTITY(1,1), -- Identificador único autoincremental
    Nombre VARCHAR(100) NOT NULL, -- Nombre del cliente
    Apellido VARCHAR(100),
    Telefono VARCHAR(20), -- Teléfono
    Direccion VARCHAR(200), -- Dirección

    Activo BIT DEFAULT 1, -- Control lógico (1 = activo, 0 = eliminado)
    FechaRegistro DATETIME DEFAULT GETDATE(), -- Fecha de creación
    FechaModificacion DATETIME NULL -- Fecha de última modificación
);


-- Tabla de productos
CREATE TABLE Productos (
    IdProducto INT PRIMARY KEY IDENTITY(1,1), -- ID único
    Nombre VARCHAR(100) NOT NULL, -- Nombre del producto
    Precio DECIMAL(10,2) NOT NULL, -- Precio
    Stock INT NOT NULL, -- Cantidad disponible

    Activo BIT DEFAULT 1, -- Borrado lógico
    FechaRegistro DATETIME DEFAULT GETDATE()
);


-- Tabla de facturas
CREATE TABLE Facturas (
    IdFactura INT PRIMARY KEY IDENTITY(1,1), -- ID de factura
    IdCliente INT NOT NULL, -- Relación con cliente
    Fecha DATETIME DEFAULT GETDATE(), -- Fecha de la factura

    CONSTRAINT FK_Facturas_Clientes
    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)
);

-- Tabla detalle de factura (productos vendidos)
CREATE TABLE DetalleFactura (
    IdDetalle INT PRIMARY KEY IDENTITY(1,1), -- ID único
    IdFactura INT NOT NULL, -- Relación con factura
    IdProducto INT NOT NULL, -- Relación con producto
    Cantidad INT NOT NULL, -- Cantidad vendida
    Precio DECIMAL(10,2) NOT NULL, -- Precio en el momento de la venta

    CONSTRAINT FK_DetalleFactura_Facturas
    FOREIGN KEY (IdFactura) REFERENCES Facturas(IdFactura),

    CONSTRAINT FK_DetalleFactura_Productos
    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);


INSERT INTO Clientes (Nombre, Apellido, Telefono, Direccion)
VALUES 
('Alejandro', 'González', '555-0101', 'Av. Reforma 123, CDMX'),
('María', 'Rodríguez', '555-0202', 'Calle Luna 45, Guadalajara'),
('Carlos', 'Sánchez', '555-0303', 'Carrera 7 #10-20, Bogotá'),
('Ana', 'Martínez', '555-0404', 'Pasaje Los Pinos 789, Santiago'),
('Luis', 'Pérez', '555-0505', 'Calle Mayor 10, Madrid');

INSERT INTO Productos (Nombre, Precio, Stock)
VALUES 
('Laptop Gamer Pro', 1250.00, 15),
('Mouse Inalámbrico', 25.50, 100),
('Monitor 27" 4K', 350.00, 20),
('Teclado Mecánico RGB', 85.00, 45),
('Auriculares Noise Cancelling', 199.99, 30),
('Memoria USB 64GB', 12.00, 200),
('Disco Duro Externo 2TB', 75.00, 12),
('Silla Ergonómica', 210.00, 8);