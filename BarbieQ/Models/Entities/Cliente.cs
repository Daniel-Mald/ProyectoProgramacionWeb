using System;
using System.Collections.Generic;

namespace BarbieQ.Models.Entities;

public partial class Cliente
{
    public int Id { get; set; }

    public string CorreoElectronico { get; set; } = null!;
}
