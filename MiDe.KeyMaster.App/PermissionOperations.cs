using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.Models;

namespace MiDe.KeyMaster.App
{
    public class PermissionOperations
    {
        private Db db;
        private ILogger logger;

        public PermissionOperations(Db db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        //CRUD 
        public bool Add(Permission permission)
        {
            var args = new Dictionary<string, object>
            {
                {"@keyId", permission.KeyId},
                {"@personId", permission.PersonId},
            };

            string sql = "insert into Permissions (KeyId, PersonId) values (@keyId, @personId)";
            var rowsAffected = db.ExecuteWrite(sql, args);

            return (rowsAffected > 0) ? true : false;
        }

        public Permission Search(Permission permission)
        {
            var args = new Dictionary<string, object>
            {
                {"@personId", permission.PersonId},
                {"@keyId", permission.KeyId},
            };

            string sql = "SELECT * FROM Permissions where PersonId= @personId and KeyId=@keyId";
            var dtTable = db.Execute(sql, args);

            if (dtTable == null || dtTable.Rows.Count == 0)
            {
                return null;
            }

            return permission;
        }

        public bool Exists(Permission permission)
        {
            var args = new Dictionary<string, object>
            {
                {"@keyId", permission.KeyId},
                {"@personId", permission.PersonId},
            };

            string sql = "SELECT * FROM Permissions where  KeyId= @keyId and PersonId = @personId";

            var dtTable = db.Execute(sql, args);
            return (dtTable.Rows.Count > 0) ? true : false;
        }

        public void Update(Permission permission)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Permission permission)
        {
            var args = new Dictionary<string, object>
            {
                {"@keyId", permission.KeyId},
                {"@personId", permission.PersonId},
            };

            string sql = "delete from Permissions where KeyId= @keyId and PersonId = @personId";
            var rowsAffected = db.ExecuteWrite(sql, args);
            return (rowsAffected > 0) ? true : false;
        }
    }
}
