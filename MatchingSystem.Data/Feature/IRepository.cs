using System.Linq.Expressions;

namespace MatchingSystem.Data.Feature;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity findById(int id);
    IEnumerable<TEntity> find(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> findAll();

    TEntity save(TEntity item);
    void update(TEntity item);
    void delete(TEntity item);
}