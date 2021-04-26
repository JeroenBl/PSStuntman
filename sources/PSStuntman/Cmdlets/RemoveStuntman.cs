using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace PSStuntman.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "Stuntman")]
    public class RemoveStuntman : Cmdlet
    {
        /// <summary>
        /// Remove a stuntman by UserID
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Remove a stuntman by UserID"
        )]
        public string UserId { get; set; }

        /// <summary>
        /// Empty database
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Removes all stuntman from the database."
        )]
        public SwitchParameter EmptyDatabase
        {
            get { return isEmptyDatabase; }
            set { isEmptyDatabase = value; }
        }
        private bool isEmptyDatabase;

        protected override void ProcessRecord()
        {
            try
            {
                if (EmptyDatabase)
                {
                    RemoveDataFromDatabase("delete from Stuntman");
                }

                if (UserId == "")
                {
                    RemoveDataFromDatabase($"delete from Stuntman where UserId = {UserId}");
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "Could not delete stuntman", ErrorCategory.NotSpecified, null));
            }
        }

        /// <summary>
        /// Private method to delete data from Sqlite
        /// </summary>
        /// <returns></returns>
        private void RemoveDataFromDatabase(string query)
        {
            var dllLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dbPath = Path.Combine(dllLocation);
            using (IDbConnection connection = new SQLiteConnection($"Data Source={dbPath}\\Stuntman.db"))
            {
                connection.Query(query);
            }
        }
    }
}
