using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace MiDe.KeyMaster.App
{
    public class Db
    {
        private DbOptions options;
        private ILogger logger;

        public Db(DbOptions options, ILogger logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public int ExecuteWrite(string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;

            logger.LogTrace(query, args);

            using (var con = new SQLiteConnection($"Data Source={options.DatabaseName}"))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(query, con))
                {
                    foreach (var pair in args)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    logger.LogTrace(cmd.ToString());
                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                }

                return numberOfRowsAffected;
            }
        }

        public DataTable Execute(string query, Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(query.Trim()))
                return null;

            logger?.LogTrace(query, args);

            using (var con = new SQLiteConnection($"Data Source={options.DatabaseName}"))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(query, con))
                {
                    foreach (KeyValuePair<string, object> entry in args)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    logger.LogTrace(cmd.ToString());
                    var da = new SQLiteDataAdapter(cmd);

                    var dt = new DataTable();
                    da.Fill(dt);

                    da.Dispose();
                    return dt;
                }
            }
        }
    }
}
