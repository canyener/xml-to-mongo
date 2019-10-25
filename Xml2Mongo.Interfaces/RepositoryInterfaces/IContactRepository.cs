using System.Collections.Generic;
using System.Threading.Tasks;
using Xml2Mongo.Models;

namespace Xml2Mongo.Interfaces.RepositoryInterfaces
{
    public interface IContactRepository:IGenericRepository<Contact>
    {
        /// <summary>
        /// Returns contact data due to matching criteria.
        /// </summary>
        /// <param name="name">Represents contact name</param>
        /// <returns></returns>
        Task<IEnumerable<Contact>> FindByName(string name);

        /// <summary>
        /// Returns contact data due to matching criteria.
        /// </summary>
        /// <param name="name">Represents contact name</param>
        /// <param name="lastName">Represents contact last name</param>
        /// <returns></returns>
        Task<Contact> FindByNameAndLastName(string name, string lastName);

        /// <summary>
        /// Updates phone numbers of given contact.
        /// </summary>
        /// <param name="name">Represents contact name</param>
        /// <param name="lastName">Represents contact last name</param>
        /// <param name="contact">Represents Contact object to update</param>
        /// <returns></returns>
        Task<bool> Update(string name, string lastName, Contact contact);
    }
}
