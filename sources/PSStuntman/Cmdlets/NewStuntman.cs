using Bogus;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace PSStuntman.Cmdlets
{
    /// <summary>
    /// PSCmdlet for New-Stuntman
    /// </summary>
    [Cmdlet(VerbsCommon.New, "Stuntman")]
    [OutputType(typeof(StuntmanModel))]
    public class NewStuntman : PSCmdlet
    {
        private Random _random;
        private Faker<StuntmanModel> _faker;

        public NewStuntman()
        {
            _random = new Random();
        }

        /// <summary>
        /// The amount of stuntman you want to create, e.g. 10
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "The amount of stuntman you want to create e.g. 10"
        )]
        public int Amount { get; set; }

        /// <summary>
        /// The CompanyName. e.g. 'contoso.com'. When left empty, a random companyname will be picked
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "The CompanyName e.g. 'Contoso'. When left empty, a random CompanyName will be picked."
        )]
        public string CompanyName { get; set; }

        /// <summary>
        /// The DomainName. e.g. 'contoso.com'. The default DomainName is set to 'enyoi'
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "The DomainName e.g. 'contoso'. When left empty, the default DomainName will be set to 'enyoi'."
        )]
        public string DomainName { get; set; }

        /// <summary>
        /// The DomainSuffix e.g. '.com'
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "The DomainSuffix e.g. 'com'."
        )]
        public string DomainSuffix { get; set; }

        /// <summary>
        /// The locale for the stuntman e.g. 'fr' (for French) or 'en' (for English). The default locale is set to 'nl'. To find more locales: https://github.com/bchavez/Bogus
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "The locale for the stuntman e.g. 'fr' (for French) or 'en' (for English). The default locale is set to 'nl'. To find more locales: https://github.com/bchavez/Bogus"
        )]
        public string Locale { get; set; }

        /// <summary>
        /// Saves the generated Stuntman to a SQlite database. You will find it in root folder from where this module is loaded
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Saves the generated Stuntman to a SQlite database. You will find it in root folder from where this module is loaded."
        )]
        public SwitchParameter SaveToDatabase
        {
            get { return isSaveToDatabase; }
            set { isSaveToDatabase = value; }
        }
        private bool isSaveToDatabase;

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessRecord()
        {
            var locale = string.IsNullOrEmpty(Locale) ? $"nl" : Locale;
            var domainName = string.IsNullOrEmpty(DomainName) ? $"enyoi" : DomainName;
            var domainSuffix = string.IsNullOrEmpty(DomainSuffix) ? $"local" : $".{DomainSuffix}";
            var companyName = string.IsNullOrEmpty(CompanyName) ? GetRandomCompany() : CompanyName;

            try
            {
                WriteVerbose($"Generating '{Amount}' of stuntman using locale '{locale}'");
                var fakerData = CreateFakerData(Amount, domainName, DomainSuffix, companyName, locale);

                if (isSaveToDatabase)
                {
                    SaveFakerDataToDatabase(fakerData);
                }
                else
                {
                    WriteObject(fakerData);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Could not create stuntman", ErrorCategory.NotSpecified, null));
            }
        }

        /// <summary>
        /// Private method to create the faker data
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="domainName"></param>
        /// <param name="domainSuffix"></param>
        /// <param name="companyName"></param>
        /// <param name="locale"></param>
        /// <param name="userIdRange"></param>
        /// <returns></returns>
        private List<StuntmanModel> CreateFakerData(int amount, string domainName, string domainSuffix, string companyName, string locale)
        {
            var fakePersons = new List<StuntmanModel>();
            try
            {
                int userId = 0;
                _faker = new Faker<StuntmanModel>(locale)
                    .CustomInstantiator(f => new StuntmanModel(userId++))

                    // User
                    .RuleFor(s => s.UserId, f => userId)
                    .RuleFor(s => s.ExternalId, (f, s) => $"STUNTMAN{userId}")
                    .RuleFor(s => s.GivenName, f => f.Person.FirstName)
                    .RuleFor(s => s.FamilyName, f => f.Person.LastName)
                    .RuleFor(s => s.DisplayName, (f, s) => $"{s.GivenName} {s.FamilyName}")
                    .RuleFor(s => s.UserName, (f, s) => $"{s.GivenName.Substring(0, 1)}.{s.FamilyName}@{domainName}{domainSuffix}")
                    .RuleFor(s => s.Initials, (f, s) => $"{s.GivenName.Substring(0, 1)}.{s.FamilyName.Substring(0, 1)}")
                    .RuleFor(s => s.PersonalEmailAddress, f => f.Person.Email)
                    .RuleFor(s => s.PersonalPhoneNumber, f => f.Person.Phone)
                    .RuleFor(s => s.BusinessEmailAddress, (f, s) => $"{s.GivenName.Substring(0, 1)}.{s.FamilyName}@{companyName}.com")
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
                    .RuleFor(s => s.IsManager, (f, s) => SetManager(s.Title))

                    // Assign a startDate between this day and 5 years back
                    .RuleFor(s => s.StartDate, f => f.Date.Past(5))

                    // Assign the endDate between this day and 5 years in the future
                    .RuleFor(s => s.EndDate, f => f.Date.Future(5))

                    .RuleFor(s => s.HoursPerWeek, f => f.PickRandom<int>(40, 30, 20, 16))
                    .RuleFor(s => s.Company, f => companyName)
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
        /// Sets the IsManager bool to true when the jobTitle contains the word 'manager'
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private int SetManager(string title)
        {
            int isManager = 0;
            if (title.Contains("Manager"))
            {
                isManager = 1;
            }
            return isManager;
        }

        /// <summary>
        /// Private method that creates a new SQLite query and uses the SaveSqliteData() method to save the data
        /// </summary>
        /// <param name="users"></param>
        private void SaveFakerDataToDatabase(List<StuntmanModel> persons)
        {
            var query = "insert into Stuntman (UserId, ExternalId, GivenName, FamilyName, DisplayName, UserName, Initials, PersonalEmailAddress, PersonalPhoneNumber, BusinessEmailAddress, BusinessPhoneNumber, BirthDate, BirthPlace, Language, City, Street, HouseNumber, ZipCode, IsActive, UserGuid, Title, IsManager, StartDate, EndDate, HoursPerWeek, Company, Department, CostCenter, ContractGuid) " +
            "values (@UserId, @ExternalId, @GivenName, @FamilyName, @DisplayName, @UserName, @Initials, @PersonalEmailAddress, @PersonalPhoneNumber, @BusinessEmailAddress, @BusinessPhoneNumber, @BirthDate, @BirthPlace, @Language, @City, @Street, @HouseNumber, @ZipCode, @IsActive, @UserGuid, @Title, @IsManager, @StartDate, @EndDate, @HoursPerWeek, @Company, @Department, @CostCenter, @ContractGuid)";

            try
            {
                var dllLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var dbPath = Path.Combine(dllLocation);
                using (IDbConnection connection = new SQLiteConnection($"Data Source={dbPath}\\Stuntman.db"))
                {
                    connection.Execute(query, persons);
                    WriteVerbose($"Stuntman saved to: {dbPath}\\Stuntman.db");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ENUM that contains a predefined set of departments
        /// </summary>
        public enum EnumDepartments
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
        public enum EnumCompanies
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