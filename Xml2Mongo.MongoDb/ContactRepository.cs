using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xml2Mongo.Interfaces.RepositoryInterfaces;
using Xml2Mongo.Models;

namespace Xml2Mongo.MongoDb
{
    public class ContactRepository:GenericRepository<Contact>,
       IContactRepository
    {

        /// <summary>
        /// Returns contact data due to matching criteria.
        /// </summary>
        /// <param name="name">Represents contact name</param>
        /// <returns></returns>
        public async Task<IEnumerable<Contact>> FindByName(string name)
        {
            var filter = Builders<Contact>.Filter.Eq("Name", name);
            var result = await _mongoCollection.Find(filter).ToListAsync();
            return  result;
        }

        /// <summary>
        /// Returns contact data due to matching criteria.
        /// </summary>
        /// <param name="name">Represents contact name</param>
        /// <param name="lastName">Represents contact last name</param>
        /// <returns></returns>
        public async Task<Contact> FindByNameAndLastName(string name,string lastName)
        {
            Contact result = null;

            try
            {
                var nameFilter = Builders<Contact>.Filter.Eq("Name", name);
                var lastNameFilter = Builders<Contact>.Filter.Eq("LastName", lastName);
                var filter = Builders<Contact>.Filter.And(nameFilter, lastNameFilter);
                result = await _mongoCollection.Find(filter).FirstAsync();
            }
            catch (Exception)
            {
            }
           
            return result;
        }

        /// <summary>
        /// Updates phone numbers of given contact.
        /// </summary>
        /// <param name="name">Represents contact name</param>
        /// <param name="lastName">Represents contact last name</param>
        /// <param name="contact">Represents Contact object to update</param>
        /// <returns></returns>
        public async Task<bool>Update(string name, string lastName, Contact contact)
        {
            var nameFilter = Builders<Contact>.Filter.Eq("Name", name);
            var lastNameFilter = Builders<Contact>.Filter.Eq("LastName", lastName);
            var filter = Builders<Contact>.Filter.And(nameFilter, lastNameFilter);
            var update = Builders<Contact>.Update
                           .Set("Phones", contact.Phones);
            var result = await _mongoCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
