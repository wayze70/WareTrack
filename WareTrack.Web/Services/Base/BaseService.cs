using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WareTrack.Web.Data;

namespace WareTrack.Web.Services.Base;

public abstract class BaseService<TDto, TEntity> : IBaseService<TDto, TEntity>
    where TDto : class
    where TEntity : class
{
    protected readonly DataContext DataContext;
    protected readonly IMapper Mapper;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseService(DataContext dataContext, IMapper mapper)
    {
        DataContext = dataContext;
        Mapper = mapper;
        DbSet = DataContext.Set<TEntity>();
    }

    public virtual async Task<bool> Create(TDto dto)
    {
        try
        {
            DbSet.Add(Mapper.Map<TEntity>(dto));
            return await DataContext.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public virtual async Task<bool> Update(TDto dto, int id)
    {
        try
        {
            TEntity? existingEntity = await DbSet.FindAsync(id);
            if (existingEntity == null)
                return false;
            Mapper.Map(dto, existingEntity);
            return await DataContext.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public virtual async Task<bool> Delete(int id)
    {
        try
        {
            TEntity? entity = await DbSet.FindAsync(id);
            if (entity is null) return false;

            DbSet.Remove(entity);
            return await DataContext.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public virtual async Task<List<TDto>> GetAll()
    {
        try
        {
            var entities = await DbSet.ToListAsync();
            return Mapper.Map<List<TDto>>(entities);
        }
        catch
        {
            return new List<TDto>();
        }
    }

    public virtual async Task<TDto?> GetById(int id)
    {
        try
        {
            TEntity? entity = await DbSet.FindAsync(id);
            return entity != null ? Mapper.Map<TDto>(entity) : null;
        }
        catch
        {
            return null;
        }
    }

    public virtual async Task<List<TDto>> GetAllWithIncludes(params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var query = DbSet.AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            var entities = await query.ToListAsync();
            return Mapper.Map<List<TDto>>(entities);
        }
        catch
        {
            return new List<TDto>();
        }
    }

    public virtual async Task<TDto?> GetByIdWithIncludes(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var query = DbSet.AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            TEntity? entity = await query.FirstOrDefaultAsync(GetIdPredicate(id));
            return entity != null ? Mapper.Map<TDto>(entity) : null;
        }
        catch
        {
            return null;
        }
    }

    public virtual async Task<List<TDto>> GetWhere(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entities = await DbSet.Where(predicate).ToListAsync();
            return Mapper.Map<List<TDto>>(entities);
        }
        catch
        {
            return new List<TDto>();
        }
    }

    public virtual async Task<List<TDto>> GetWhereWithIncludes(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var query = DbSet.AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            var entities = await query.Where(predicate).ToListAsync();
            return Mapper.Map<List<TDto>>(entities);
        }
        catch
        {
            return new List<TDto>();
        }
    }

    protected abstract Expression<Func<TEntity, bool>> GetIdPredicate(int id);
}