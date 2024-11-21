﻿using System;
using System.Collections.Generic;

namespace Practica06_FNavas.Models;

public partial class Categorium
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
