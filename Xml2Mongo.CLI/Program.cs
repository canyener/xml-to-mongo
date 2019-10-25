using System;
using System.Text;
using Xml2Mongo.Services;

namespace Xml2Mongo.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ContactService contactService = new ContactService();


            if (args.Length > 0)
            {
                do
                {
                    Operations(args, contactService);
                } while (args[0] != "--exit");
            }
            else
            {
                Console.WriteLine("Welcome to Xml2Mongo, --help for available commands.");

                string operation;
                do
                {

                    //Seperates operation command and the given argument and runs related operation.
                    operation = Console.ReadLine();
                    var operations = operation.Split(' ');

                    if (operations.Length > 1)
                    {
                        Operations(operations, contactService);
                    }
                    else if (operation == "--help")
                    {
                        Console.WriteLine(GetHelpText());
                    }
                } while (operation != "--exit");
            }

        }

        /// <summary>
        /// Runs the operation due to given command.
        /// </summary>
        /// <param name="args">Represents command arguments entered by user.</param>
        /// <param name="contactService">Service instance that runs essential methods for business logic operations</param>
        private static void Operations(string[] args, ContactService contactService)
        {
            switch (args[0])
            {
                case "--help":
                    {
                        Console.WriteLine(GetHelpText());
                    }; break;
                case "--import":
                    {
                        var xmlPath = args[1];

                        var isImported = contactService.ImportToDatabase(xmlPath.Trim());

                        if (isImported)
                        {
                            Console.WriteLine("Import operation successful!");
                        }
                        else
                        {
                            Console.WriteLine("An error occured while importing file. Please try again later.");
                        }
                    }; break;
                case "--find":
                    {
                        var resultTask = contactService.FindByName(args[1]);

                        resultTask.Wait();

                        var results = resultTask.Result;

                        if (results.Count > 0)
                        {
                            foreach (var item in resultTask.Result)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found due to matching criteria.");
                        }

                    }; break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns available command lists to user.
        /// </summary>
        /// <returns></returns>
        private static string GetHelpText()
        {
            var sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append(@"--import <filepath> :");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(@"    Imports the xml file at selected path to database. i.e : --import C:\Documents\myfile.xml");

            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(@"--find <name> :");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(@"    Returns contact data from database that name criteria conforms. This command is case-sensitive. i.e. : --find James");

            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(@"--exit :");
            sb.Append(Environment.NewLine);
            sb.Append(@"    Quits the application.");

            return sb.ToString();
        }
    }
}
