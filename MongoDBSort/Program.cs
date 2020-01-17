using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;

namespace MongoDBSort
{
    class Program
    {
        private static IMongoCollection<BsonDocument> collection;

        static void Main(string[] args)
        {
            MongoUrlBuilder mongoUrlBuilder = new MongoUrlBuilder();
            mongoUrlBuilder.Scheme = MongoDB.Driver.Core.Configuration.ConnectionStringScheme.MongoDB;
            mongoUrlBuilder.Server = new MongoServerAddress("localhost",27017);
            var clientSettings = mongoUrlBuilder.ToMongoUrl();
            
            MongoClient mongoClient = new MongoClient(clientSettings);
            //MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
            var db = mongoClient.GetDatabase("animaldb");
            collection = db.GetCollection<BsonDocument>("animals");

            var animalCollection = db.GetCollection<Animal>("animals");

            var filter = Builders<Animal>.Filter.Where(animal => animal.Name == "Rufus");
            var thing = db.GetCollection<Animal>("animals").Find(filter);
            
            var nonGeneric = Builders<BsonDocument>.Filter.Eq("Name", "Rufus");
            var sameThing = db.GetCollection<BsonDocument>("animals").Find(nonGeneric);

            var giraffeFilter = "{name: 'giraffe'}";
            var giraffe = db.GetCollection<Animal>("animals")
                .Find<Animal>(giraffeFilter)
                .FirstOrDefault();

            // generellt exempel
            var sort = Builders<Animal>.Sort.Ascending(animal => animal.Name); //Sortera alfabetiskt på name key/value pair
            var jsonFilter = "{}";  // alla dokument
            var result = animalCollection
                .Find<Animal>(jsonFilter)
                .Sort(sort);

            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            var document = collection.Find(new BsonDocument()).Project(projection);
            Animal cat = BsonSerializer.Deserialize<Animal>(document.First());

            var nyttAnonymtObjekt = new { X = "Rufus", HasTail = true };
        }

        void Example()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Key", "Value");
            var projection = Builders<BsonDocument>.Projection.Exclude("_id").Exclude("OtherKey");
            var findWithProjection = collection
            .Find(filter)
            .Project(projection);
        }
    }
}
