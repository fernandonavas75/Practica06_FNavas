-- Usar la base de datos recién creada
USE Supermarket;
GO

-- Crear la tabla Cliente
CREATE TABLE Cliente (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    Telefono NVARCHAR(15),
    Activo BIT DEFAULT 1
);

-- Crear la tabla Categoría
CREATE TABLE Categoria (
    CategoriaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(200)
);

-- Crear la tabla Producto
CREATE TABLE Producto (
    ProductoID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(200),
    Precio DECIMAL(10,2) NOT NULL CHECK (Precio > 0),
    Stock INT NOT NULL CHECK (Stock >= 0),
    CategoriaID INT NOT NULL FOREIGN KEY REFERENCES Categoria(CategoriaID) ON DELETE CASCADE
);

-- Crear la tabla Carrito
CREATE TABLE Carrito (
    CarritoID INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID INT NOT NULL FOREIGN KEY REFERENCES Cliente(ClienteID) ON DELETE CASCADE,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10,2) DEFAULT 0 CHECK (Total >= 0)
);

-- Crear la tabla DetalleCarrito
CREATE TABLE DetalleCarrito (
    DetalleID INT IDENTITY(1,1) PRIMARY KEY,
    CarritoID INT NOT NULL FOREIGN KEY REFERENCES Carrito(CarritoID) ON DELETE CASCADE,
    ProductoID INT NOT NULL FOREIGN KEY REFERENCES Producto(ProductoID) ON DELETE CASCADE,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    Subtotal DECIMAL(10,2) NOT NULL -- Subtotal se calcula antes de insertarse
);
GO

-- Crear un trigger para calcular el Subtotal automáticamente al insertar
CREATE TRIGGER trg_CalcularSubtotal
ON DetalleCarrito
INSTEAD OF INSERT
AS
BEGIN
    INSERT INTO DetalleCarrito (CarritoID, ProductoID, Cantidad, Subtotal)
    SELECT 
        i.CarritoID, 
        i.ProductoID, 
        i.Cantidad, 
        i.Cantidad * p.Precio AS Subtotal
    FROM inserted i
    INNER JOIN Producto p ON i.ProductoID = p.ProductoID;
END;
GO

GO

-- Crear el trigger para validar stock
CREATE TRIGGER trg_ValidarStock
ON DetalleCarrito
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @ProductoID INT, @Cantidad INT, @CarritoID INT;

    SELECT @ProductoID = ProductoID, @Cantidad = Cantidad, @CarritoID = CarritoID
    FROM inserted;

    -- Validar que el producto tenga suficiente stock
    IF (SELECT Stock FROM Producto WHERE ProductoID = @ProductoID) < @Cantidad
    BEGIN
        RAISERROR ('No hay suficiente stock disponible para este producto.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Reducir el stock del producto y permitir la inserción
    UPDATE Producto
    SET Stock = Stock - @Cantidad
    WHERE ProductoID = @ProductoID;

    -- Insertar el detalle del carrito
    INSERT INTO DetalleCarrito (CarritoID, ProductoID, Cantidad)
    SELECT CarritoID, ProductoID, Cantidad
    FROM inserted;
END;
GO

-- Insertar registros en la tabla Cliente
INSERT INTO Cliente (Nombre, Apellido, Email, Telefono) 
VALUES 
('Fernando', 'Navas', 'fernando.navas@example.com', '0991111111'),
('María', 'Lopez', 'maria.lopez@example.com', '0992222222'),
('Carlos', 'Perez', 'carlos.perez@example.com', '0993333333'),
('Ana', 'Jimenez', 'ana.jimenez@example.com', '0994444444'),
('Luis', 'Torres', 'luis.torres@example.com', '0995555555');
GO

-- Insertar registros en la tabla Categoría
INSERT INTO Categoria (Nombre, Descripcion) 
VALUES 
('Lácteos', 'Productos derivados de la leche'),
('Bebidas', 'Líquidos embotellados y jugos'),
('Snacks', 'Botanas y aperitivos');
GO

-- Insertar registros en la tabla Producto
INSERT INTO Producto (Nombre, Descripcion, Precio, Stock, CategoriaID)
VALUES 
('Leche', 'Leche descremada 1L', 1.50, 100, 1),
('Queso', 'Queso fresco 500g', 4.00, 50, 1),
('Yogurt', 'Yogurt natural 1L', 2.20, 80, 1),
('Agua', 'Agua mineral 1.5L', 1.00, 200, 2),
('Coca Cola', 'Refresco 500ml', 0.80, 150, 2),
('Pepsi', 'Refresco 500ml', 0.75, 120, 2),
('Chips', 'Papas fritas 150g', 1.20, 60, 3),
('Galletas', 'Galletas de chocolate 200g', 2.50, 40, 3),
('Chocolates', 'Barra de chocolate 100g', 1.80, 30, 3),
('Barra de Granola', 'Barra de cereal 50g', 0.90, 70, 3);
GO

-- Verificar inserciones
SELECT * FROM Cliente;
SELECT * FROM Categoria;
SELECT * FROM Producto;
