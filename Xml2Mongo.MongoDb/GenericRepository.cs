using MongoDB.Driver;
using System.Configuration;
using System.Threading.Tasks;
using Xml2Mongo.Interfaces.RepositoryInterfaces;

namespace Xml2Mongo.MongoDb
{
    public abstract class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        #region "Fields"

        public readonly IMongoCollection<T> _mongoCollection;

        #endregion

        #region "Constructors"

        public GenericRepository()
        {
            IMongoClient mongoClient = new MongoClient(ConfigurationManager.AppSettings.Get("mongoConnection"));
            var mongoDatabase = mongoClient.GetDatabase("contactsdb");
            _mongoCollection = mongoDatabase.GetCollection<T>(typeof(T).Name.ToLower());
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Inserts given entity type to database.
        /// </summary>
        /// <param name="entity">Represents entity to insert database</param>
        /// <returns></returns>
        public async Task Add(T entity)
        {
            await _mongoCollection.InsertOneAsync(entity);
        }

        #endregion
    }
}
