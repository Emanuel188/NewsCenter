namespace NewsCenter.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<NoticiaModel> Noticia { get; set; } = null!;
        public IEnumerable<NoticiaModel>? NoticiasDelDia { get; set; }

        
    }

    public class NoticiaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;

        public DateOnly Fecha { get; set; }

        public int? CiudadId { get; set; }

        public int? AutorId { get; set; }

        public string? Autor { get; set; }

        public string Ciudad { get; set; }
    }
}