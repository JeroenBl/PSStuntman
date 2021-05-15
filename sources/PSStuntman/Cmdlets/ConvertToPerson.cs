using PSStuntman.Models;
using System.Collections.Generic;
using System.Management.Automation;

namespace PSStuntman.Cmdlets
{
    /// <summary>
    /// Retrieve stuntman from Sqlite
    /// </summary>
    [Cmdlet(VerbsData.ConvertTo, "Person")]
    [OutputType(typeof(PersonModel))]
    public class ConvertToPerson : PSCmdlet
    {
        /// <summary>
        /// Converts a stuntman to a person
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Converts a stuntman to a person",
            ValueFromPipeline = true
        )]
        public List<StuntmanModel> Stuntman { get; set; }

        protected override void ProcessRecord()
        {
            var persons = ConvertToPersonModel(Stuntman);
            WriteObject(persons);
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
