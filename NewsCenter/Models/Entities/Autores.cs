using System;
using System.Collections.Generic;

namespace NewsCenter.Models.Entities;

public partial class Autores
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Noticias> Noticias { get; set; } = new List<Noticias>();
}
