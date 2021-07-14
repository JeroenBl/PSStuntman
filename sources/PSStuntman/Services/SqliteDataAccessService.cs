using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PSStuntman.Services
{
    public class SqliteDataAccessService
    {
        public List<GenericModel> ReadFromDatabase<GenericModel>(string query)
        {
            var dllLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dbPath = Path.Combine(dllLocation);
            using (IDbConnection connection = new SQLiteConnection($"Data Source={dbPath}\\Stuntman.db"))
            {
                var output = connection.Query<GenericModel>(query, new DynamicParameters());
                return output.ToList();
            }
        }

        public void AddToDatabase(string query, object obj)
        {
            var dllLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dbPath = Path.Combine(dllLocation);
            using (IDbConnection connection = new SQLiteConnection($"Data Source={dbPath}\\Stuntman.db"))
            {
                connection.Execute(query, obj);
            }
        }
    }
}