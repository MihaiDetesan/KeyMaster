using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.Models;

namespace MiDe.KeyMaster.App
{
    public class HistoryOperations
    {
        private Db db;
        private ILogger logger;

        public HistoryOperations(Db db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        //CRUD 
        public bool Add(Record record)
        {
            var args = new Dictionary<string, object>
            {
                {"@keyId", record.KeyId},
                {"@personId", record.PersonId},
                {"@borrowDate", record.BorrowDate.ToString()},
                {"@returnDate", record.ReturnDate.ToString()},
            };

            string sql = "insert into History (KeyId, PersonId, BorrowDate, ReturnDate) values (@keyId, @personId, @borrowDate, @returnDate)";
            var rowsAffected = db.ExecuteWrite(sql, args);

            return (rowsAffected > 0) ? true : false;
        }

        public Record Search(Record record)
        {
            throw new NotImplementedException();

            //var args = new Dictionary<string, object>
            //{
            //    {"@keyId", record.KeyId},
            //    {"@personId", record.PersonId},
            //    {"@borrowDate", record.BorrowDate.ToString()},
            //    {"@returnDate", record.ReturnDate.ToString()},
            //};

            //string sql = "SELECT * FROM History where PersonId= @personId and KeyId=@keyId";
            //var dtTable = db.Execute(sql, args);

            //if (dtTable == null || dtTable.Rows.Count == 0)
            //{
            //    return null;
            //}

            //return record;
        }

        public bool Exists(Record record)
        {
            throw new NotImplementedException();

            //var args = new Dictionary<string, object>
            //{
            //    {"@keyId", record.KeyId},
            //    {"@personId", record.PersonId},
            //    {"@borrowDate", record.BorrowDate.ToString()},
            //    {"@returnDate", record.ReturnDate.ToString()},
            //};

            //string sql = "SELECT * FROM Permissions where  KeyId= @keyId and PersonId = @personId and BorrowDate = @borrowDate and ReturnDate = @returnDate";

            //var dtTable = db.Execute(sql, args);
            //return (dtTable.Rows.Count > 0) ? true : false;
        }

        public void Update(Permission permission)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Permission permission)
        {
            throw new NotImplementedException();
            //var args = new Dictionary<string, object>
            //{
            //    {"@keyId", permission.KeyId},
            //    {"@personId", permission.PersonId},
            //};

            //string sql = "delete from Permissions where KeyId= @keyId and PersonId = @personId";
            //var rowsAffected = db.ExecuteWrite(sql, args);
            //return (rowsAffected > 0) ? true : false;
        }
    }
}
