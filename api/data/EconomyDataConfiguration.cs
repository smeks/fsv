using API.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace API.Data
{
    public static class EconomyDataConfiguration
    {
        public static void Setup()
        {
            BsonClassMap.RegisterClassMap<Job>(cm =>
            {
                cm.AutoMap();
                cm.MapField(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Player>(cm =>
            {
                cm.AutoMap();
                cm.MapField(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Aircraft>(cm =>
            {
                cm.AutoMap();
                cm.MapField(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<CompatibleAircraft>(cm =>
            {
                cm.AutoMap();
                cm.MapField(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Airport>(cm =>
            {
                cm.AutoMap();
                cm.MapField(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
