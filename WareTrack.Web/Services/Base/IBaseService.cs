using System.Linq.Expressions;

namespace WareTrack.Web.Services.Base;

public interface IBaseService<TDto, TEntity> 
    where TDto : class 
    where TEntity : class
{
    Task<bool> Create(TDto dto);
    Task<bool> Update(TDto dto, int id);
    Task<bool> Delete(int id);
    Task<List<TDto>> GetAll();
    Task<TDto?> GetById(int id);
    Task<List<TDto>> GetAllWithIncludes(params Expression<Func<TEntity, object>>[] includes);
    Task<TDto?> GetByIdWithIncludes(int id, params Expression<Func<TEntity, object>>[] includes);
    Task<List<TDto>> GetWhere(Expression<Func<TEntity, bool>> predicate);
    Task<List<TDto>> GetWhereWithIncludes(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
}