using Bogus;
using PSStuntman.Models;
using System;
using System.Collections.Generic;

namespace PSStuntman.Services
{
    public class StuntmanService
    {
        private Random _random;
        private Faker<StuntmanModel> _faker;

        public StuntmanService()
        {
            _random = new Random();
        }

        /// <summary>
        /// Public method to create the faker data
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="companyName"></param>
        /// <param name="domainName"></param>
        /// <param name="domainSuffix"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public List<StuntmanModel> CreateStuntman(int amount, string companyName, string domainName, string domainSuffix, string locale)
        {
            var _locale = string.IsNullOrEmpty(locale) ? $"nl" : locale;
            var _domainName = string.IsNullOrEmpty(domainName) ? $"enyoi" : domainName;
            var _domainSuffix = string.IsNullOrEmpty(domainSuffix) ? $".local" : $".{domainSuffix}";
            var _companyName = string.IsNullOrEmpty(companyName) ? GetRandomCompany() : companyName;

            var fakePersons = new List<StuntmanModel>();
            try
            {
                int userId = 0;
                _faker = new Faker<StuntmanModel>(_locale)
                    .CustomInstantiator(f => new StuntmanModel(userId++))

                    // User
                    .RuleFor(s => s.UserId, f => userId)
                    .RuleFor(s => s.ExternalId, (f, s) => $"STUNTMAN{userId}")
                    .RuleFor(s => s.GivenName, f => f.Person.FirstName)
                    .RuleFor(s => s.FamilyName, f => f.Person.LastName)
                    .RuleFor(s => s.DisplayName, (f, s) => $"{s.GivenName} {s.FamilyName}")
                    .RuleFor(s => s.UserName, (f, s) => $"{s.GivenName.Substring(0, 1)}.{s.FamilyName}@{_domainName}{_domainSuffix}")
                    .RuleFor(s => s.Initials, (f, s) => $"{s.GivenName.Substring(0, 1)}.{s.FamilyName.Substring(0, 1)}")
                    .RuleFor(s => s.PersonalEmailAddress, f => f.Person.Email)
                    .RuleFor(s => s.PersonalPhoneNumber, f => f.Person.Phone)
                    .RuleFor(s => s.BusinessEmailAddress, (f, s) => $"{s.GivenName.Substring(0, 1)}.{s.FamilyName}@{_companyName}.com")
                    .RuleFor(s => s.BusinessPhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(s => s.BirthDate, f => f.Person.DateOfBirth)
                    .RuleFor(s => s.BirthPlace, f => f.Address.City())
                    .RuleFor(s => s.Language, f => f.Locale)
                    .RuleFor(s => s.City, f => f.Address.City())
                    .RuleFor(s => s.Street, f => f.Address.StreetName())
                    .RuleFor(s => s.HouseNumber, (f, s) => (_random.Next(0, 200)))
                    .RuleFor(s => s.ZipCode, f => f.Address.ZipCode())
                    .RuleFor(s => s.IsActive, f => f.PickRandomParam(new int[] { 0, 1 }))
                    .RuleFor(s => s.UserGuid, f => Guid.NewGuid().ToString())

                    // Contract
                    .RuleFor(s => s.Title, f => f.Name.JobTitle())

                    // Assign a startDate between this day and 5 years back
                    .RuleFor(s => s.StartDate, f => f.Date.Past(5))

                    // Assign the endDate between this day and 5 years in the future
                    .RuleFor(s => s.EndDate, f => f.Date.Future(5))

                    .RuleFor(s => s.HoursPerWeek, f => f.PickRandom<int>(40, 30, 20, 16))
                    .RuleFor(s => s.Company, f => _companyName)
                    .RuleFor(s => s.Department, f => f.PickRandom<EnumDepartments>().ToString())
                    .RuleFor(s => s.CostCenter, (f, s) => $"{s.Department.Substring(0, 2).ToUpper()}")
                    .RuleFor(s => s.ContractGuid, f => Guid.NewGuid().ToString());

                fakePersons = _faker.Generate(amount);


                return fakePersons;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Assigns a random company if the CompanyName parameter is left empty
        /// </summary>
        /// <returns></returns>
        private string GetRandomCompany()
        {
            var companies = Enum.GetValues(typeof(EnumCompanies));
            var companyName = companies.GetValue(_random.Next(companies.Length)).ToString();

            return companyName;
        }

        /// <summary>
        /// ENUM that contains a predefined set of departments
        /// </summary>
        private enum EnumDepartments
        {
            Engineering,
            HR,
            RD,
            Support,
            Legal,
            Marketing,
            Accounting,
            Finance,
            IT,
            Staffing,
            Sales,
            Recruitment,
            Management
        }

        /// <summary>
        /// ENUM that contains a predefined set of company's
        /// </summary>
        private enum EnumCompanies
        {
            Bluejam,
            Buzzdog,
            Topiczoom,
            Realcube,
            Yodoo,
            Fivechat,
            Katz,
            Edgeify,
            Skibox,
            Kayveo,
            Skilith,
            Tavu,
            Browsebug,
            Kwimbee,
            Eabox,
            Quatz,
            Realpoint,
            Yata,
            Flashspan,
            Yadel,
            Eare,
            Divanoodle,
            Jabbersphere,
            Tagchat
        }
    }

}
