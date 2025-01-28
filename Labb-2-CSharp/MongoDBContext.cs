using MongoDB.Bson.Serialization;
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

            RegisterClassMaps();
        }

        private void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<Player>(cm =>
            {
                cm.AutoMap();
                cm.SetDiscriminator("Player");
            });

            BsonClassMap.RegisterClassMap<Rat>(cm =>
            {
                cm.AutoMap();
                cm.SetDiscriminator("Rat");
            });

            BsonClassMap.RegisterClassMap<Snake>(cm =>
            {
                cm.AutoMap();
                cm.SetDiscriminator("Snake");
            });

            BsonClassMap.RegisterClassMap<Wall>(cm =>
            {
                cm.AutoMap();
                cm.SetDiscriminator("Wall");
            });
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}