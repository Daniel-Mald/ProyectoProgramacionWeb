using System;
using System.Collections.Generic;

namespace BarbieQ.Models.Entities;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
