using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.Models;

namespace MiDe.KeyMaster.App
{
    public class KeyOperations
    {
        public Db db;
        public ILogger logger;

        public KeyOperations(Db db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        //CRUD 
        public bool Add(Key key)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", key.Id},
                {"@description", key.Description}
            };

            string sql = "insert into Keys (Id, Description) values (@id, @description)";
            var rowsAffected = db.ExecuteWrite(sql, args);

            return (rowsAffected > 0) ? true : false;
        }

        public Key Search(string keyId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", keyId},
            };

            string sql = "SELECT * FROM Keys where Id= @id";
            var dtTable = db.Execute(sql, args);

            if (dtTable == null || dtTable.Rows.Count == 0)
            {
                return null;
            }

            var key = new Key
            {
                Id = Convert.ToString(dtTable.Rows[0]["Id"]),
                Description = Convert.ToString(dtTable.Rows[0]["Description"]),
            };

            return key;
        }

        public bool Exists(string keyId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", keyId},
            };

            string sql = "SELECT * FROM Keys where Id= @id";
            var dtTable = db.Execute(sql, args);
            return (dtTable.Rows.Count > 0) ? true : false;
        }

        public bool Update(Key key)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", key.Id},
                {"@description", key.Description}
            };

            string sql = "UPDATE Keys SET Id = @id, Description = @description WHERE Id = @id";
            var rowsAffected = db.ExecuteWrite(sql, args);
            return (rowsAffected > 0) ? true : false;
        }

        public bool Delete(string keyId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", keyId},
            };
            
            string sql = "delete from Keys where Id= @id";
            var rowsAffected = db.ExecuteWrite(sql, args);
            return (rowsAffected > 0) ? true : false;
        }

    }
}
