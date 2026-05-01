/*
 * IRepository.cs
 * FreshHarvestMarket
 *
 * This interface defines a generic repository pattern for data access.
 *
 * It provides a standardized set of methods for performing CRUD
 * (Create, Read, Update, Delete) operations on entities.
 *
 * The generic type parameter <T> allows this interface to be used
 * with any entity class in the application.
 *
 * This abstraction helps separate data access logic from business logic,
 * making the application more modular, maintainable, & testable.
 */

namespace FreshHarvestMarket.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T? Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
