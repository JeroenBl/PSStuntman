using PSStuntman.Models;
using PSStuntman.Services;
using System;
using System.Management.Automation;

namespace PSStuntman.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Department")]
    [OutputType(typeof(DepartmentModel))]
    public class GetDepartment : PSCmdlet
    {
        private SqliteDataAccessService _sqliteDataAccessService;

        public GetDepartment()
        {
            _sqliteDataAccessService = new SqliteDataAccessService();
        }

        protected override void ProcessRecord()
        {
            try
            {
                var departments = _sqliteDataAccessService.ReadFromDatabase<DepartmentModel>("select * from Departments");
                if (departments.Count < 1)
                {
                    WriteObject($"Unable to obtain departments. Make sure the sqlite database not empty");
                }
                else
                {
                    WriteObject(departments);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Could not retrieve stuntman", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
