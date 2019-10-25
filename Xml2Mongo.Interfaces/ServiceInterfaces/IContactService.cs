using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xml2Mongo.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// Service that runs any type of Contact operations.This Service is generic, as there could be many contact types.
    /// </summary>
    /// <typeparam name="T">Represents Contact object.</typeparam>
    public interface IContactService<T> where T : class
    {
        /// <summary>
        /// Checks requirements aand saves given entities to database.
        /// </summary>
        /// <param name="collection">Represents contact collection to save.</param>
        /// <returns></returns>
        Task<bool> Save(IEnumerable<T> collection);

        /// <summary>
        /// Method that parses xml file located at given path to Contact collection.
        /// </summary>
        /// <param name="path">Represents path of the xml file.</param>
        /// <returns></returns>
        IEnumerable<T> Parse(string path);

        /// <summary>
        /// Method that Imports xml file located at given path to database.
        /// </summary>
        /// <param name="path">Represents path of the xml file</param>
        /// <returns>Return true When import is successful, false otherwise.</returns>
        bool ImportToDatabase(string path);

        /// <summary>
        /// Method that finds data from database due to matching criteria.
        /// </summary>
        /// <param name="name">Represents name property of Contact object.</param>
        /// <returns>Returns serialized Json collection</returns>
        Task<IList<string>> FindByName(string name);

        /// <summary>
        /// Method that finds data from database due to matchinf criteria.
        /// </summary>
        /// <param name="name">Represents name property of Contact object.</param>
        /// <param name="lastName">Represents lastName property of the Contact object.</param>
        /// <returns></returns>
        Task<T> FindByNameAndLastName(string name, string lastName);
    }
}
