using StackExchange.Redis;

namespace PickyBride.Infrastructure.Db;

public class RedisConn
{
    private static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect(
        new ConfigurationOptions
        {
            EndPoints = { "localhost:6379" },
            Password = "123123"
        });

    public static IDatabase GetRedisDatabase()
    {
        return _redis.GetDatabase();
    }
}