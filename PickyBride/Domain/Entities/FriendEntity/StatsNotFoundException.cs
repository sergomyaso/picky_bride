namespace PickyBride.Domain.Entities.FriendEntity;

public class StatsNotFoundException : ApplicationException
{
    public StatsNotFoundException()
    {
    }

    public StatsNotFoundException(string message) : base(message)
    {
    }
}