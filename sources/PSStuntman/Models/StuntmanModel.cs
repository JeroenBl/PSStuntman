using System;

namespace PSStuntman.Models
{
    /// <summary>
    /// The [User] model
    /// </summary>
    public class StuntmanModel
    {
        public StuntmanModel(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Empty ctor added because without it, Dapper throws an exception when retrieving users from the SQlite database.
        /// </summary>
        public StuntmanModel()
        {

        }

        // User
        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Initials { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public string BusinessEmailAddress { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public int IsActive { get; set; }
        public string UserGuid { get; set; }

        // Contract
        public string Title { get; set; }
        public int IsManager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HoursPerWeek { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public int DepartmentExternalId { get; set; }
        public string CostCenter { get; set; }
        public string ContractGuid { get; set; }
    }
}
