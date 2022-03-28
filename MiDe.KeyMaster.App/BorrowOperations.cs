using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.Models;

namespace MiDe.KeyMaster.App
{
    public class BorrowOperations
    {
        private Db db;
        private ILogger logger;

        public BorrowOperations(Db db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        //CRUD 
        public bool Add(Borrow borrow)
        {
            var args = new Dictionary<string, object>
            {
                {"@keyId", borrow.KeyId},
                {"@personId", borrow.PersonId},
                {"@date", borrow.CreatedDate},
            };

            string sql = "insert into Borrow (KeyId, PersonId, Date) values (@keyId, @personId, @date)";
            var rowsAffected = db.ExecuteWrite(sql, args);

            return (rowsAffected > 0) ? true : false;
        }

        public Borrow SearchByKey(string keyId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", keyId},
            };

            string sql = "SELECT * FROM Borrow where KeyId= @id";
            var dtTable = db.Execute(sql, args);

            if (dtTable == null || dtTable.Rows.Count == 0)
            {
                return null;
            }

            var borrow = new Borrow
            {
                KeyId = Convert.ToString(dtTable.Rows[0]["KeyId"]),
                PersonId = Convert.ToString(dtTable.Rows[0]["PersonId"]),
            };

            return borrow;
        }

        public Borrow SearchByPerson(string personId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", personId},
            };

            string sql = "SELECT * FROM Borrow where PersonId= @id";
            var dtTable = db.Execute(sql, args);

            if (dtTable == null || dtTable.Rows.Count == 0)
            {
                return null;
            }

            var borrow = new Borrow
            {
                KeyId = Convert.ToString(dtTable.Rows[0]["KeyId"]),
                PersonId = Convert.ToString(dtTable.Rows[0]["PersonId"]),
            };

            return borrow;
        }

        public bool Exists(Borrow borrow)
        {
            var args = new Dictionary<string, object>
            {
                {"@keyId", borrow.KeyId},
                {"@personId", borrow.PersonId},
            };

            string sql = "SELECT * FROM Borrow where  KeyId= @keyId and PersonId = @personId";

            var dtTable = db.Execute(sql, args);
            return (dtTable.Rows.Count > 0) ? true : false;
        }

        public void Update(Borrow borrow)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string keyId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", keyId},
            };

            string sql = "delete from Borrow where KeyId= @id";
            var rowsAffected = db.ExecuteWrite(sql, args);
            return (rowsAffected > 0) ? true : false;
        }
    }
}
