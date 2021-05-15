using Dapper;
using PSStuntman.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace PSStuntman.Cmdlets
{
    /// <summary>
    /// Retrieve stuntman from Sqlite
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Stuntman")]
    [OutputType(typeof(StuntmanModel))]
    public class GetStuntman : PSCmdlet
    {
        /// <summary>
        /// Retrieve a stuntman by UserID
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Retrieve a stuntman by UserID"
        )]
        public string UserId { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Convert the stuntman to a person object"
        )]
        public SwitchParameter ConvertToPerson
        {
            get { return isConvertToPerson; }
            set { isConvertToPerson = value; }
        }
        private bool isConvertToPerson;

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessRecord()
        {
            var query = string.IsNullOrEmpty(UserId) ? "select * from Stuntman" : $"select* from Stuntman where UserID = {UserId}"; 

            try
            {
                var stuntman = ListDataFromDatabase(query);
                if (stuntman.Count < 1)
                {
                    WriteObject($"Unable to obtain stuntman. Make sure the sqlite database is not empty and that the stuntman with id '{UserId}' exists.");
                }
                else
                {
                    if (isConvertToPerson)
                    {
                        var persons = ConvertToPersonModel(stuntman);
                        WriteObject(persons);
                    }
                    else
                    {
                        WriteObject(stuntman);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Could not get stuntman", ErrorCategory.NotSpecified, null));
            }
        }

        /// <summary>
        /// Private method to get data from Sqlite
        /// </summary>
        /// <returns></returns>
        private List<StuntmanModel> ListDataFromDatabase(string query)
        {
            var dllLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dbPath = Path.Combine(dllLocation);
            using (IDbConnection connection = new SQLiteConnection($"Data Source={dbPath}\\Stuntman.db"))
            {
                var output = connection.Query<StuntmanModel>(query, new DynamicParameters());
                WriteVerbose($"Retrieved {output.ToList().Count} Stuntman");
                return output.ToList();
            }
        }

        private List<PersonModel> ConvertToPersonModel(List<StuntmanModel> stuntmanList)
        {
            List<PersonModel> persons = new List<PersonModel>();
            foreach (var stuntman in stuntmanList)
            {
                persons.Add(new PersonModel
                {
                    PersonId = stuntman.UserId,
                    DisplayName = stuntman.DisplayName,
                    ExternalId = stuntman.ExternalId,

                    Name = new PersonModel.NameModel
                    {
                        GivenName = stuntman.GivenName,
                        FamilyName = stuntman.FamilyName,
                        NickName = stuntman.GivenName
                    },

                    Details = new PersonModel.DetailsModel
                    {
                        BirthDate = stuntman.BirthDate,
                        BirthPlace = stuntman.BirthPlace
                    },
                    
                    Contact = new PersonModel.ContactModel
                    {
                        Business = new PersonModel.ContactModel.BusinessModel
                        {
                            Email = stuntman.BusinessEmailAddress,
                            Phone = new PersonModel.ContactModel.BusinessModel.BusinessPhoneNumber
                            {
                                Fixed = stuntman.BusinessPhoneNumber
                            }
                        },

                        Personal = new PersonModel.ContactModel.PersonalModel
                        {
                            Address = new PersonModel.ContactModel.PersonalModel.PersonalAddressModel
                            {
                                Street = stuntman.Street,
                                HouseNumber = stuntman.HouseNumber,
                                Locality = stuntman.City,
                                PostalCode = stuntman.ZipCode
                            },
                            Email = stuntman.PersonalEmailAddress,
                            Phone = new PersonModel.ContactModel.PersonalModel.PersonalPhoneModel
                            {
                                Fixed = stuntman.PersonalPhoneNumber
                            }
                        }
                    }
                    
               });
            }

            return persons;
        }
    }
}