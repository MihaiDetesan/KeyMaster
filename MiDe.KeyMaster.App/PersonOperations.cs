using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.Models;

namespace MiDe.KeyMaster.App
{
    public class PersonOperations
    {
        private Db db;
        private ILogger logger;

        public PersonOperations(Db db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        //CRUD 
        public bool Add(Person person)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", person.Id},
                {"@firstName", person.FirstName},
                {"@lastName", person.LastName}
            };

            string sql = "insert into Persons (Id, FirstName, LastName) values (@id, @firstName, @lastName)";
            var rowsAffected = db.ExecuteWrite(sql, args);

            return (rowsAffected > 0) ? true : false;
        }

        public Person Search(string personId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", personId},
            };

            string sql = "SELECT * FROM Persons where Id= @id";
            var dtTable = db.Execute(sql, args);

            if (dtTable == null || dtTable.Rows.Count == 0)
            {
                return null;
            }

            var person = new Person
            {
                Id = Convert.ToString(dtTable.Rows[0]["Id"]),
                FirstName = Convert.ToString(dtTable.Rows[0]["FirstName"]),
                LastName = Convert.ToString(dtTable.Rows[0]["LastName"]),
            };

            return person;
        }

        public bool Exists(string personId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", personId},
            };

            string sql = "SELECT * FROM Persons where Id= @id";
            var dtTable = db.Execute(sql, args);
            return (dtTable.Rows.Count > 0) ? true : false;
        }

        public bool Update(Person person)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", person.Id},
                {"@firstName", person.FirstName},
                {"@lastName", person.LastName},
            };

            string sql = "UPDATE Persons SET Id = @id, FirstName = @firstName, LastName = @lastName, WHERE Id = @id";
            var rowsAffected = db.ExecuteWrite(sql, args);
            return (rowsAffected > 0) ? true : false;
        }

        public bool Delete(string personId)
        {
            var args = new Dictionary<string, object>
            {
                {"@id", personId},
            };

            string sql = "delete from Persons where Id= @id";
            var rowsAffected = db.ExecuteWrite(sql, args);
            return (rowsAffected > 0) ? true : false;
        }

    }
}
