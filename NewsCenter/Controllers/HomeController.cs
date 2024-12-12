using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsCenter.Models.Entities;
using NewsCenter.Models.ViewModels;
using NewsCenter.Repositories;

namespace NewsCenter.Controllers
{
    public class HomeController : Controller
    {

        private readonly NoticiaRepository _repository;

        public HomeController(NoticiaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index(string? query = "")
        {
            IndexViewModel vm = new IndexViewModel();

            var noticias = _repository.GetAll();
            if (!string.IsNullOrEmpty(query))
            {
                noticias = noticias.Where(x => x.Contenido.Contains(query));
            }

             var vms = noticias.Select(noticia => new NoticiaModel
             {
                 Id = noticia.Id,
                 Fecha = noticia.Fecha,
                 Titulo = noticia.Titulo,
                 CiudadId = noticia.CiudadId,
                 Ciudad = noticia.Ciudad.Nombre
             });

            var noticiasDelDia = _repository.GetAll().Where(x => x.Fecha.Day == DateTime.Today.Day)
                .Select(noticia => new NoticiaModel()
                {
                    Id = noticia.Id,
                    Fecha = noticia.Fecha,
                    Titulo = noticia.Titulo,
                    CiudadId = noticia.CiudadId,
                    Ciudad = noticia.Ciudad.Nombre
                });

            vm.Noticia = vms;
            vm.NoticiasDelDia = noticiasDelDia;

            return View(vm);
        }

        public IActionResult Pagina(string id)
        {
            PaginaViewModel vm = new PaginaViewModel();

            var noticia = _repository.GetByTitulo(id.Replace("-", " "));
            vm.Id = noticia.Id;
            vm.Titulo = noticia.Titulo;
            vm.Contenido = noticia.Contenido;
            vm.Fecha = noticia.Fecha;
            vm.NombreAutor = noticia.Autor.Nombre;


            return View(vm);
        }
    }
}
