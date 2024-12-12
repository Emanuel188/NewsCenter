using Microsoft.EntityFrameworkCore;
using NewsCenter.Models.Entities;

namespace NewsCenter.Repositories
{
    public class NoticiaRepository : Repository<Noticias>
    {
        public NoticiaRepository(NewscenterContext ctx) : base(ctx)
        {
        }


        public override IEnumerable<Noticias> GetAll()
        {
            return ctx.Noticias
                .Include(x => x.Ciudad)
                .Include(x => x.Autor);
        }

        public override Noticias? Get(object id)
        {
            return ctx.Noticias
                .Include(x => x.Ciudad)
                .Include(x => x.Autor)
                .FirstOrDefault(x => x.Id == (int)id);
        }

        public Noticias? GetByTitulo(string titulo)
        {
            return ctx.Noticias
                .Include(x => x.Ciudad)
                .Include(x => x.Autor)
                .FirstOrDefault(x => x.Titulo == titulo);
        }
    }
}
