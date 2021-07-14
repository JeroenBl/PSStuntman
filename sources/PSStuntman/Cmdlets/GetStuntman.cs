using PSStuntman.Models;
using PSStuntman.Services;
using System;
using System.Management.Automation;

namespace PSStuntman.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Stuntman")]
    [OutputType(typeof(StuntmanModel))]
    public class GetStuntman : PSCmdlet
    {
        private SqliteDataAccessService _sqliteDataAccessService;

        public GetStuntman()
        {
            _sqliteDataAccessService = new SqliteDataAccessService();
        }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Retrieve a stuntman by UserID"
        )]
        public string UserId { get; set; }

        protected override void ProcessRecord()
        {
            var query = string.IsNullOrEmpty(UserId) ? "select * from Stuntman" : $"select* from Stuntman where UserID = {UserId}";

            try
            {
                var stuntman = _sqliteDataAccessService.ReadFromDatabase<StuntmanModel>(query);
                if (stuntman.Count < 1)
                {
                    WriteObject($"Unable to obtain stuntman. Make sure the sqlite database is not empty and that the stuntman with id '{UserId}' exists.");
                }
                else
                {
                    WriteObject(stuntman);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Could not retrieve stuntman", ErrorCategory.InvalidOperation, null));
            }
        }
    }
}