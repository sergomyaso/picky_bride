using Microsoft.Extensions.Logging.Abstractions;
using PickyBride.Domain.Repository.ContenderRepository;

namespace PickyBride.Domain.Entities;

public class Hall
{
    private Queue<Contender>? _contenders;

    public Hall()
    {
        _contenders = null;
    }

    public void FillHall(Queue<Contender> contenders)
    {
        _contenders = contenders;
    }
    
    public Contender? GetNextContender()
    {
        if (_contenders == null || _contenders.Count == 0)
        {
            return null;
        }

        return _contenders.Dequeue();
    }
}