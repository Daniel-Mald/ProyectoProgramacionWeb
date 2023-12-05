using System;
using System.Collections.Generic;

namespace BarbieQ.Models.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int? CantidadExistencia { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdCategoria { get; set; }

    public string Ingredientes { get; set; } = null!;

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;
}
