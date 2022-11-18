namespace PickyBride.Domain.Entities.FriendEntity;

public class Friend
{
    private readonly Dictionary<int, int> _contendersStats;

    public Friend()
    {
        _contendersStats = new Dictionary<int, int>();
    }

    public bool IsContendersBetter(Contender curr, int compareWithContenderId)
    {
        if (!_contendersStats.ContainsKey(compareWithContenderId))
        {
            throw new StatsNotFoundException("stats by contender id not found");
        }

        var pastStats = _contendersStats[compareWithContenderId];

        return curr.ValQuality >= pastStats ? true : false;
    }


    public void TrackContender(Contender contender)
    {
        _contendersStats.Add(contender.Id, contender.ValQuality);
    }
}