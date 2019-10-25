using System.Threading.Tasks;

namespace Xml2Mongo.Interfaces.RepositoryInterfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Inserts given entity type to database.
        /// </summary>
        /// <param name="entity">Represents entity to insert database</param>
        /// <returns></returns>
        Task Add(T entity);
    }
}
