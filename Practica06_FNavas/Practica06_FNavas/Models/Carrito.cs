using System;
using System.Collections.Generic;

namespace Practica06_FNavas.Models;

public partial class Carrito
{
    public int CarritoId { get; set; }

    public int ClienteId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public decimal? Total { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<DetalleCarrito> DetalleCarritos { get; set; } = new List<DetalleCarrito>();
}
