using Microsoft.AspNetCore.Mvc;
using NewsCenter.Areas.Administrador.Models;
using NewsCenter.Models.Entities;
using NewsCenter.Models.ViewModels;
using NewsCenter.Repositories;

namespace NewsCenter.Areas.Administrador.Controllers
{
    [Area("Administrador")]
    public class HomeController : Controller
    {

        private readonly NoticiaRepository _repository;
        private readonly Repository<Ciudades> _repositoryCiudades;
        private readonly Repository<Autores> _repositoryAutores;

        public HomeController(NoticiaRepository repository, Repository<Ciudades> ciudadesRepository, Repository<Autores> autoresRepository)
        {
            _repository = repository;
            _repositoryCiudades = ciudadesRepository;
            _repositoryAutores = autoresRepository;
        }

        public IActionResult Index(string? query = "")
        {
            IndexViewModel vm = new IndexViewModel();

            var noticias = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(query))
            {
                noticias = noticias.Where(n => n.Contenido.Contains(query));
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

        public IActionResult AgregarNoticia()
        {
            var ciudades = _repositoryCiudades.GetAll().Select(c => new CiudadModel
            {
                Id = c.Id,
                Nombre = c.Nombre
            });

            var autores = _repositoryAutores.GetAll().Select(a => new AutorModel
            {
                Id = a.Id,
                Nombre = a.Nombre
            });

            var vm = new AgregarNoticiaViewModel()
            {
                Autores = autores,
                Ciudades = ciudades
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AgregarNoticia(CrearNoticiaViewModel vm)
        {
            // Validar si el título es nulo o vacío
            if (string.IsNullOrWhiteSpace(vm.Titulo))
            {
                ModelState.AddModelError("", "El título es requerido");
            }

            // Validar si el contenido es nulo o vacío
            if (string.IsNullOrWhiteSpace(vm.Contenido))
            {
                ModelState.AddModelError("", "El contenido es requerido");
            }

            // Validar si la fecha está en el pasado
            if (vm.Fecha < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("", "La fecha no puede ser en el pasado");
            }

            // Validar si el autor es válido
            if (vm.AutorId <= 0)
            {
                ModelState.AddModelError("", "El autor es requerido");
            }

            // Validar si la ciudad es válida
            if (vm.CiudadId <= 0)
            {
                ModelState.AddModelError("", "La ciudad es requerida");
            }

            // Validar el archivo de imagen (opcional)
            if (vm.Imagen != null)
            {
                // Validar el tipo de contenido del archivo
                if (vm.Imagen.ContentType != "image/jpg" && vm.Imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "La imagen debe ser un archivo PNG");
                }

                // Validar el tamaño del archivo
                if (vm.Imagen.Length > 1024 * 1024)
                {
                    ModelState.AddModelError("", "La imagen debe pesar menos de 1MB");
                }
            }

            // Si el modelo no es válido, retornar la vista con los errores
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Crear la entidad Noticias
            var noticia = new Noticias()
            {
                Titulo = vm.Titulo,
                Contenido = vm.Contenido,
                Fecha = vm.Fecha,
                AutorId = vm.AutorId,
                CiudadId = vm.CiudadId
            };

            // Insertar en la base de datos
            _repository.Insert(noticia);

            var contentType = vm.Imagen.ContentType.Split("/")[1];

            // Guardar la imagen si fue cargada
            if (vm.Imagen != null)
            {
                string filePath = Path.Combine("wwwroot/imagenes", $"{noticia.Id}.jpg");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    vm.Imagen.CopyTo(stream);
                }
            }

            // Redirigir a la acción Index (o cualquier otra vista)
            return RedirectToAction("Index", new { Area = "Administrador"});
        }

        public IActionResult EditarNoticia(int id)
        {
            var noticia = _repository.Get(id);
            if (noticia == null) return RedirectToAction("Index", new { Area = "Administrador" });

            var vm = new AgregarNoticiaViewModel()
            {
                NoticiaId = noticia.Id,
                Fecha = noticia.Fecha,
                Titulo = noticia.Titulo,
                CiudadId = noticia.CiudadId,
                AutorId = noticia.AutorId,
                Autores = _repositoryAutores.GetAll().Select(a => new AutorModel
                {
                    Id = a.Id,
                    Nombre = a.Nombre
                }),
                Ciudades = _repositoryCiudades.GetAll().Select(c => new CiudadModel
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                }),
                Contenido = noticia.Contenido

            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditarNoticia(AgregarNoticiaViewModel vm)
        {

            var noticia = _repository.Get(vm.NoticiaId);
            if (noticia == null) return RedirectToAction("Index", new { Area = "Administrador" });

            if (string.IsNullOrWhiteSpace(vm.Titulo))
            {
                ModelState.AddModelError("", "El título es requerido");
            }

            // Validar si el contenido es nulo o vacío
            if (string.IsNullOrWhiteSpace(vm.Contenido))
            {
                ModelState.AddModelError("", "El contenido es requerido");
            }

            // Validar si la fecha está en el pasado
            if (!vm.Fecha.HasValue)
            {
                ModelState.AddModelError("", "La fecha no puede estar vacia");
            }

            // Validar si el autor es válido
            if (vm.AutorId <= 0)
            {
                ModelState.AddModelError("", "El autor es requerido");
            }

            // Validar si la ciudad es válida
            if (vm.CiudadId <= 0)
            {
                ModelState.AddModelError("", "La ciudad es requerida");
            }

            // Validar el archivo de imagen (opcional)
            if (vm.Imagen != null)
            {
                // Validar el tipo de contenido del archivo
                if (vm.Imagen.ContentType != "image/jpg" && vm.Imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "La imagen debe ser un archivo PNG");
                }

                // Validar el tamaño del archivo
                if (vm.Imagen.Length > 1024 * 1024)
                {
                    ModelState.AddModelError("", "La imagen debe pesar menos de 1MB");
                }
            }

            // Si el modelo no es válido, retornar la vista con los errores
            var ciudades = _repositoryCiudades.GetAll().Select(c => new CiudadModel
            {
                Id = c.Id,
                Nombre = c.Nombre
            });
            var autores = _repositoryAutores.GetAll().Select(a => new AutorModel
            {
                Id = a.Id,
                Nombre = a.Nombre
            });

            vm.Autores = autores;
            vm.Ciudades = ciudades;
            if (!ModelState.IsValid)
                return View(vm);


             noticia.Titulo = vm.Titulo;
            noticia.Contenido = vm.Contenido;
            noticia.Fecha = (DateOnly)vm.Fecha;
            noticia.AutorId = vm.AutorId;
            noticia.CiudadId = vm.CiudadId;



            if (vm.Imagen != null)
            {
                string filePath = Path.Combine("wwwroot/imagenes", $"{noticia.Id}.jpg");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    vm.Imagen.CopyTo(stream);
                }
            }
            _repository.Update(noticia);

            // Redirigir a la acción Index (o cualquier otra vista)
            return RedirectToAction("Index", new { Area = "Administrador" });

        }

        public IActionResult EliminarNoticia(int id)
        {
            var noticia = _repository.Get(id);
            if (noticia == null) return RedirectToAction("Index", new { Area = "Administrador" });

            var vm = new EliminarNoticiaViewModel()
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult EliminarNoticia(EliminarNoticiaViewModel vm)
        {
            var noticia = _repository.Get(vm.Id);
            if (noticia == null) return RedirectToAction("Index", new { Area = "Administrador" });
            _repository.Delete(noticia);
            return RedirectToAction("Index", new { Area = "Administrador" });
        }


    }
}
