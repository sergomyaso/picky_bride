using PickyBride.Domain.Entities;

namespace PickyBride.Domain.Repository.ContenderRepository;

public interface IContenderRepository
{
    Queue<Contender> GenerateContenders(GenerateContendersFilter filter);
    Queue<Contender>? GetAttemptContenders(int attempt);
    void SaveAttemptContenders(int attempt, Queue<Contender> contenders);
}