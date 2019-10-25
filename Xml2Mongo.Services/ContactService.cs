using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xml2Mongo.Interfaces.ServiceInterfaces;
using Xml2Mongo.Models;
using Xml2Mongo.MongoDb;

namespace Xml2Mongo.Services
{
    /// <summary>
    /// Service that runs any type of Contact operations.This Service is generic, as there could be many contact types.
    /// </summary>
    public class ContactService : IContactService<Contact>
    {
        #region "Fields"

        private readonly ContactRepository _contactRepository;

        #endregion

        #region "Constructors"

        public ContactService()
        {
            _contactRepository = new ContactRepository();
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Method that parses xml file located at given path to Contact collection.
        /// </summary>
        /// <param name="path">Represents path of the xml file.</param>
        /// <returns></returns>
        public IEnumerable<Contact> Parse(string path)
        {
            IList<Contact> contactList = new List<Contact>();

           
                //No file check needed as we're sure that the given file is in xml format.
                var xmlDocument = XDocument.Load(path);


                //Iterating nodes and creating model to insert database.
                foreach (XElement xe in xmlDocument.Descendants("sequence"))
                {
                    Contact contact = new Contact();
                    contact.Name = xe.Element("name").Value;
                    contact.LastName = xe.Element("lastName").Value;
                    contact.Phones = new List<string>();
                    contact.Phones.Add(xe.Element("phone").Value);

                    contactList.Add(contact);
                }
            

            return contactList;
        }

        /// <summary>
        /// Checks requirements aand saves given entities to database.
        /// </summary>
        /// <param name="collection">Represents contact collection to save.</param>
        /// <returns></returns>
        public async Task<bool> Save(IEnumerable<Contact> collection)
        {
            try
            {
                foreach (var item in collection)
                {
                    //Checking if any contact with same name and lastname exists in database. If yes, we just add the missing phones to this existing contact.
                    Contact existingContact = await _contactRepository.FindByNameAndLastName(item.Name, item.LastName);

                    if (existingContact != null)
                    {
                        //Getting the phone numbers that does not exist in database.
                        List<string> newPhones = item.Phones.Except(existingContact.Phones).ToList();
                        existingContact.Phones.AddRange(newPhones);

                        await _contactRepository.Update(item.Name, item.LastName, existingContact);
                    }
                    else
                    {
                        await _contactRepository.Add(item);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Method that Imports xml file located at given path to database.
        /// </summary>
        /// <param name="path">Represents path of the xml file</param>
        /// <returns>Return true When import is successful, false otherwise.</returns>
        public bool ImportToDatabase(string path)
        {
        
           //Checking if an xml file exits at given path.
            if (!File.Exists(path))
            {
                return false;
            }
            
            //Parsing xml file to contact list.
            var contactList = this.Parse(path);
            
            //Grouping data and avoiding duplicate records.
            var contactsToSave = SharpenDataToSave(contactList);

            var saveTask = Save(contactsToSave);

            saveTask.Wait();

            return saveTask.Result;
        }

        /// <summary>
        /// Method that groups duplicate data and makes data ready to save database.
        /// </summary>
        /// <param name="contactList">Represents distributed contact list read from xml file.</param>
        /// <returns></returns>
        private static IEnumerable<Contact> SharpenDataToSave(IEnumerable<Contact> contactList)
        {

            //Avoiding all duplicate data in contact list.
            var contactDataFromXml = contactList
                .GroupBy(item => new { item.Name, item.LastName, item.Phones })
                .Select(p => new
                {
                    Name = p.Key.Name,
                    LastName = p.Key.LastName,
                    Phone = p.Key.Phones.FirstOrDefault()
                })
                .ToList();

            var contactsToSave = new List<Contact>();


            //Iterating file data and merging phone data that belongs to same contact. 
            foreach (var item in contactDataFromXml)
            {
                if (!contactsToSave.Any(p => p.Name == item.Name && p.LastName == item.LastName))
                {
                    var phones = contactDataFromXml
                       .Where(p => p.Name == item.Name
                                && p.LastName == item.LastName)
                       .Select(p => p.Phone)
                       .Distinct()
                       .ToList();

                    contactsToSave.Add(new Contact
                    {
                        Name = item.Name,
                        LastName = item.LastName,
                        Phones = phones

                    });
                }

            }
            return contactsToSave;
        }

        /// <summary>
        /// Method that finds data from database due to matching criteria.
        /// </summary>
        /// <param name="name">Represents name property of Contact object.</param>
        /// <returns>Returns serialized Json collection</returns>
        public async Task<IList<string>> FindByName(string name)
        {
            var contactList = await _contactRepository.FindByName(name);

            return contactList.Select(p => p.ToString()).ToList();
        }

        /// <summary>
        /// Method that finds data from database due to matchinf criteria.
        /// </summary>
        /// <param name="name">Represents name property of Contact object.</param>
        /// <param name="lastName">Represents lastName property of the Contact object.</param>
        /// <returns></returns>
        public async Task<Contact> FindByNameAndLastName(string name,string lastName)
        {
            return await _contactRepository.FindByNameAndLastName(name,lastName);
        }

        #endregion

    }
}
