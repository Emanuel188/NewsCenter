using NewsCenter.Models.Entities;

namespace NewsCenter.Repositories
{
    public class Repository<T> where T : class
    {
        public NewscenterContext ctx;
        public Repository(NewscenterContext ctx)
        {
            this.ctx = ctx;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return ctx.Set<T>();
        }

        public virtual T? Get(object id)
        {
            return ctx.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {
            ctx.Add(entity);
            ctx.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            ctx.Update(entity);
            ctx.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            ctx.Remove(entity);
            ctx.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            var entity = ctx.Find<T>(id);
            if (entity != null)
            {
                ctx.Remove(entity);
            }
            ctx.SaveChanges();
        }
    }
}
