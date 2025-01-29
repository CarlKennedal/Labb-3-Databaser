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

        private static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Player)))
            {
                BsonClassMap.RegisterClassMap<Player>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("Player");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Wall)))
            {
                BsonClassMap.RegisterClassMap<Wall>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("Wall");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Rat)))
            {
                BsonClassMap.RegisterClassMap<Rat>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("Rat");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Snake)))
            {
                BsonClassMap.RegisterClassMap<Snake>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("Snake");
                });
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}