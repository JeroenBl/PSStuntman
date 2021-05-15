using System;

namespace PSStuntman.Models
{
    public class PersonModel
    {
        public int PersonId { get; set; }
        public string DisplayName { get; set; }
        public string ExternalId { get; set; }
        public NameModel Name { get; set; }
        public ContactModel Contact { get; set; }
        public DetailsModel Details { get; set; }


        public class NameModel
        {
            public string GivenName { get; set; }
            public string NickName { get; set; }
            public string FamilyName { get; set; }
        }

        public class ContactModel
        {
            public PersonalModel Personal { get; set; }
            public BusinessModel Business { get; set; }

            public class PersonalModel
            {
                public PersonalAddressModel Address { get; set; }
                public PersonalPhoneModel Phone { get; set; }
                public string Email { get; set; }

                public class PersonalAddressModel
                {
                    public string Locality { get; set; }
                    public string Street { get; set; }
                    public int HouseNumber { get; set; }
                    public string PostalCode { get; set; }
                }

                public class PersonalPhoneModel
                {
                    public string Fixed { get; set; }
                }
            }

            public class BusinessModel
            {
                public string Email { get; set; }
                public BusinessPhoneNumber Phone { get; set; }

                public class BusinessPhoneNumber
                {
                    public string Fixed { get; set; }
                }
            }
        }

        public class DetailsModel
        {
            public DateTime BirthDate { get; set; }
            public string BirthPlace { get; set; }
        }
    }
}