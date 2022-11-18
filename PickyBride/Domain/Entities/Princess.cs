using PickyBride.Domain.Entities.FriendEntity;

namespace PickyBride.Domain.Entities;

public class Princess
{
    // константы определения счастья
    private static readonly int AloneHappinessLevel = 10;
    private static readonly int BadHusbandHappinessLevel = 0;

    // константы качества женихов
    private static readonly int MaxHusbandQuality = 100;
    private static readonly int MediumHusbandQuality = MaxHusbandQuality / 2;

    private readonly Friend _friend;

    // мета для принятия решений
    private int _thingPeriod;
    private int _currentAttempt;
    private int _borderContenderId;

    // будущий муж и окончательный выбор (может быть null, что значит не вышла замуж)
    private Contender? _husband;

    public Princess(Friend friend)
    {
        _borderContenderId = -1;
        _friend = friend;
    }

    public void PreparePrincess(int countContenders)
    {
        _thingPeriod = Convert.ToInt32(countContenders / Math.E);
    }

    public int GetHappinessLevel()
    {
        if (_husband == null)
        {
            return AloneHappinessLevel;
        }

        if (_husband.ValQuality > MediumHusbandQuality)
        {
            return _husband.ValQuality;
        }

        return BadHusbandHappinessLevel;
    }

    public bool IsContenderFutureHusband(Contender contender)
    {
        _currentAttempt += 1;

        if (_thingPeriod >= _currentAttempt)
        {
            TrySetBorderContenderQuality(contender);
            _friend.TrackContender(contender);

            return false;
        }

        if (!_friend.IsContendersBetter(contender, _borderContenderId)) return false;

        _husband = contender;

        return true;
    }

    private void TrySetBorderContenderQuality(Contender contender)
    {
        if (_borderContenderId == -1)
        {
            _borderContenderId = contender.Id;

            return;
        }

        if (_friend.IsContendersBetter(contender, _borderContenderId))
        {
            _borderContenderId = contender.Id;
        }
    }
}