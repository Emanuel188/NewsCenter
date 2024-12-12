using System;
using System.Collections.Generic;

namespace NewsCenter.Models.Entities;

public partial class Noticias
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public int? CiudadId { get; set; }

    public int? AutorId { get; set; }

    public virtual Autores? Autor { get; set; }

    public virtual Ciudades? Ciudad { get; set; }
}
