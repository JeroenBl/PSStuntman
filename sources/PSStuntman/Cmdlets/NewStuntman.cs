using PSStuntman.Models;
using PSStuntman.Services;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PSStuntman.Cmdlets
{
    /// <summary>
    /// PSCmdlet for New-Stuntman
    /// </summary>
    [Cmdlet(VerbsCommon.New, "Stuntman")]
    [OutputType(typeof(StuntmanModel))]
    public class NewStuntman : PSCmdlet
    {
        private StuntmanService _stuntmanService;
        private DepartmentService _departmentService;
        private SqliteDataAccessService _sqliteDataAccessService;

        public NewStuntman()
        {
            _stuntmanService = new StuntmanService();
            _departmentService = new DepartmentService();
            _sqliteDataAccessService = new SqliteDataAccessService();
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
        /// Saves the generated Stuntman to the database. The database is located in the root folder from where this module is loaded
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Saves the generated Stuntman to the database. The database is located in the root folder from where this module is loaded."
        )]
        public SwitchParameter SaveToDatabase
        {
            get { return isSaveToDatabase; }
            set { isSaveToDatabase = value; }
        }
        private bool isSaveToDatabase;

        /// <summary>
        /// "Creates departments for use in HelloID and promotes the first stuntman in the department to manager"
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Creates departments for use in HeloID and promotes the first stuntman in the department to manager."
        )]
        public SwitchParameter CreateDepartments
        {
            get { return isCreateDepartments; }
            set { isCreateDepartments = value; }
        }
        private bool isCreateDepartments;

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                List<DepartmentModel> departments = null;
                WriteVerbose($"Generating '{Amount}' of stuntman using locale '{Locale}'");
                var stuntmam = _stuntmanService.CreateStuntman(Amount, CompanyName, DomainName, DomainSuffix, Locale);

                if (isCreateDepartments)
                {
                    WriteVerbose("Creating departments for use in HelloID");
                    departments = _departmentService.CreateDepartments(stuntmam);
                }

                if (isSaveToDatabase)
                {
                    SaveStuntmanToDatabase(stuntmam);
                    if (isCreateDepartments)
                    {
                        SaveDepartmentsToDatabase(departments);
                    }
                }
                else
                {
                    WriteObject(stuntmam);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Could not create stuntman", ErrorCategory.InvalidOperation, null));
            }
        }

        public void SaveStuntmanToDatabase(List<StuntmanModel> persons)
        {
            var query = "insert into Stuntman (UserId, ExternalId, GivenName, FamilyName, DisplayName, UserName, Initials, PersonalEmailAddress, PersonalPhoneNumber, BusinessEmailAddress, BusinessPhoneNumber, BirthDate, BirthPlace, Language, City, Street, HouseNumber, ZipCode, IsActive, UserGuid, Title, IsManager, StartDate, EndDate, HoursPerWeek, Company, Department, CostCenter, ContractGuid) " +
            "values (@UserId, @ExternalId, @GivenName, @FamilyName, @DisplayName, @UserName, @Initials, @PersonalEmailAddress, @PersonalPhoneNumber, @BusinessEmailAddress, @BusinessPhoneNumber, @BirthDate, @BirthPlace, @Language, @City, @Street, @HouseNumber, @ZipCode, @IsActive, @UserGuid, @Title, @IsManager, @StartDate, @EndDate, @HoursPerWeek, @Company, @Department, @CostCenter, @ContractGuid)";

            _sqliteDataAccessService.AddToDatabase(query, persons);
        }

        public void SaveDepartmentsToDatabase(List<DepartmentModel> departments)
        {
            var query = "insert into Departments (ExternalId, DisplayName, ManagerExternalId) " +
            "values (@ExternalId, @DisplayName, @ManagerExternalId)";

            _sqliteDataAccessService.AddToDatabase(query, departments);
        }
    }
}