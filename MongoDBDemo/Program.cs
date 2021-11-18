using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MongoDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creates MongoDB Collection (NOSQL Run-in: No-Structure enforced)
            //Groups all information in to a single collection and makes it more easier to find 
            //A single users information
            MongoCRUD db = new MongoCRUD("AddressBook");
            //IntelliAuth  App 
            MongoCRUD db2 = new MongoCRUD("IntelliAuthV1");

            //Inserting the new person model into the 'users' table after converting it into .BSON
            //.BSON Being Mongo's Interpretation of .JSON
            //Structures are not specific

            //Insert record w/o constructor 
            //db.InsertRecord("Users", new PersonModel { Firstname = "Andile", LastName = "Shaba" });
            //Console.ReadLine();

            //Insert w/object
            //db.InsertRecord("Users", new PersonModel 
            //{ 
            //    Firstname = "Andile", 
            //    LastName = "Shaba" ,
            //    PrimaryAddress = new AddressModel 
            //    { 
            //        StreetAddress = "59 Quail Avenue",
            //        City = "Pretoria",
            //        State = "GP",
            //        ZipCode = "18512"
            //    }
            //});

            var recs = db.LoadRecords<PersonModel>("Users");
            //OI
            var recs2 = db2.LoadRecords2<ApplicationUsers>("ApplicationUser");

            Console.WriteLine("User records from the Address Book: \n");
            foreach(var rec in recs)
            {
                Console.WriteLine($"{rec.Id}: { rec.Firstname } { rec.LastName }");
            }

            Console.WriteLine("\n User records from the Application Users in IntelliAuth DB:");
            foreach (var recs22 in recs2)
            {
                Console.WriteLine($"{recs22._id} - { recs22.FullName } - { recs22.Email } - " +
                    $"{ recs22.Phone } - { recs22.Address } - { recs22.Code } - { recs22.Password }");
            }
            Console.ReadLine();
        }
    }

    public class PersonModel
    { 
        //Stores field as ID 
        [BsonId]
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public AddressModel PrimaryAddress { get; set; }
    }
    public class AddressModel 
    { 
    public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class ApplicationUsers
    {
        public string _id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public Boolean IsBlocked { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public DateTime Created { get; set; }
        public string JwtToken { get; set; }
        public string JwtTokenCreated { get; set; }
        public string ResetToken { get; set; }
        public string ResetTokenCreated { get; set; }
        public string LastSignIn { get; set; }
        public object[] ApplicationRoles { get; set; }
    }

    //Performing updates to database
    public class MongoCRUD 
    {
        private IMongoDatabase db;

        public MongoCRUD(string database) 
        {
            //Initialize db prop and connects to database 
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        //Single record insertion into database passing value 't'.
        public void InsertRecord<T>(string table, T record) 
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        //Retreiving data from the database 
        public List<T> LoadRecords<T>(string table) 
        {
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        public List<T> LoadRecords2<T>(string table)
        {
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }
    }
}
