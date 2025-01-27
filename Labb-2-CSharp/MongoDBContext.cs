using MongoDB.Driver;

namespace Labb_2_CSharp
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(string databaseName = "CarlKennedal")
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
