using System;
using System.Collections.Generic;

namespace Practica06_FNavas.Models;

public partial class DetalleCarrito
{
    public int DetalleId { get; set; }

    public int CarritoId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Carrito Carrito { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
