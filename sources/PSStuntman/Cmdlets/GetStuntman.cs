using Dapper;
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
                    WriteObject(stuntman);
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
    }
}