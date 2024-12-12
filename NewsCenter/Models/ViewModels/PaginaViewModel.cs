namespace NewsCenter.Models.ViewModels
{
    public class PaginaViewModel
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string Contenido { get; set; } = null!;

        public DateOnly Fecha { get; set; }

        public int? AutorId { get; set; }

        public string NombreAutor { get; set; } = null!;
    }
}
