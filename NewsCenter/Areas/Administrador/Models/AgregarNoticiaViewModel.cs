namespace NewsCenter.Areas.Administrador.Models
{
    public class AgregarNoticiaViewModel
    {
        public int NoticiaId { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public IFormFile? Imagen { get; set; }
        public DateOnly? Fecha { get; set; }
        public int? AutorId { get; set; }
        public int? CiudadId { get; set; }
        public IEnumerable<AutorModel>? Autores { get; set; }
        public IEnumerable<CiudadModel>? Ciudades { get; set; }
    }

    public class CrearNoticiaViewModel
    {
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public IFormFile? Imagen { get; set; }
        public DateOnly Fecha { get; set; }
        public int AutorId { get; set; }
        public int CiudadId { get; set; }
    }

    public class AutorModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class CiudadModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
